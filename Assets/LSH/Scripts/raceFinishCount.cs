using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class raceFinishCount : MonoBehaviour
{
    private PhotonView pv;
    public GameObject countText;
    List<string> playerList;
    bool firstPlayer = true;
    private Animator animator;
    public GameObject leaderBoard;
    public GameObject rowPrefab;
    public Transform rowsParent;

    void Start()
    {
        pv = GetComponent<PhotonView>();
        playerList = new List<string>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            // �浹�� �÷��̾��� �̸��� ������ List�� �ִ� �̸����� ��
            var getName = other.gameObject.GetComponent<PlayerCtrl>().name;
            var search = playerList.Find(list => list == getName);

            // ����Ʈ�� ���� �̸��̸�, ����Ʈ�� �߰�
            if (search != getName)
                playerList.Add(other.gameObject.GetComponent<PlayerCtrl>().name);

            // 1�� �÷��̾ ������ ī��Ʈ�ٿ��� �����ϰ�, ���Ŀ� ������� ����
            if (firstPlayer)
            {
                firstPlayer = false;
                FinishCountStart();
            }
        }
    }

    void FinishCountStart()
    {
        StartCoroutine(FinishCountdown());
        pv.RPC("GameFinishRPC", RpcTarget.Others);
    }

    [PunRPC]
    public void GameFinishRPC()
    {
        StartCoroutine(FinishCountdown());
    }

    IEnumerator FinishCountdown()
    {
        int timer = 10;
        while (timer != -1)
        {
            GameObject cntObj = Instantiate(countText);
            cntObj.transform.SetParent(GameObject.Find("Canvas").transform, false);
            animator = cntObj.GetComponent<Animator>();
            if (timer > 0)
            {
                cntObj.GetComponent<Text>().text = timer.ToString();
                animator.SetBool("TimerEnd", false);
            }
               
            else
            {
                cntObj.GetComponent<Text>().text = "Time Up!";
                animator.SetBool("TimerEnd", true);
            }
               
            timer--;
            yield return new WaitForSeconds(1.0f);
            Destroy(cntObj);
        }

        LeaderBoard();
    }

    void LeaderBoard()
    {
        leaderBoard.SetActive(true);

        for (int rank = 0; rank < playerList.Count; rank++) 
        {
            GameObject newGO = Instantiate(rowPrefab, rowsParent);
            Text[] texts = newGO.GetComponentsInChildren<Text>();
            texts[0].text = (rank + 1).ToString();
            texts[1].text = playerList[rank];
        }
    }
}
