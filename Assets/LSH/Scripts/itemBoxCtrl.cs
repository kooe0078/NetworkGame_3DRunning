using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemBoxCtrl : MonoBehaviour
{
    float randomItemNum;
    public GameObject getEffect;

    void Update()
    {
        // 아이템 획득을 위한 난수 생성
        randomItemNum = Random.Range(0, 10);
    }

    private void OnCollisionEnter(Collision coll)
    {
        // 플레이어와 충돌할 경우
        if (coll.transform.CompareTag("Player"))
        {
            // 상자 제거 이펙트 생성
            var getInstance = Instantiate(getEffect, transform.position, transform.rotation);
            var getParticle = getInstance.GetComponent<ParticleSystem>();
            Destroy(getInstance, getParticle.main.duration);
            //상자 비활성화 후 5초 뒤 재생성
            gameObject.SetActive(false);

            Invoke("boxReset", 5.0f);
            useItem useItem = GameObject.FindWithTag("Player").GetComponent<useItem>();
            // 플레이어가 아이템을 보유하지 않았을 때만 아이템을 얻게 함
            if (!useItem.isPlayerGetItem)
            {
                switch (randomItemNum)
                {
                    case 0:
                    case 1:
                    case 2:
                        useItem.getBooster = true;
                        Debug.Log("부스터 획득");
                        break;
                    case 3:
                    case 4:
                    case 5:
                    case 6:
                        useItem.getMissile = true;
                        Debug.Log("미사일 획득");
                        break;
                    case 7:
                    case 8:
                    case 9:
                        useItem.getShield = true;
                        Debug.Log("쉴드 획득");
                        break;
                    default:
                        break;
                }

                useItem.isPlayerGetItem = true;
            }
            else
                Debug.Log("플레이어는 아이템을 가지고 있으니 추가 아이템을 얻지 않습니다");
        }
    }

    void boxReset()
    {
        // 아이템 박스 재생성
        gameObject.SetActive(true);
    }
}
