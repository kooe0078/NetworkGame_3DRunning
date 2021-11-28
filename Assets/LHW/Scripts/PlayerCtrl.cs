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
    // �÷��̾ ���������� �ƴ��� Ȯ���ϴ� bool ����
    private bool bJumping = false;
    // �÷��̾ �̻��Ͽ� ���� ��� �̵��� �����ϴ� bool ����
    private bool isMoveAble = true;
    // ���� �� ���� �� ����
    private PhotonView pv;
    // �÷��̾��� �̸��� �޾ƿ��� ����
    public TextMesh playerName;
    string name = "";
    // �÷��̾��� ��ġ �� ȸ���� ����ȭ�� ���� ����
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
            // �̻��Ͽ� ������ 2�ʰ� �̵� �Ұ�
            characterRigidbody.AddForce(Vector3.up * 5f, ForceMode.Impulse);
            isMoveAble = false;
            Invoke("moveAble", 2.0f);
        }
    }

    public void moveAble()
    {
        // �̵� �Ұ� ����
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
