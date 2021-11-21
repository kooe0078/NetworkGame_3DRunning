using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    public float maxSpeed = 7f;
    public float jumpForce = 7f;

    private Rigidbody characterRigidbody;
    public Transform cameraTransform = null;
    public float turnVelocity;
    Vector3 moveDir = Vector3.zero;
    private Animator animator;

    private bool bJumping = false;

    // 플레이어가 미사일에 맞을 경우 이동을 제한하는 bool 변수
    private bool isMoveAble = true;
    void Start()
    {
        characterRigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputZ = Input.GetAxis("Vertical");
        float MouseX = Input.GetAxis("Mouse X");
        // -1 ~ 1
        animator.SetFloat("Vertical", inputZ);
        animator.SetFloat("Horizontal", inputX);
        if (isMoveAble)
        {
            MoveTo(new Vector3(inputX, 0, inputZ));

            transform.Translate(moveDir * maxSpeed * Time.deltaTime, Space.World);

            RotateTo();
            Jump();
        }
    }
    private void MoveTo(Vector3 direction)
    {
        Vector3 moveis = cameraTransform.rotation * direction;

        moveDir = new Vector3(moveis.x, moveDir.y, moveis.z);
    }
    private void RotateTo()
    {
        float playerAngle = Mathf.Atan2(moveDir.x, moveDir.z) * Mathf.Rad2Deg;
        if (moveDir.x != 0 || moveDir.z != 0)
            transform.rotation = Quaternion.Euler(0f, playerAngle, 0f);
    }
    private void Jump()
    {
        if(!bJumping)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                bJumping = true;
                animator.SetBool("bJumping", true);
                characterRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            bJumping = false;
            animator.SetBool("bJumping", false);
        }

        if (collision.gameObject.CompareTag("Missile"))
        {
            // 미사일에 맞으면 2초간 이동 불가
            characterRigidbody.AddForce(Vector3.up * 10f, ForceMode.Impulse);
            isMoveAble = false;
            Invoke("moveAble", 2.0f);
        }
    }

    public void moveAble()
    {
        // 이동 불가 해제
        isMoveAble = true;
    }
}
