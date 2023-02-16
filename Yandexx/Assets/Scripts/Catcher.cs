using System.Collections;
using UnityEngine;

[RequireComponent(typeof(HingeJoint2D))]
public class Catcher : MonoBehaviour
{
    private HingeJoint2D _joint;
    private Player player1;
    private bool _canCatch = true;
    [SerializeField] private bool _playerConnected;

    private void Start()
    {
        _joint = GetComponent<HingeJoint2D>();
        player1 = GameObject.Find("Player").GetComponent<Player>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_canCatch)
        {
            if (other.gameObject.TryGetComponent(out Player player) & !player1.Connected)
            {
                _playerConnected = true;
                _joint.enabled = true;
                player1.Connected = true;
            }
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (_playerConnected)
            {
                _joint.enabled = false;
                player1.Connected = false;
                _canCatch = false;
                StartCoroutine(Waiting());
            }
        }
    }
    private IEnumerator Waiting()
    {
        yield return new WaitForSeconds(1f);
        _canCatch = true;
    }
}
