using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class useItem : MonoBehaviour
{
    public GameObject Missile;
    public GameObject Shield;
    public Transform MissileSpawnPos;

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
        Debug.Log("ShieldOn");
        Shield.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        Shield.SetActive(false);
        Debug.Log("ShieldOff");
    }

    IEnumerator useBooster()
    {
        // �ν��� ��� �� 3�� �� �̵��ӵ� ����
        PlayerCtrl playerCtrl = GameObject.Find("Player").GetComponent<PlayerCtrl>();
        Debug.Log("BoosterOn");
        playerCtrl.maxSpeed *= 5.0f;
        yield return new WaitForSeconds(3.0f);
        playerCtrl.maxSpeed /= 5.0f;
        Debug.Log("BoosterOff");
    }

    void playerItemReset()
    {
        getMissile = false;
        getShield = false;
        getBooster = false;
        isPlayerGetItem = false;
    }
}
