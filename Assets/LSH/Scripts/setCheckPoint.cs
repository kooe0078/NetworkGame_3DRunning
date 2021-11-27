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
        // ���������� �����ߴ� üũ����Ʈ�� ��ġ ����
        if (other.gameObject.CompareTag("Player"))
        {
            rSP.lastestCheckPoint = transform.position;
        }
    }
}
