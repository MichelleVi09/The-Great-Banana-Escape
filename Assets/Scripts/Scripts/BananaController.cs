using UnityEngine;
using UnityEngine.InputSystem;   //Input System

public class BananaController : MonoBehaviour
{
    //Components / refs
    private CharacterController Controller;
    public Transform Cam;                     //drag Main Camera

    //Movement
    public float Speed = 5f;                  // units/sec
    public float jumpHeight = 1.5f;
    public float gravityValue = -9.81f;
    private Vector3 playerVelocity;

    //Input Actions 
    [Header("Input Actions")]
    public InputActionReference moveAction;   
    public InputActionReference jumpAction;   

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
        //grounding and gravity 
        bool grounded = Controller.isGrounded;
        if (grounded && playerVelocity.y < 0f)
            playerVelocity.y = -2f; //so we don't bounce 

        //to read input 
        Vector2 input = moveAction ? moveAction.action.ReadValue<Vector2>() : Vector2.zero;

        //camera relative movement 
        Vector3 camRight = Cam.right;
        Vector3 camFwd = Cam.forward;
        camRight.y = 0f; camFwd.y = 0f;
        camRight.Normalize(); camFwd.Normalize();

        Vector3 Movement = camRight * input.x + camFwd * input.y;

        if (Movement.sqrMagnitude > 1f) Movement.Normalize();

        //applying horizontal movement 
        Controller.Move(Movement * Speed * Time.deltaTime);

        //rotating camera 
        if (Movement.sqrMagnitude > 0.0001f)
        {
            float sens = Cam.GetComponent<CameraFollow>().sensivity;
            transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * sens * Time.deltaTime);

            Quaternion CamRotation = Cam.rotation;
            CamRotation.x = 0f;
            CamRotation.z = 0f;
            transform.rotation = Quaternion.Lerp(transform.rotation, CamRotation, 0.1f);
        }

        //for jumping
        if (jumpAction && jumpAction.action.triggered && grounded)
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravityValue);

        //for gravity 
        playerVelocity.y += gravityValue * Time.deltaTime;
        Controller.Move(playerVelocity * Time.deltaTime);
    }
}
