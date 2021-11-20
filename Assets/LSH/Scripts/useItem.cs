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
        // 마우스 좌클릭으로 미사일 발사
        if (Input.GetButtonDown("Fire1"))
        {
            Instantiate(Missile, MissileSpawnPos.position, MissileSpawnPos.rotation);
        }
        // 마우스 우클릭으로 쉴드 생성
        if (Input.GetButtonDown("Fire2"))
        {
            StartCoroutine(ShieldActive());
        }
        // 마우스 휠 클릭시 부스터
        if (Input.GetButtonDown("Fire3"))
        {
            StartCoroutine(Booster());
        }
    }

    IEnumerator ShieldActive()
    {
        // 쉴드 생성 후 2초뒤 제거
        Debug.Log("ShieldOn");
        Shield.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        Shield.SetActive(false);
        Debug.Log("ShieldOff");
    }

    IEnumerator Booster()
    {
        // 부스터 사용 후 3초뒤 복귀
        PlayerCtrl playerCtrl = GameObject.Find("Player").GetComponent<PlayerCtrl>();
        Debug.Log("BoosterOn");
        playerCtrl.maxSpeed *= 5.0f;
        yield return new WaitForSeconds(3.0f);
        playerCtrl.maxSpeed /= 5.0f;
        Debug.Log("BoosterOff");
    }
}
