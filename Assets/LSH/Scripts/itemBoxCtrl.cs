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
        // 플레이어와 충돌할 경우
        if (coll.transform.CompareTag("Player"))
        {
            //StartCoroutine(playerGetItem());
            Debug.Log("Box Off");
            gameObject.SetActive(false);
            Invoke("boxReset", 5.0f);
            Debug.Log("Hit Player Box");
            useItem useItem = GameObject.Find("Player").GetComponent<useItem>();

            // 플레이어가 아이템을 보유하지 않은 경우
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
