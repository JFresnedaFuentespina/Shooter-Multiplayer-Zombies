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

    public CharacterController controller;
    private Vector3 velocity;
    public PhotonView photonView;
    public TextMeshProUGUI healthText;
    private Player player;
    void Start()
    {
        controller = GetComponent<CharacterController>();
        player = GetComponent<Player>();
        healthText.text = health.ToString();

        if (PhotonNetwork.InRoom && !photonView.IsMine)
        {
            controller.enabled = false;
        }
    }

    void Update()
    {
        if(player.currentState == Player.PlayerState.DEAD) return;
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
        if (!photonView.IsMine) return;
        if(player.currentState == Player.PlayerState.DEAD) return;

        if (collision.gameObject.CompareTag("Zombie"))
        {
            photonView.RPC("PlayerTakeDamage", RpcTarget.All, 20f, photonView.ViewID);
        }
    }

    [PunRPC]
    public void PlayerTakeDamage(float damage, int viewId)
    {
        if (photonView.ViewID == viewId)
        {
            health -= damage;
            healthText.text = health.ToString();
            if (health <= 0f)
            {
                player.currentState = Player.PlayerState.DEAD;
                controller.enabled = false;
            }
        }
    }
}
