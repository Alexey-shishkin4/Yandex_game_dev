using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Slider : MonoBehaviour
{
    private const float MinMoveDistance = 0.001f;
    private const float ShellRadius = 0.01f;
    [SerializeField] private float _minGroundNormalY = .65f;
    [SerializeField] private float _gravityModifier = 1f;
    [SerializeField] private Vector2 _velocity;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private float _speed;

    private Rigidbody2D _rb2d;

    private Vector2 _groundNormal;
    private Vector2 _targetVelocity;
    private ContactFilter2D _contactFilter;
    private RaycastHit2D[] _hitBuffer = new RaycastHit2D[16];
    private List<RaycastHit2D> _hitBufferList = new List<RaycastHit2D>(16);
    private bool isJump = false;

    private void OnEnable()
    {
        _rb2d = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        _contactFilter.useTriggers = false;
        _contactFilter.SetLayerMask(_layerMask);
        _contactFilter.useLayerMask = true;
    }

    private void Update()
    {
        Vector2 alongSurface = Vector2.Perpendicular(_groundNormal);
        if ((_rb2d.position + alongSurface)[1] > _rb2d.position[1])
        {
            alongSurface *= -1;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            isJump = true;
        }

        _targetVelocity = alongSurface * _speed;
    }

    private void FixedUpdate()
    {
        if (isJump)
        {
            _velocity = GetJumpVector();
            isJump = false;
        }
        else
        {
            _velocity += _gravityModifier * Physics2D.gravity * Time.deltaTime;
            _velocity.x = _targetVelocity.x;
        }


        Vector2 deltaPosition = _velocity * Time.deltaTime;
        Vector2 moveAlongGround = new Vector2(_groundNormal.y, -_groundNormal.x);
        Vector2 move = moveAlongGround * deltaPosition.x;

        Movement(move, false);

        move = Vector2.up * deltaPosition.y;
        Movement(move, true);
    }

    private void Movement(Vector2 move, bool yMovement)
    {
        float distance = move.magnitude;

        if (distance > MinMoveDistance)
        {
            int count = _rb2d.Cast(move, _contactFilter, _hitBuffer, distance + ShellRadius);

            _hitBufferList.Clear();

            for (int i = 0; i < count; i++)
            {
                _hitBufferList.Add(_hitBuffer[i]);
            }

            for (int i = 0; i < _hitBufferList.Count; i++)
            {
                Vector2 currentNormal = _hitBufferList[i].normal;
                if (currentNormal.y > _minGroundNormalY)
                {
                    if (yMovement)
                    {
                        _groundNormal = currentNormal;
                        currentNormal.x = 0;
                    }
                }

                float projection = Vector2.Dot(_velocity, currentNormal);
                if (projection < 0)
                {
                    _velocity = _velocity - projection * currentNormal;
                }

                float modifiedDistance = _hitBufferList[i].distance - ShellRadius;
                distance = modifiedDistance < distance ? modifiedDistance : distance;
            }
        }
        _rb2d.position = _rb2d.position + move.normalized * distance;
    }

    private Vector2 GetJumpVector()
    {
        float y1 = _rb2d.position[1] + (float)(_speed * 0.8) +(float)(9.81 * (0.8 * 0.8) / 2);
        float x1 = _rb2d.position[0] + (float)(_speed * 0.8) + (float)(9.81 * (0.8 * 0.8) / 2);
        return new Vector2(x1 - _rb2d.position[0], y1 - _rb2d.position[1]);
    }
}