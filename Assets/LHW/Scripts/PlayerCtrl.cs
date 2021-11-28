using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerCtrl : MonoBehaviourPun, IPunObservable
{
    public float maxSpeed = 7f;
    public float jumpForce = 7f;

    private Rigidbody characterRigidbody;
    private Transform cameraTransform = null;
    public float turnVelocity;
    Vector3 moveDir = Vector3.zero;
    private Animator animator;
    // 플레이어가 점프중인지 아닌지 확인하는 bool 변수
    private bool bJumping = false;
    // 플레이어가 미사일에 맞을 경우 이동을 제한하는 bool 변수
    private bool isMoveAble = true;
    // 포톤 뷰 선언 및 설정
    private PhotonView pv;
    // 플레이어의 이름을 받아오는 변수
    public TextMesh playerName;
    string name = "";
    // 플레이어의 위치 및 회전값 동기화를 위한 변수
    private Vector3 currPos;
    private Quaternion currRot;
    private CameraCtrl camCtrl;

    void Start()
    {
        characterRigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        pv = GetComponent<PhotonView>();

        pv.ObservedComponents[0] = this;
        cameraTransform = GameObject.Find("Main Camera").transform;
        camCtrl = GameObject.Find("CameraBase").GetComponent<CameraCtrl>();
        if(camCtrl && pv.IsMine)
            camCtrl.CameraFollowObj= GameObject.Find("CamFollow");
    }

    void Update()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputZ = Input.GetAxis("Vertical");
        float MouseX = Input.GetAxis("Mouse X");
        // -1 ~ 1
        if (pv.IsMine)
        {
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
        else if (!pv.IsMine)
        {
            if (transform.position != currPos)
            {
                animator.SetFloat("Vertical", inputZ);
                animator.SetFloat("Horizontal", inputX);
                MoveTo(new Vector3(inputX, 0, inputZ));
                transform.Translate(moveDir * maxSpeed * Time.deltaTime, Space.World);
            }
            else
            {
                animator.SetFloat("Vertical", 0.0f) ;
                animator.SetFloat("Horizontal", 0.0f);
            }

            if (transform.rotation != currRot)
            {
                RotateTo();
            }            
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
            characterRigidbody.AddForce(Vector3.up * 5f, ForceMode.Impulse);
            isMoveAble = false;
            Invoke("moveAble", 2.0f);
        }
    }

    public void moveAble()
    {
        // 이동 불가 해제
        isMoveAble = true;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            stream.SendNext(name);
        }
        else
        {
            currPos = (Vector3)stream.ReceiveNext();
            currRot = (Quaternion)stream.ReceiveNext();
            SetPlayerName((string)stream.ReceiveNext());
        }
    }

    public void SetPlayerName(string name)
    {
        this.name = name;
        GetComponent<PlayerCtrl>().playerName.text = this.name;
    }

    public void playerUseMissile()
    {
        useItem _useItem = GetComponent<useItem>();
        _useItem.useMssile();
        pv.RPC("useMissileRPC", RpcTarget.Others);
    }

    [PunRPC]
    void useMissileRPC()
    {
        useItem _useItem = GetComponent<useItem>();
        _useItem.useMssile();
    }
}
