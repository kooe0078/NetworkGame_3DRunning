using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System.Linq;

public class raceRankingManager : MonoBehaviour
{
    private GameObject finishLine;
    private GameObject[] players;
    private PhotonView pv;

    private InGameManager inGameManager;
    private Text currentRankText;
    private Text maxPlayerText;

    bool setTrigger = true;

    Vector3 finishLinePosition;
    void Start()
    {
        finishLine = GameObject.Find("FinishLine");
        finishLinePosition = finishLine.transform.position;
        pv = GetComponent<PhotonView>();

        inGameManager = GameObject.Find("InGameManager").GetComponent<InGameManager>();
        currentRankText = GameObject.Find("Canvas").transform.Find("rankingPanel").transform.Find("currentRankingText").GetComponent<Text>();
        maxPlayerText = GameObject.Find("Canvas").transform.Find("rankingPanel").transform.Find("maxPlayerText").GetComponent<Text>();

        StartCoroutine(GetPlayers());
    }

    IEnumerator GetPlayers()
    {
        while(true){
            players = GameObject.FindGameObjectsWithTag("Player");
            yield return new WaitForSeconds(3.0f);
        }
    }

    public void Update()
    {
        StartCoroutine(currentRank());
        //pv.RPC("rankCalculRPC", RpcTarget.Others);
    }

    [PunRPC]
    IEnumerator currentRank()
    {
        if (pv.IsMine && inGameManager.bCount)
        {
            yield return new WaitForSeconds(0.1f);

            // 현재 참가중인 모든 플레이어의 수
            int maxPlayer = PhotonNetwork.CurrentRoom.PlayerCount;

            // 자기 자신과 결승점의 거리 계산
            var dist = (finishLinePosition - this.transform.position).sqrMagnitude;
            Debug.DrawLine(this.transform.position, finishLinePosition, Color.red);

            int numberOfBackPlayer = 0;
            for (int currentPlayer = 0; currentPlayer < players.Length; currentPlayer++)
            {
                // 다른 플레이어와 결승점의 거리 계산
                var otherPlayerDist = (finishLinePosition - players[currentPlayer].transform.position).sqrMagnitude;
                // 자기보다 뒤에 있는 플레이어가 있을때, 뒷 사람 수 변수 값을 증가시킴
                if (dist < otherPlayerDist)
                {
                    numberOfBackPlayer++;
                }                
            }

            if (setTrigger && (maxPlayer - numberOfBackPlayer) == 1)
            {
                passingPlayerRPC();
                pv.RPC("passingPlayerRPC", RpcTarget.Others);
                setTrigger = false;
            }
            if (!setTrigger && (maxPlayer - numberOfBackPlayer) != 1)
            {
                setTrigger = true;
            }
            //Debug.Log("현재 등수: " + (maxPlayer - numberOfBackPlayer));

            // 등수 텍스트 출력
            if (maxPlayerText != null)
                maxPlayerText.text = maxPlayer.ToString();
            if (currentRankText != null)
                currentRankText.text = (maxPlayer - numberOfBackPlayer).ToString();
        }
    }

    [PunRPC]
    public void passingPlayerRPC()
    {
        Debug.Log("추월하는 애니메이션 트리거");
    }
}
