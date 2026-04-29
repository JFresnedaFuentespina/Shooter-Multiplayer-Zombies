using UnityEngine;
using Photon.Pun;
using TMPro;

public class MovePlayer : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float speed;
    public float jumpForce = 1f;
    public float gravity = -9.81f;

    public float walkSpeed = 5f;
    public float runSpeed = 10f;

    public float health = 100f;

    private CharacterController controller;
    private Vector3 velocity;
    private GameOverManager gameOverManager;
    public PhotonView photonView;
    public TextMeshProUGUI healthText;
    void Start()
    {
        controller = GetComponent<CharacterController>();
        gameOverManager = FindAnyObjectByType<GameOverManager>();
        healthText.text = health.ToString();
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
            Hit(20f);
        }
    }

    [PunRPC]
    public void Hit(float damage)
    {
        if (PhotonNetwork.InRoom)
        {
            photonView.RPC("PlayerTakeDamage", RpcTarget.All, damage, photonView.ViewID);
        }
        else
        {
            PlayerTakeDamage(damage, photonView.ViewID);
        }
    }

    public void PlayerTakeDamage(float damage, int viewId)
    {
        if (photonView.ViewID == viewId)
        {
            health -= damage;
            healthText.text = health.ToString();
            if (health <= 0f)
            {
                Die();
            }
        }
    }

    private void Die()
    {
        // Handle player death logic here
        if (gameOverManager != null)
        {
            gameOverManager.ShowMenu();
        }
        Destroy(gameObject);
    }
}
