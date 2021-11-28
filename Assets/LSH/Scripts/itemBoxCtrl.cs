using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemBoxCtrl : MonoBehaviour
{
    float randomItemNum;
    public GameObject getEffect;

    void Update()
    {
        // ������ ȹ���� ���� ���� ����
        randomItemNum = Random.Range(0, 10);
    }

    private void OnCollisionEnter(Collision coll)
    {
        // �÷��̾�� �浹�� ���
        if (coll.transform.CompareTag("Player"))
        {
            // ���� ���� ����Ʈ ����
            var getInstance = Instantiate(getEffect, transform.position, transform.rotation);
            var getParticle = getInstance.GetComponent<ParticleSystem>();
            Destroy(getInstance, getParticle.main.duration);
            //���� ��Ȱ��ȭ �� 5�� �� �����
            gameObject.SetActive(false);

            Invoke("boxReset", 5.0f);
            useItem useItem = GameObject.FindWithTag("Player").GetComponent<useItem>();
            // �÷��̾ �������� �������� �ʾ��� ���� �������� ��� ��
            if (!useItem.isPlayerGetItem)
            {
                switch (randomItemNum)
                {
                    case 0:
                    case 1:
                    case 2:
                        useItem.getBooster = true;
                        Debug.Log("�ν��� ȹ��");
                        break;
                    case 3:
                    case 4:
                    case 5:
                    case 6:
                        useItem.getMissile = true;
                        Debug.Log("�̻��� ȹ��");
                        break;
                    case 7:
                    case 8:
                    case 9:
                        useItem.getShield = true;
                        Debug.Log("���� ȹ��");
                        break;
                    default:
                        break;
                }

                useItem.isPlayerGetItem = true;
            }
            else
                Debug.Log("�÷��̾�� �������� ������ ������ �߰� �������� ���� �ʽ��ϴ�");
        }
    }

    void boxReset()
    {
        // ������ �ڽ� �����
        gameObject.SetActive(true);
    }
}
