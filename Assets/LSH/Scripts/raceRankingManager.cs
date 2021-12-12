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

            // ���� �������� ��� �÷��̾��� ��
            int maxPlayer = PhotonNetwork.CurrentRoom.PlayerCount;

            // �ڱ� �ڽŰ� ������� �Ÿ� ���
            var dist = (finishLinePosition - this.transform.position).sqrMagnitude;
            Debug.DrawLine(this.transform.position, finishLinePosition, Color.red);

            int numberOfBackPlayer = 0;
            for (int currentPlayer = 0; currentPlayer < players.Length; currentPlayer++)
            {
                // �ٸ� �÷��̾�� ������� �Ÿ� ���
                var otherPlayerDist = (finishLinePosition - players[currentPlayer].transform.position).sqrMagnitude;
                // �ڱ⺸�� �ڿ� �ִ� �÷��̾ ������, �� ��� �� ���� ���� ������Ŵ
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
            //Debug.Log("���� ���: " + (maxPlayer - numberOfBackPlayer));

            // ��� �ؽ�Ʈ ���
            if (maxPlayerText != null)
                maxPlayerText.text = maxPlayer.ToString();
            if (currentRankText != null)
                currentRankText.text = (maxPlayer - numberOfBackPlayer).ToString();
        }
    }

    [PunRPC]
    public void passingPlayerRPC()
    {
        Debug.Log("�߿��ϴ� �ִϸ��̼� Ʈ����");
    }
}
