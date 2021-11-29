using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemBoxCtrl : MonoBehaviour
{
    public GameObject getEffect;

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
        }
    }    

    void boxReset()
    {
        // 아이템 박스 재생성
        gameObject.SetActive(true);
    }
}
