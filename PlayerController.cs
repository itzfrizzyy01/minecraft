// PlayerController.cs
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour {
    public float walkSpeed = 5f;
    public float gravity = -9.81f;
    public float jumpSpeed = 5f;
    public Camera playerCamera;
    public float lookSpeed = 2f;
    private CharacterController cc;
    private Vector3 velocity;
    private float pitch = 0f;

    void Start() {
        cc = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        if (playerCamera == null) playerCamera = GetComponentInChildren<Camera>();
    }

    void Update() {
        // Mouse look
        float mx = Input.GetAxis("Mouse X") * lookSpeed;
        float my = Input.GetAxis("Mouse Y") * lookSpeed;
        transform.Rotate(Vector3.up, mx);
        pitch -= my;
        pitch = Mathf.Clamp(pitch, -85f, 85f);
        playerCamera.transform.localEulerAngles = new Vector3(pitch, 0, 0);

        // Movement
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;
        cc.Move(move * walkSpeed * Time.deltaTime);

        // Gravity/jump
        if (cc.isGrounded && velocity.y < 0) velocity.y = -2f;
        if (Input.GetButtonDown("Jump") && cc.isGrounded) velocity.y = jumpSpeed;
        velocity.y += gravity * Time.deltaTime;
        cc.Move(velocity * Time.deltaTime);

        // Interact (place/break)
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)) {
            Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width/2, Screen.height/2));
            if (Physics.Raycast(ray, out RaycastHit hit, 8f)) {
                // Simple interaction using point hit
                if (Input.GetMouseButtonDown(0)) {
                    // break: spawn particle, remove block (placeholder)
                    Debug.Log("Break at: " + hit.point);
                } else {
                    // place block at adjacent position
                    Debug.Log("Place near: " + hit.point);
                }
            }
        }
    }
}
