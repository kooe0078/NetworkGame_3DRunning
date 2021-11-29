using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class useItem : MonoBehaviour
{
    public GameObject Missile;
    public GameObject Shield;
    public Transform MissileSpawnPos;
    public GameObject boosterEffect;

    public bool isPlayerGetItem = false;
    public bool getMissile = false;
    public bool getShield = false;
    public bool getBooster = false;

    // 포톤 뷰 선언 및 설정
    private PhotonView pv;

    private PlayerCtrl playerCtrl;
    private void Start()
    {
        playerCtrl = gameObject.GetComponent<PlayerCtrl>();
        pv = GetComponent<PhotonView>();
    }

    void Update()
    {
        // if문의 조건으로 pv.IsMine을 걸면 호스트 플레이어만 아이템을 사용할 수 있는 현상 발생
        // 조건을 걸지 않으면 어떤 클라에서 아이템을 사용해도 호스트의 캐릭터에만 아이템이 사용됨
        // 왜 이러는지 모르겠다 ㅅㅂ
        // 3시 11분 추가
        // 위에 적은 문제 왜인지 모르겟는데 해결됨, 근데 1번 유니티에서 쉴드쓰면 2번 유니티에서 부스터로 보임
        // 2번 유니티에서 쉴드쓰면 1번에서 부스터로 보임 ㅋㅋㅋㅋㅋㅋㅋㅋㅋㅋ 미치겠다 진짜
        // 일단 이거 적고 다시 유니티파일 복사해서 해볼건데 안되면 적어두고 콜랍하고 멀쩡해지면 지울거임
        // 3시 37분
        // 다 멀쩡해졌네 ㅋㅋㅋㅋㅋㅋㅋㅋㅋㅋㅋㅋㅋㅋㅋㅋㅋㅋㅋㅋㅋㅋㅋㅋㅋㅋㅋㅋㅋㅋㅋㅋㅋㅋㅋㅋ
        // 나 뭐 딱히 크게 건든거 없음 저때부터? 왜지?????????????
        // 돌겠네 진짜 ㅋㅋㅋㅋㅋㅋㅋㅋㅋㅋㅋㅋ 
        // 지금 코드로 대충 미사일 쏘면 무조건 상대한테 날아가는데 각자 화면에 따로 나옴, 이건 내일 고칠거임 자러감 ㅅㄱ
        if (isPlayerGetItem)
        {
            // 마우스 좌클릭으로 미사일 발사
            if (getMissile && Input.GetButtonDown("Fire1"))
            {
                useMssile();
                playerItemReset();
            }
            // 마우스 우클릭으로 쉴드 생성
            if (getShield && Input.GetButtonDown("Fire2"))
            {
                Debug.Log("쉴드 사용1");
                useShield();
                playerItemReset();
            }
            // 마우스 휠 클릭시 부스터
            if (getBooster && Input.GetButtonDown("Fire3"))
            {
                Debug.Log("부스터 사용1");
                useBooster();
                playerItemReset();
            }
        }
    }

    public void useMssile()
    {
        StartCoroutine(CreateMissile());
        // 미사일 사용 RPC 호출
        pv.RPC("useMissileRPC", RpcTarget.Others);
    }

    [PunRPC]
    void useMissileRPC()
    {        
        StartCoroutine(CreateMissile());
    }

    IEnumerator CreateMissile()
    {
        GameObject missile = Instantiate(Missile, MissileSpawnPos.position, MissileSpawnPos.rotation);
        yield return null;
    }

    void useShield()
    {
        StartCoroutine(CreateShield());
        // 쉴드 사용 RPC 호출
        pv.RPC("useShieldRPC", RpcTarget.Others);
    }

    [PunRPC]
    void useShieldRPC()
    {
        StartCoroutine(CreateShield());
    }

    IEnumerator CreateShield()
    {
        Debug.Log("쉴드 사용2");
        // 쉴드 생성 후 2초 뒤 제거
        Shield.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        Shield.SetActive(false);
    }

    void useBooster()
    {
        StartCoroutine(CreateBooster());
        // 부스터 사용 RPC 호출
        pv.RPC("useBoosterRPC", RpcTarget.Others);
    }

    [PunRPC]
    void useBoosterRPC()
    {
        StartCoroutine(CreateBooster());
    }

    IEnumerator CreateBooster()
    {
        Debug.Log("부스터 사용2");
        playerCtrl.maxSpeed *= 2.0f;
        //부스터 사용 이펙트 생성
        var boosterInstance = Instantiate(boosterEffect, transform.position, transform.rotation);
        var boosterParticle = boosterInstance.GetComponent<ParticleSystem>();
        Destroy(boosterInstance, boosterParticle.main.duration);

        // 부스터 사용 후 3초 뒤 이동속도 복구
        yield return new WaitForSeconds(3.0f);
        playerCtrl.maxSpeed /= 2.0f;
    }

    void playerItemReset()
    {
        getMissile = false;
        getShield = false;
        getBooster = false;
        isPlayerGetItem = false;
    }
}
