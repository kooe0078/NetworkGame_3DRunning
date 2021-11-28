using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class returnSavePoint : MonoBehaviour
{
    public Vector3 lastestCheckPoint;

    void Start()
    {
        // 체크포인트가 없을 경우 초기 위치 설정
        lastestCheckPoint = new Vector3(0, 0, 0);
    }

    private void OnCollisionEnter(Collision collision)
    {        
        // 플레이어가 맵 밖으로 떨어졌을 때 마지막 체크포인트로 텔레포트
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.position = lastestCheckPoint;
        }
        
    }
}
