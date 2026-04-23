using UnityEngine;
using Photon.Pun;

public class MovePlayer : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float speed;
    public float jumpForce = 1f;
    public float gravity = -9.81f;

    public float walkSpeed = 5f;
    public float runSpeed = 10f;

    private CharacterController controller;
    private Vector3 velocity;
    private GameOverManager gameOverManager;
    public PhotonView photonView;
    void Start()
    {
        controller = GetComponent<CharacterController>();
        gameOverManager = FindAnyObjectByType<GameOverManager>();
    }

    void Update()
    {
        // estar online y photonview no nos pertenece
        if (PhotonNetwork.InRoom && !photonView.IsMine)
        {
            return;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        bool isGroundedCustom = controller.isGrounded || Physics.Raycast(transform.position, Vector3.down, 1.2f);

        Vector3 move = transform.right * x + transform.forward * z;

        if (Input.GetKey(KeyCode.LeftShift) && isGroundedCustom)
        {
            speed = runSpeed;
        }
        else
        {
            speed = walkSpeed;
        }

        controller.Move(move * speed * Time.deltaTime);

        if (isGroundedCustom && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (Input.GetButtonDown("Jump") && isGroundedCustom)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * Physics.gravity.y);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Zombie"))
        {
            Debug.Log("Player hit by zombie!");
            if (gameOverManager != null)
            {
                gameOverManager.ShowMenu();
            }
        }
    }
}
