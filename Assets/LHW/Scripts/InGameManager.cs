using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class InGameManager : MonoBehaviour
{
    private PhotonView pv;
    public GameObject gameStartButton;
    public Text countText;
    public Vector3 startPos;
    public bool bGameStart = true;
    // Start is called before the first frame update
    void Start()
    {
        bGameStart = true;
        float randomX = Random.Range(45, 55);
        startPos = new Vector3(randomX, 1, 5);
        pv = GetComponent<PhotonView>();
        if (PhotonNetwork.IsMasterClient)
        {
            gameStartButton.SetActive(true);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void GameStartButtonClicked()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount < 2) return;
        CharTeleport();
        CountDownStart();
        pv.RPC("GameStartButtonClickedRPC", RpcTarget.Others);  
    }
    [PunRPC]
    void GameStartButtonClickedRPC()
    {
        CharTeleport();
        CountDownStart();
    }

    void CharTeleport()
    {
        gameStartButton.SetActive(false);
        GameObject[] player;
        player = GameObject.FindGameObjectsWithTag("Player");

        foreach(var x in player)
        {
            x.transform.position = startPos;
        }
    }
    void CountDownStart()
    {
        StartCoroutine(CountDown());
    }

    IEnumerator CountDown()
    {
        int cnt = 3;
        bGameStart = false;
        yield return new WaitForSeconds(3.0f);
        countText.gameObject.SetActive(true);
        while (cnt != -1) 
        {
            if (cnt > 0)
                countText.text = cnt.ToString();
            else
                countText.text = "Go!";
            cnt--;
            yield return new WaitForSeconds(1.0f);        
        }
        bGameStart = true;
        countText.gameObject.SetActive(false);
    }
}
