using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class useItem : MonoBehaviour
{
    public GameObject Missile;
    public GameObject Shield;
    private Vector3 missileSpawnPos = new Vector3(0, 20, 0);
    public GameObject boosterEffect;

    public bool isPlayerGetItem = false;
    public bool getMissile = false;
    public bool getShield = false;
    public bool getBooster = false;

    // 포톤 뷰 선언 및 설정
    private PhotonView pv;
    private PlayerCtrl playerCtrl;
    private Image itemImage;

    private void Start()
    {
        playerCtrl = gameObject.GetComponent<PlayerCtrl>();
        pv = GetComponent<PhotonView>();
        itemImage = GameObject.Find("itemImage").GetComponent<Image>();
    }

    void Update()
    {
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
        playerCtrl.bAttack = true;
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
        yield return new WaitForSeconds(0.5f);
        // missileSpawnPos = new Vector3(missileSpawnPos.position.x, missileSpawnPos.position.y, missileSpawnPos.position.z - 10);
        GameObject missile = Instantiate(Missile, missileSpawnPos, Quaternion.identity);
        missile.GetComponent<missileCtrl>().bAttack = playerCtrl.bAttack;
        yield return new WaitForSeconds(1.5f);
        playerCtrl.bAttack = false;
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
        Sprite sprite = Resources.Load("itemImage/Icon_X", typeof(Sprite)) as Sprite; 
        itemImage.sprite = sprite;

        getMissile = false;
        getShield = false;
        getBooster = false;
        isPlayerGetItem = false;
    }
}
