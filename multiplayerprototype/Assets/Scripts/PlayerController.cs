using UnityEngine;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;

public class PlayerController : NetworkBehaviour
{
    public float walkSpeed = 5f;
    public float jumpSpeed = 5f;
    public float gravity = 9.81f;
    public float lookSpeed = 2f;
    public float lookXLimit = 60f;
    public float crouchHeight = 1f;
    public float standingHeight = 2f;
    public Camera playerCamera;
    public GameObject armsModel;

    private CharacterController characterController;
    private float rotationX = 0;
    private Vector3 velocity;
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        if (!IsLocalPlayer)
        {
            playerCamera.gameObject.SetActive(false);
            armsModel.SetActive(false);
        }
    }

    void Update()
    {
        if (!IsLocalPlayer) return;  
        float moveZ = Input.GetAxis("Vertical");   
        float moveX = Input.GetAxis("Horizontal"); 
        bool wantsToJump = Input.GetButton("Jump"); 
        bool crouch = Input.GetKey(KeyCode.LeftControl);

        rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        if (crouch)
        {
            characterController.height = crouchHeight;
            characterController.center = new Vector3(0, crouchHeight / 2, 0);
        }
        else
        {
            characterController.height = standingHeight;
            characterController.center = new Vector3(0, standingHeight / 2, 0);
        }

        float speed = walkSpeed;
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        if (characterController.isGrounded)
        {
            velocity.y = 0f; 
            if (wantsToJump)
            {
                velocity.y = jumpSpeed;
            }
        }
        else
        {
            velocity.y -= gravity * Time.deltaTime;
        }
        Vector3 move = (forward * moveZ + right * moveX) * speed;
        move.y = velocity.y;
        characterController.Move(move * Time.deltaTime);
    }

}
