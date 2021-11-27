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
            // ���콺 ��Ŭ������ �̻��� �߻�
            if (getMissile && Input.GetButtonDown("Fire1"))
            {
                Instantiate(Missile, MissileSpawnPos.position, MissileSpawnPos.rotation);
                playerItemReset();
            }
            // ���콺 ��Ŭ������ ���� ����
            if (getShield && Input.GetButtonDown("Fire2"))
            {
                StartCoroutine(useShield());
                playerItemReset();
            }
            // ���콺 �� Ŭ���� �ν���
            if (getBooster && Input.GetButtonDown("Fire3"))
            {
                StartCoroutine(useBooster());
                playerItemReset();
            }
        }
    }

    IEnumerator useShield()
    {
        // ���� ���� �� 2�� �� ����
        Shield.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        Shield.SetActive(false);
    }

    IEnumerator useBooster()
    {
        // �ν��� ��� �� 3�� �� �̵��ӵ� ����
        PlayerCtrl playerCtrl = GameObject.Find("Player").GetComponent<PlayerCtrl>();
        playerCtrl.maxSpeed *= 2.0f;
        //�ν��� ��� ����Ʈ ����
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
