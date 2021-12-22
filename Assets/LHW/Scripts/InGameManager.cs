using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class InGameManager : MonoBehaviour
{
    private PhotonView pv;
    public GameObject gameStartButton;
    public GameObject gameReadyButton;
    public GameObject lessReadyPanel;
    public GameObject rankingPanel;
    public GameObject itemPanel;
    public Text ReadyCnt;
    public GameObject countText;
    public Vector3 startPos;
    public bool bGameStart = true;
    public bool bCount = false;
    private bool bReady = false;
    public int gameReadyCnt = 0;
    private Animator animator;
    public ScrollRect scroll_rect = null;
    public string chatMessage;
    public InputField PlayerChatting;
    public Text chatText;
    public Button ChattingButton; 
    public string playerName = "";
    // Start is called before the first frame update
    void Start()
    {      
        bGameStart = true;      
        float randomX = Random.Range(45, 55);
        startPos = new Vector3(randomX, 1, 5);
        pv = GetComponent<PhotonView>();
        DrawReadyCnt();
        pv.RPC("PunDrawReadyCnt", RpcTarget.Others);
        if (PhotonNetwork.IsMasterClient)
        {
            gameStartButton.SetActive(true);
        }
        else
        {
            gameReadyButton.SetActive(true);
        }
    }
    public void GameStartButtonClicked()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount - 1 == gameReadyCnt && PhotonNetwork.CurrentRoom.PlayerCount!=1)
        {
            CharTeleport();
            CountDownStart();
            pv.RPC("GameStartButtonClickedRPC", RpcTarget.Others);
        }
        else
        {
            lessReadyPanel.SetActive(true);
        }
    }
    [PunRPC]
    void GameStartButtonClickedRPC()
    {        
        CharTeleport();
        CountDownStart();
    }

    void CharTeleport()
    {
        rankingPanel.SetActive(true);
        itemPanel.SetActive(true);
        gameStartButton.SetActive(false);
        gameReadyButton.SetActive(false);
        GameObject[] player;
        player = GameObject.FindGameObjectsWithTag("Player");

        foreach(var x in player)
        {
            x.transform.position = startPos;
            x.transform.rotation = Quaternion.identity;
        }
    }
    public void GameReadyButtonClicked()
    {
        PlusReadyCnt();
        pv.RPC("PunPlusReadyCnt", RpcTarget.Others);
    }
    [PunRPC]
    public void PunPlusReadyCnt()
    {
        PlusReadyCnt();
    }
    public void PlusReadyCnt()
    {
        if(!bReady)
        {
            gameReadyCnt++;
            bReady = true;
        }
        else
        {
            gameReadyCnt--;
            bReady = false;
        }
        DrawReadyCnt();
    }
    [PunRPC]
    void PunDrawReadyCnt()
    {
        DrawReadyCnt();
    }
    void DrawReadyCnt()
    {
        ReadyCnt.text = "Ready: " + gameReadyCnt.ToString() + " / " + (PhotonNetwork.CurrentRoom.PlayerCount - 1).ToString();
    }

    void CountDownStart()
    {
        StartCoroutine(CountDown());
    }

    public void GetPlayerName(string name)
    {
        playerName = name;
    }
    IEnumerator CountDown()
    {           
        //커서 비활성화
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        ReadyCnt.gameObject.SetActive(false);
        ChattingButton.gameObject.SetActive(false);
        PlayerChatting.gameObject.SetActive(false);
        scroll_rect.gameObject.SetActive(false);
        int cnt = 3;
        bGameStart = false;
        yield return new WaitForSeconds(3.0f);
       
        while (cnt != -1)
        {
            GameObject cntObj = Instantiate(countText);
            cntObj.transform.SetParent(GameObject.Find("Canvas").transform, false);
            animator = cntObj.GetComponent<Animator>();

            if (cnt > 0)
            {
                animator.SetBool("bCount", true);
                cntObj.GetComponent<Text>().text = cnt.ToString();
            }               
            else
            {
                bGameStart = true;
                bCount = true;
                animator.SetBool("bCountEnd", true);
                cntObj.GetComponent<Text>().text = "Go!";
                yield return new WaitForSeconds(0.5f);
            }             
            cnt--;
            yield return new WaitForSeconds(1.0f);
            Destroy(cntObj);
        }      
        //countText.gameObject.SetActive(false);
    }
    public void ClickChattingButton()
    {
        chatMessage = PlayerChatting.text;
        PlayerChatting.text = string.Empty;
        ShowChat(chatMessage, playerName);
        pv.RPC("ChatInfo", RpcTarget.Others, chatMessage, playerName);
    }

    [PunRPC]
    public void ChatInfo(string sChat, string name)
    {
        ShowChat(sChat, name);
    }

    public void ShowChat(string chat, string name)
    {
        if (playerName != "")
            chatText.text += name + ": " + chat + "\n";
        scroll_rect.verticalNormalizedPosition = 0.0f;
    }
}
