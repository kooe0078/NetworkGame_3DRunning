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

    public Text currentRankText;
    public Text maxPlayerText;

    Vector3 finishLinePosition;

    void Start()
    {
        finishLine = GameObject.Find("FinishLine");
        finishLinePosition = finishLine.transform.position;
        pv = GetComponent<PhotonView>();

        currentRankText = GameObject.Find("currentRankingText").GetComponent<Text>();
        maxPlayerText = GameObject.Find("maxPlayerText").GetComponent<Text>();

        StartCoroutine(GetPlayers());
    }

    IEnumerator GetPlayers()
    {
        while(true){
            players = GameObject.FindGameObjectsWithTag("Player");
            yield return new WaitForSeconds(3.0f);
        }
    }

    void Update()
    {
        StartCoroutine(currentRank());
        //pv.RPC("rankCalculRPC", RpcTarget.Others);
    }

    IEnumerator currentRank()    
    {
        if (pv.IsMine)
        {
            yield return new WaitForSeconds(0.1f);

            // ���� �������� ��� �÷��̾��� ��
            int maxPlayer = PhotonNetwork.CurrentRoom.PlayerCount;

            // �ڱ� �ڽŰ� ������� �Ÿ� ���
            var dist = (finishLinePosition - this.transform.position).sqrMagnitude;
            Debug.DrawLine(this.transform.position, finishLinePosition, Color.red);

            int numberOfBackPlayer = 0;
            for (int currentPlayer = 0; currentPlayer < players.Length; currentPlayer++)
            {
                //if (players[currentPlayer].GetComponent<PlayerCtrl>().photonView.IsMine)
                //    continue;
                // �ٸ� �÷��̾�� ������� �Ÿ� ���
                var otherPlayerDist = (finishLinePosition - players[currentPlayer].transform.position).sqrMagnitude;
                // �ڱ⺸�� �ڿ� �ִ� �÷��̾ ������, �� ��� �� ���� ���� ������Ŵ
                if (dist < otherPlayerDist)
                {
                    numberOfBackPlayer++;
                }
            }
            //Debug.Log("���� ���: " + (maxPlayer - numberOfBackPlayer));

            // ��� �ؽ�Ʈ ���
            if (maxPlayerText != null)
                maxPlayerText.text = maxPlayer.ToString();
            if (currentRankText != null)
                currentRankText.text = (maxPlayer - numberOfBackPlayer).ToString();
        }
    }
}
