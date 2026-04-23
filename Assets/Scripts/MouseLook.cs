using Photon.Pun;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float speed = 2f;
    public Transform cameraTransform;
    public Transform gunTransform;
    public float mouseSensitivity = 2f;
    private float verticalRotation = 0f;
    public PhotonView photonView;

    private GameOverManager gameOverManager;
    private PauseManager pauseManager;

    void Start()
    {
        gameOverManager = FindAnyObjectByType<GameOverManager>();
        pauseManager = FindAnyObjectByType<PauseManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // estar online y photonview no nos pertenece
        if (PhotonNetwork.InRoom && !photonView.IsMine)
        {
            cameraTransform.gameObject.SetActive(false);
            return;
        }

        if (gameOverManager.isPaused) return;
        if (pauseManager.isPaused) return;

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        transform.Rotate(Vector3.up, mouseX * mouseSensitivity);
        verticalRotation -= mouseY * mouseSensitivity;
        verticalRotation = Mathf.Clamp(verticalRotation, -80f, 80f);
        cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
        gunTransform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
    }
}
