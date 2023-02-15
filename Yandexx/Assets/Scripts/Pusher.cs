using UnityEngine;

public class Pusher : MonoBehaviour
{
    private Rigidbody2D body;
    private float swingForce = 25;
    private RaycastHit2D hit;
    private float last;

    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    private Vector3 GetDirection()
    {
        return Vector3.Cross(hit.point - body.position, Vector3.forward).normalized;
    }

    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
       
        if (horizontalInput > 0)
            horizontalInput = 1;
        else if (horizontalInput < 0)
            horizontalInput = -1;
        
        if (last != horizontalInput && horizontalInput != 0)
        {
            body.AddForce(GetDirection() * horizontalInput * swingForce, ForceMode2D.Impulse);
        }

        if (horizontalInput != 0) 
            last = horizontalInput;
    }
}
