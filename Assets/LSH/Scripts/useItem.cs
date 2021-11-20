using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class useItem : MonoBehaviour
{
    public GameObject Missile;
    public GameObject Shield;
    public Transform MissileSpawnPos;

    void Update()
    {
        // ���콺 ��Ŭ������ �̻��� �߻�
        if (Input.GetButtonDown("Fire1"))
        {
            Instantiate(Missile, MissileSpawnPos.position, MissileSpawnPos.rotation);
        }
        // ���콺 ��Ŭ������ ���� ����
        if (Input.GetButtonDown("Fire2"))
        {
            StartCoroutine(ShieldActive());
        }
        // ���콺 �� Ŭ���� �ν���
        if (Input.GetButtonDown("Fire3"))
        {
            StartCoroutine(Booster());
        }
    }

    IEnumerator ShieldActive()
    {
        // ���� ���� �� 2�ʵ� ����
        Debug.Log("ShieldOn");
        Shield.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        Shield.SetActive(false);
        Debug.Log("ShieldOff");
    }

    IEnumerator Booster()
    {
        // �ν��� ��� �� 3�ʵ� ����
        PlayerCtrl playerCtrl = GameObject.Find("Player").GetComponent<PlayerCtrl>();
        Debug.Log("BoosterOn");
        playerCtrl.maxSpeed *= 5.0f;
        yield return new WaitForSeconds(3.0f);
        playerCtrl.maxSpeed /= 5.0f;
        Debug.Log("BoosterOff");
    }
}
