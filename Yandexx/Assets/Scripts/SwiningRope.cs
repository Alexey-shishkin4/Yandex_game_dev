using System.Collections;
using UnityEngine;

public class SwiningRope : MonoBehaviour
{
    private Rigidbody2D body;
    private float swingForce = 25;
    private RaycastHit2D hit;
    private float last;

    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    Vector3 GetDirection()
    {
        return Vector3.Cross(hit.point - body.position, Vector3.forward).normalized;
    }

    private void Update()
    {
        float h = Input.GetAxis("Horizontal");
        if (h > 0) h = 1; else if (h < 0) h = -1;

        if (last != h && h != 0)
        {
            body.AddForce(GetDirection() * h * swingForce, ForceMode2D.Impulse);
        }

        if (h != 0) last = h;
    }
}
