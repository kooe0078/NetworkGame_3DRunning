using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class setCheckPoint : MonoBehaviour
{
    returnSavePoint rSP;

    void Start()
    {
        rSP = GameObject.Find("UnderRespawner").GetComponent<returnSavePoint>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // 마지막으로 도착했던 체크포인트의 위치 저장
        if (other.gameObject.CompareTag("Player"))
        {
            rSP.lastestCheckPoint = transform.position;
        }
    }
}
