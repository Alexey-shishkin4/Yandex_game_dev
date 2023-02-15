using UnityEngine;

[RequireComponent(typeof(HingeJoint2D))]
public class Catcher : MonoBehaviour
{
    private HingeJoint2D _joint;
    private bool connected = true;

    private void Start()
    {
        _joint = GetComponent<HingeJoint2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent(out Player player) & !connected)
        {
            _joint.enabled = true;
            connected = true;
        }

    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _joint.enabled = false;
            connected = false;
        }
    }
}
