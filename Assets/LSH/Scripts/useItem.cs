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

    // ���� �� ���� �� ����
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
            // ���콺 ��Ŭ������ �̻��� �߻�
            if (getMissile && Input.GetButtonDown("Fire1"))
            {
                useMssile();
                playerItemReset();
            }
            // ���콺 ��Ŭ������ ���� ����
            if (getShield && Input.GetButtonDown("Fire2"))
            {
                Debug.Log("���� ���1");
                useShield();
                playerItemReset();
            }
            // ���콺 �� Ŭ���� �ν���
            if (getBooster && Input.GetButtonDown("Fire3"))
            {
                Debug.Log("�ν��� ���1");
                useBooster();
                playerItemReset();
            }
        }
    }

    public void useMssile()
    {
        playerCtrl.bAttack = true;
        StartCoroutine(CreateMissile());
        // �̻��� ��� RPC ȣ��
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
        // ���� ��� RPC ȣ��
        pv.RPC("useShieldRPC", RpcTarget.Others);
    }

    [PunRPC]
    void useShieldRPC()
    {
        StartCoroutine(CreateShield());
    }

    IEnumerator CreateShield()
    {
        Debug.Log("���� ���2");
        // ���� ���� �� 2�� �� ����
        Shield.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        Shield.SetActive(false);
    }

    void useBooster()
    {
        StartCoroutine(CreateBooster());
        // �ν��� ��� RPC ȣ��
        pv.RPC("useBoosterRPC", RpcTarget.Others);
    }

    [PunRPC]
    void useBoosterRPC()
    {
        StartCoroutine(CreateBooster());
    }

    IEnumerator CreateBooster()
    {
        Debug.Log("�ν��� ���2");
        playerCtrl.maxSpeed *= 2.0f;
        //�ν��� ��� ����Ʈ ����
        var boosterInstance = Instantiate(boosterEffect, transform.position, transform.rotation);
        var boosterParticle = boosterInstance.GetComponent<ParticleSystem>();
        Destroy(boosterInstance, boosterParticle.main.duration);

        // �ν��� ��� �� 3�� �� �̵��ӵ� ����
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
