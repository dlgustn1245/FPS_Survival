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
    public bool isRun = false;

    float currentCameraRotationX = 0.0f;
    float moveSpeed;
    float applyCrouchPosY;
    float originPosY;
    
    bool isGround = true;
    bool isCrouch = false;
    bool isWalk = false;

    public Camera theCamera;

    Rigidbody rb;
    CapsuleCollider capsuleCollider;
    CrossHair crossHair;
    Vector3 lastPos;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        crossHair = FindObjectOfType<CrossHair>();
    }

    void Start()
    {
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
        if(Input.GetKeyDown(KeyCode.Escape)) UnityEditor.EditorApplication.isPlaying = false;
    }

    void FixedUpdate()
    {
        Move();
        MoveCheck();
    }

    void Move()
    {
        float moveDirX = Input.GetAxisRaw("Horizontal");
        float moveDirZ = Input.GetAxisRaw("Vertical");

        Vector3 moveHorizontal = transform.right * moveDirX;
        Vector3 moveVertical = transform.forward * moveDirZ;

        Vector3 velocity = (moveHorizontal + moveVertical).normalized * moveSpeed;

        rb.MovePosition(transform.position + velocity * Time.deltaTime);
    }

    void MoveCheck()
    {
        if (!isRun && !isCrouch && isGround)
        {
            if (Vector3.Distance(lastPos, this.transform.position) >= 0.01f) isWalk = true; //경사면에서 미끄러지는 경우가 있기때문에 여유를 둠
            else isWalk = false;
            crossHair.WalkingAnim(isWalk);
            lastPos = this.transform.position;
        }
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
        if (isCrouch) Crouch();
        isRun = true;
        crossHair.RunningAnim(isRun);
        moveSpeed = runSpeed;
    }

    void RunningCancel()
    {
        isRun = false;
        crossHair.RunningAnim(isRun);
        moveSpeed = walkSpeed;
    }

    void IsGround()
    {
        isGround = Physics.Raycast(transform.position, Vector3.down, capsuleCollider.bounds.extents.y + 0.1f);
        crossHair.RunningAnim(!isGround);
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
        rb.velocity = transform.up * jumpForce;
    }

    void TryCrouch()
    {
        if (Input.GetKeyUp(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.LeftControl))
        {
            Crouch();
        }
    }

    void Crouch()
    {
        isCrouch = !isCrouch;
        crossHair.CrouchingAnim(isCrouch);
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
        rb.MoveRotation(rb.rotation * Quaternion.Euler(characterRotationY));
    }
}
