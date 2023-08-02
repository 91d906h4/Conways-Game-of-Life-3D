using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Transform mainCamera;

    Rigidbody rb;

    float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    int MoveForce = 20;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }

    // Update is called once per frame
    void Update()
    {
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + mainCamera.eulerAngles.y;
            float smothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

            transform.rotation = Quaternion.Euler(0f, smothAngle, 0f);
        }


        // Get Camera direction.
        Vector3 forward = Camera.main.transform.forward;
        Vector3 right = Camera.main.transform.right;

        // Fixed the Y axis movement.
        forward.y = 0;
        right.y = 0;

        // Normalize the vector.
        forward = forward.normalized;
        right = right.normalized;

        Vector3 forward_relation = vertical * forward;
        Vector3 right_relation = horizontal * right;

        Vector3 movement = forward_relation + right_relation;

        // Make movement.
        rb.velocity = new Vector3(movement.x * MoveForce, rb.velocity.y, movement.z * MoveForce);
    }

    private void FixedUpdate()
    {
    }
}
