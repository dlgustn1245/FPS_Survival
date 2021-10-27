using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed;
    public float runSpeed;
    public float crouchSpeed;
    public float lookSensitivity;
    public float cameraRotationLimit;
    public float jumpForce;
    public float crouchPosY;

    float currentCameraRotationX = 0.0f;
    float moveSpeed;
    float applyCrouchPosY;
    float originPosY;
    
    bool isRun = false;
    bool isGround = true;
    bool isCrouch = false;

    public Camera theCamera;

    Rigidbody rb2d;
    CapsuleCollider capsuleCollider;

    void Start()
    {
        rb2d = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        moveSpeed = walkSpeed;
        originPosY = theCamera.transform.localPosition.y;
        applyCrouchPosY = originPosY;
    }

    void Update()
    {
        IsGround();
        TryRun();
        TryJump();
        TryCrouch();
        CameraRotation();
        CharacterRotation();
    }

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        float moveDirX = Input.GetAxisRaw("Horizontal");
        float moveDirZ = Input.GetAxisRaw("Vertical");

        Vector3 moveHorizontal = transform.right * moveDirX;
        Vector3 moveVertical = transform.forward * moveDirZ;

        Vector3 velocity = (moveHorizontal + moveVertical).normalized * moveSpeed;

        rb2d.MovePosition(transform.position + velocity * Time.deltaTime);
    }

    void TryRun()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Running();
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            RunningCancel();
        }
    }

    void Running()
    {
        isRun = true;
        moveSpeed = runSpeed;
    }

    void RunningCancel()
    {
        if (isCrouch) Crouch();
        isRun = false;
        moveSpeed = walkSpeed;
    }

    void IsGround()
    {
        isGround = Physics.Raycast(transform.position, Vector3.down, capsuleCollider.bounds.extents.y + 0.1f);
    }
    
    void TryJump()
    {
        if(Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            Jump();
        }
    }

    void Jump()
    {
        if (isCrouch) Crouch(); //앉은 상태에서 점프 -> 앉은 상태 해제
        rb2d.velocity = transform.up * jumpForce;
    }

    void TryCrouch()
    {
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            Crouch();
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Crouch();
        }
    }

    void Crouch()
    {
        isCrouch = !isCrouch;
        if (isCrouch)
        {
            moveSpeed = crouchSpeed;
            applyCrouchPosY = crouchPosY;
        }
        else
        {
            moveSpeed = walkSpeed;
            applyCrouchPosY = originPosY;
        }

        StartCoroutine(CrouchCoroutine());
    }

    IEnumerator CrouchCoroutine()
    {
        float yPos = theCamera.transform.localPosition.y;
        int cnt = 0;

        while(yPos != applyCrouchPosY)
        {
            ++cnt;
            yPos = Mathf.Lerp(yPos, applyCrouchPosY, 0.05f);
            theCamera.transform.localPosition = new Vector3(0.0f, yPos, 0.0f);
            if (cnt > 15) break;
            yield return null;
        }
        theCamera.transform.localPosition = new Vector3(0.0f, applyCrouchPosY, 0.0f);
    }

    void CameraRotation()
    {
        float xRotation = Input.GetAxisRaw("Mouse Y");
        float cameraRotationX = xRotation * lookSensitivity;
        currentCameraRotationX -= cameraRotationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

        theCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0.0f, 0.0f);
    }

    void CharacterRotation()
    {
        float yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 characterRotationY = new Vector3(0.0f, yRotation, 0.0f) * lookSensitivity;
        rb2d.MoveRotation(rb2d.rotation * Quaternion.Euler(characterRotationY));
    }
}
