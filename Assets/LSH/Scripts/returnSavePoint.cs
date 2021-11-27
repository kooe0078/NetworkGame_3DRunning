using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class returnSavePoint : MonoBehaviour
{
    public Vector3 lastestCheckPoint;

    void Start()
    {
        // üũ����Ʈ�� ���� ��� �ʱ� ��ġ ����
        lastestCheckPoint = new Vector3(50, 1, 5);
    }

    private void OnCollisionEnter(Collision collision)
    {        
        // �÷��̾ �� ������ �������� �� ������ üũ����Ʈ�� �ڷ���Ʈ
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.position = lastestCheckPoint;
        }
        
    }
}
