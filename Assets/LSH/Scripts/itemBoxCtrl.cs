using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemBoxCtrl : MonoBehaviour
{
    float randomItemNum;

    void Update()
    {
        randomItemNum = Random.Range(0, 10);
    }

    private void OnCollisionEnter(Collision coll)
    {
        // �÷��̾�� �浹�� ���
        if (coll.transform.CompareTag("Player"))
        {
            //StartCoroutine(playerGetItem());
            Debug.Log("Box Off");
            gameObject.SetActive(false);
            Invoke("boxReset", 5.0f);
            Debug.Log("Hit Player Box");
            useItem useItem = GameObject.Find("Player").GetComponent<useItem>();

            // �÷��̾ �������� �������� ���� ���
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

    //IEnumerator playerGetItem()
    //{
    //    Debug.Log("Box Off");
    //    gameObject.SetActive(false);
    //    yield return new WaitForSeconds(5.0f);
    //    gameObject.SetActive(true);
    //    Debug.Log("Box On");
    //}

    void boxReset()
    {
        gameObject.SetActive(true);
        Debug.Log("Box On");
    }
}
