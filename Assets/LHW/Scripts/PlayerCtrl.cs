using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

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
    // 플레이어의 이동을 제한하는 bool 변수
    private bool isMoveAble = true;
    // 포톤 뷰 선언 및 설정
    private PhotonView pv;
    // 플레이어의 이름을 받아오는 변수
    public TextMesh playerName;
    public string name = "";
    // 플레이어의 위치 및 회전값 동기화를 위한 변수
    public Vector3 currPos;
    private Quaternion currRot;
    // 아이템 획득을 위한 난수 설정
    float randomItemNum;
    useItem useItem;

    private InGameManager ingameManager;
    public bool bAttack = false;

    private Image itemImage;

    void Start()
    {
        ingameManager = GameObject.Find("InGameManager").GetComponent<InGameManager>();
        characterRigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        pv = GetComponent<PhotonView>();
        useItem = GetComponent<useItem>();
        itemImage = GameObject.Find("Canvas").transform.Find("itemPanel").transform.Find("itemImage").GetComponent<Image>();

        pv.ObservedComponents[0] = this;
        cameraTransform = GameObject.Find("Main Camera").transform;

        if (pv.IsMine)
        {
            ingameManager.GetPlayerName(name);
            GameObject.Find("CameraBase").GetComponent<CameraCtrl>().CameraFollowObj
                = transform.Find("CamFollow").gameObject.transform;
        }

        PhotonNetwork.NickName = name;
    }

    void Update()
    {      
        if (ingameManager && ingameManager.bGameStart)
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
        }

        // 아이템 획득을 위한 난수 생성
        randomItemNum = Random.Range(0, 10);
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
                pv.RPC("JumpRPC", RpcTarget.All);
            }
        }        
    }

    [PunRPC]
    private void JumpRPC()
    {
        bJumping = true;
        animator.SetBool("bJumping", true);
        characterRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
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
            if (!bAttack)
            {
                Debug.Log(bAttack);
                // 미사일에 맞으면 2초간 이동 불가
                characterRigidbody.AddForce(Vector3.up * 10f, ForceMode.Impulse);
                animator.SetBool("IsHit", true);
                isMoveAble = false;
                Invoke("moveAble", 2.0f);
            }
        }

        if (collision.gameObject.CompareTag("ItemBox"))
        {
            if (pv.IsMine)
                playerGetItem();
        }
    }

    public void moveAble()
    {
        // 이동 불가 해제
        isMoveAble = true;
        animator.SetBool("IsHit", false);
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

    void playerGetItem()
    {
        Sprite MissileSprite = Resources.Load("itemImage/sprite_198", typeof(Sprite)) as Sprite;
        Sprite BoosterSprite = Resources.Load("itemImage/sprite_325", typeof(Sprite)) as Sprite;
        Sprite ShieldSprite = Resources.Load("itemImage/sprite_379", typeof(Sprite)) as Sprite;

        // 플레이어가 아이템을 보유하지 않았을 때만 아이템을 얻게 함
        if (!useItem.isPlayerGetItem)
        {
            switch (randomItemNum)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                    useItem.getMissile = true;
                    itemImage.sprite = MissileSprite;
                    Debug.Log("미사일 획득");
                    break;                    
                case 4:
                case 5:
                case 6:
                    useItem.getBooster = true;
                    itemImage.sprite = BoosterSprite;
                    Debug.Log("부스터 획득");
                    break;
                case 7:
                case 8:
                case 9:
                    useItem.getShield = true;
                    itemImage.sprite = ShieldSprite;
                    Debug.Log("쉴드 획득");
                    break;
                default:
                    break;
            }
            // 플레이어가 아이템 보유중
            useItem.isPlayerGetItem = true;
        }
        else
            Debug.Log("플레이어는 아이템을 가지고 있으니 추가 아이템을 얻지 않습니다");
    }
}
