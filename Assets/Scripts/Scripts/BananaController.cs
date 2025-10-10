using UnityEngine;
using UnityEngine.InputSystem;   // <-- new Input System

public class BananaController : MonoBehaviour
{
    // Components / refs
    private CharacterController Controller;
    public Transform Cam;                     // drag Main Camera here (has CameraFollow)

    // Movement
    public float Speed = 5f;                  // units/sec
    public float jumpHeight = 1.5f;
    public float gravityValue = -9.81f;
    private Vector3 playerVelocity;

    // Input Actions (assign in Inspector)
    [Header("Input Actions")]
    public InputActionReference moveAction;   // Vector2
    public InputActionReference jumpAction;   // Button

    void Awake()
    {
        Controller = GetComponent<CharacterController>();   // uses existing CC
    }

    void OnEnable()
    {
        if (moveAction) moveAction.action.Enable();
        if (jumpAction) jumpAction.action.Enable();
    }

    void OnDisable()
    {
        if (moveAction) moveAction.action.Disable();
        if (jumpAction) jumpAction.action.Disable();
    }

    void Update()
    {
        // --- Grounding & gravity ---
        bool grounded = Controller.isGrounded;
        if (grounded && playerVelocity.y < 0f)
            playerVelocity.y = -2f; // small stick force so we don't bounce

        // --- Read input (Vector2) ---
        Vector2 input = moveAction ? moveAction.action.ReadValue<Vector2>() : Vector2.zero;

        // --- Camera-relative movement (matches your original) ---
        // NOTE: do NOT multiply input by Time.deltaTime here; we apply it once in Move().
        Vector3 camRight = Cam.right;
        Vector3 camFwd = Cam.forward;
        camRight.y = 0f; camFwd.y = 0f;
        camRight.Normalize(); camFwd.Normalize();

        Vector3 Movement = camRight * input.x + camFwd * input.y;

        if (Movement.sqrMagnitude > 1f) Movement.Normalize();

        // --- Apply horizontal movement ---
        Controller.Move(Movement * Speed * Time.deltaTime);

        // --- Rotate to camera / mouse like your original (only while moving) ---
        if (Movement.sqrMagnitude > 0.0001f)
        {
            // Use CameraFollow.sensivity as you had
            float sens = Cam.GetComponent<CameraFollow>().sensivity;
            transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * sens * Time.deltaTime);

            Quaternion CamRotation = Cam.rotation;
            CamRotation.x = 0f;
            CamRotation.z = 0f;
            transform.rotation = Quaternion.Lerp(transform.rotation, CamRotation, 0.1f);
        }

        // --- Jump ---
        if (jumpAction && jumpAction.action.triggered && grounded)
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravityValue);

        // --- Gravity ---
        playerVelocity.y += gravityValue * Time.deltaTime;
        Controller.Move(playerVelocity * Time.deltaTime);
    }
}
