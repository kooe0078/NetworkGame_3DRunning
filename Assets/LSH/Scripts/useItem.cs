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

    // ���� �� ���� �� ����
    private PhotonView pv;

    private PlayerCtrl playerCtrl;
    private void Start()
    {
        playerCtrl = gameObject.GetComponent<PlayerCtrl>();
        pv = GetComponent<PhotonView>();
    }

    void Update()
    {
        // if���� �������� pv.IsMine�� �ɸ� ȣ��Ʈ �÷��̾ �������� ����� �� �ִ� ���� �߻�
        // ������ ���� ������ � Ŭ�󿡼� �������� ����ص� ȣ��Ʈ�� ĳ���Ϳ��� �������� ����
        // �� �̷����� �𸣰ڴ� ����
        // 3�� 11�� �߰�
        // ���� ���� ���� ������ �𸣰ٴµ� �ذ��, �ٵ� 1�� ����Ƽ���� ���徲�� 2�� ����Ƽ���� �ν��ͷ� ����
        // 2�� ����Ƽ���� ���徲�� 1������ �ν��ͷ� ���� �������������������� ��ġ�ڴ� ��¥
        // �ϴ� �̰� ���� �ٽ� ����Ƽ���� �����ؼ� �غ��ǵ� �ȵǸ� ����ΰ� �ݶ��ϰ� ���������� �������
        // 3�� 37��
        // �� ���������� ������������������������������������������������������������������������
        // �� �� ���� ũ�� �ǵ�� ���� ��������? ����?????????????
        // ���ڳ� ��¥ ������������������������ 
        // ���� �ڵ�� ���� �̻��� ��� ������ ������� ���ư��µ� ���� ȭ�鿡 ���� ����, �̰� ���� ��ĥ���� �ڷ��� ����
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
        GameObject missile = Instantiate(Missile, MissileSpawnPos.position, MissileSpawnPos.rotation);
        yield return null;
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
        getMissile = false;
        getShield = false;
        getBooster = false;
        isPlayerGetItem = false;
    }
}
