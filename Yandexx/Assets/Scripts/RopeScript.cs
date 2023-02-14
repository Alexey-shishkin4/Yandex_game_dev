using UnityEngine;

public class RopeScript : MonoBehaviour
{
    private HingeJoint2D _joint;

    private void Start()
    {
        _joint = GetComponent<HingeJoint2D>();

    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //Debug.Log("Pressed primary button.");
            _joint.enabled = false;
        }
    }
}
