using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    void Update()
    {
        if (isPlayerGetItem)
        {
            // 마우스 좌클릭으로 미사일 발사
            if (getMissile && Input.GetButtonDown("Fire1"))
            {
                Instantiate(Missile, MissileSpawnPos.position, MissileSpawnPos.rotation);
                playerItemReset();
            }
            // 마우스 우클릭으로 쉴드 생성
            if (getShield && Input.GetButtonDown("Fire2"))
            {
                StartCoroutine(useShield());
                playerItemReset();
            }
            // 마우스 휠 클릭시 부스터
            if (getBooster && Input.GetButtonDown("Fire3"))
            {
                StartCoroutine(useBooster());
                playerItemReset();
            }
        }
    }

    IEnumerator useShield()
    {
        // 쉴드 생성 후 2초 뒤 제거
        Shield.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        Shield.SetActive(false);
    }

    IEnumerator useBooster()
    {
        // 부스터 사용 후 3초 뒤 이동속도 복구
        PlayerCtrl playerCtrl = GameObject.Find("Player").GetComponent<PlayerCtrl>();
        playerCtrl.maxSpeed *= 2.0f;
        //부스터 사용 이펙트 생성
        var boosterInstance = Instantiate(boosterEffect, transform.position, transform.rotation);
        var boosterParticle = boosterInstance.GetComponent<ParticleSystem>();
        Destroy(boosterInstance, boosterParticle.main.duration);

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
