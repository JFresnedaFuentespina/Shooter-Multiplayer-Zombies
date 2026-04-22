using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float speed = 5f;
    public float jumpForce = 1f;

    private CharacterController controller;
    private Vector3 velocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);
        bool isGroundedCustom = controller.isGrounded || Physics.Raycast(transform.position, Vector3.down, 1.2f);

        if (isGroundedCustom && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (Input.GetButtonDown("Jump") && isGroundedCustom)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * Physics.gravity.y);
        }

        if (isGroundedCustom)
        {
            Debug.Log("Player is grounded");
        }
        else
        {
            Debug.Log("Player is not grounded");
        }

        velocity.y += Physics.gravity.y * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
