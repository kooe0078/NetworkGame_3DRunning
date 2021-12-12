using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class raceFinishCount : MonoBehaviour
{
    private PhotonView pv;
    public Text countText;
    List<string> playerList;
    bool firstPlayer = true;

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
            // 충돌한 플레이어의 이름을 가져와 List에 있는 이름인지 비교
            var getName = other.gameObject.GetComponent<PlayerCtrl>().name;
            var search = playerList.Find(list => list == getName);

            // 리스트에 없는 이름이면, 리스트에 추가
            if (search != getName)
                playerList.Add(other.gameObject.GetComponent<PlayerCtrl>().name);

            // 1등 플레이어가 들어오면 카운트다운을 실행하고, 이후엔 실행되지 않음
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
        countText.gameObject.SetActive(true);
        while (timer != -1)
        {
            if (timer > 0)
                countText.text = timer.ToString();
            else
                countText.text = "Time Up!";
            timer--;
            yield return new WaitForSeconds(1.0f);
        }
        countText.gameObject.SetActive(false);

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
