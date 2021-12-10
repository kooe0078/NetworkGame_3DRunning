using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PhotonInit : MonoBehaviourPunCallbacks
{
    // �̱��� ������ ���� �ؾ� ��
    public static PhotonInit instance;

    public InputField playerInput;
    public Button chattingBtn;

    bool isGameStart = false;
    bool isLoggIn = false;
    string playerName = "";
    string connectionState = "";
    public string chatMessage ;
    Text chatText;
    ScrollRect scroll_rect = null;
    PhotonView pv;

    Text connectionInfoText;

    [Header("LobbyCanvas")] public GameObject LobbyCanvas;
    public GameObject LobbyPanel;
    public GameObject MakeRoomPanel;
    public GameObject RoomPanel;
    public InputField RoomNumberInput;
    public InputField RoomInput;
    public InputField RoomPwInput;
    public Toggle PwToggle;
    public GameObject PwPanel;
    public GameObject PwErrorLog;
    public GameObject PwConfirmBtn;
    public GameObject PwPanelCloseBtn;
    public InputField PwCheckIF;
    public bool LockState = false;
    public string privateroom;
    public Button[] CellBtn;
    public Button PreviousBtn;
    public Button NextBtn;
    public Button CreateRoomBtn;
    public int hashtablecount;
    public GameObject FadeInOutPrefab;

    List<RoomInfo> myList = new List<RoomInfo>();
    int currentPage = 1, maxPage, multiple, roomnumber;
    private void Awake()
    {
        
        PhotonNetwork.GameVersion = "MyFps 1.0";
        PhotonNetwork.ConnectUsingSettings();

        // ���� 2���� ������ �ε��� �Ǳ⶧���� UI ó���� �غ���
        if(GameObject.Find("ChatText") != null)
            chatText = GameObject.Find("ChatText").GetComponent<Text>();

        if (GameObject.Find("Scroll View") != null)
            scroll_rect = GameObject.Find("Scroll View").GetComponent<ScrollRect>();

        if (GameObject.Find("ConnectionInfoText") != null)
            connectionInfoText = GameObject.Find("ConnectionInfoText").GetComponent<Text>();
        
        connectionState = "������ ������ ���� ��...";

        if(connectionInfoText)
            connectionInfoText.text = connectionState;

        DontDestroyOnLoad(gameObject);
    }

    public static PhotonInit Instance
    {
        get
        {
            if(!instance)
            {
                instance = FindObjectOfType(typeof(PhotonInit)) as PhotonInit;

                if (instance == null)
                    Debug.Log("no singleton obj");
            }

            return instance;
        }
    }

    private void OnApplicationQuit()
    {
        PlayerPrefs.SetInt("LogIn", 0);
    }

    public void Connect()
    {

        if (PhotonNetwork.IsConnected)
        {
            connectionState = "�뿡 ����...";
            if (connectionInfoText)
                connectionInfoText.text = connectionState;

            LobbyPanel.SetActive(false);
            RoomPanel.SetActive(true);

            PhotonNetwork.JoinLobby();
        }
        else
        {
            connectionState = "�������� : ������ ������ ������� ����\n���� ��õ���...";
            if (connectionInfoText)
                connectionInfoText.text = connectionState;
            PhotonNetwork.ConnectUsingSettings();
        }

    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        connectionState = "No Room";
        if (connectionInfoText)
            connectionInfoText.text = connectionState;
        Debug.Log("No Room");
    }

    

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        connectionState = "Finish make a room";
        if (connectionInfoText)
            connectionInfoText.text = connectionState;
        Debug.Log("Finish make a room");
        
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        Debug.Log("OnCreateRoomFailed:"+returnCode + "-"+message);
    }
   

    public override void OnJoinedRoom()
    {
        GameObject fadeInOutObj = Instantiate(FadeInOutPrefab);
        fadeInOutObj.transform.SetParent(GameObject.Find("Canvas").transform, false);
        fadeInOutObj.GetComponent<FadeInOut>().bFadeOut = true;
        fadeInOutObj.GetComponent<FadeInOut>().bFadeOutEndFadeIn = true;
        fadeInOutObj.GetComponent<FadeInOut>().FadeInOutChange();
        StartCoroutine(JoinRoom());
    }

    IEnumerator JoinRoom()
    {
        yield return new WaitForSeconds(2.0f);
        base.OnJoinedRoom();
        connectionState = "Joined Room";
        if (connectionInfoText)
            connectionInfoText.text = connectionState;
        Debug.Log("Joined Room");
        isLoggIn = true;
        PlayerPrefs.SetInt("LogIn", 1);

        PhotonNetwork.LoadLevel("Map_2");
    }
    private void Update()
    {
        if (PlayerPrefs.GetInt("LogIn") == 1)
            isLoggIn = true;

        if (isGameStart == false && SceneManager.GetActiveScene().name == "Map_2" && isLoggIn == true)
        {
            isGameStart = true;
            if (GameObject.Find("ChatText") != null)
                chatText = GameObject.Find("ChatText").GetComponent<Text>();

            if (GameObject.Find("Scroll View") != null)
                scroll_rect = GameObject.Find("Scroll View").GetComponent<ScrollRect>();

            // �÷��̾� ��ǲ �ʵ� ��ü
            if (GameObject.Find("InFutFieldChat") != null)
                playerInput = GameObject.Find("InFutFieldChat").GetComponent<InputField>();

            if (GameObject.Find("ChattingButton") != null)
            {
                chattingBtn = GameObject.Find("ChattingButton").GetComponent<Button>();
                chattingBtn.onClick.AddListener(SetPlayerName);
            }

            StartCoroutine(CreatePlayer());
        }
    }

    IEnumerator CreatePlayer()
    {
        while(!isGameStart)
        {
            yield return new WaitForSeconds(0.5f);
        }

        GameObject tempPlayer = PhotonNetwork.Instantiate("Player",
                                    new Vector3(0.5f, 1, -5),
                                    Quaternion.identity,
                                    0);
        
        tempPlayer.GetComponent<PlayerCtrl>().SetPlayerName(playerName);
        pv = GetComponent<PhotonView>();
        yield return null;
    }

    private void OnGUI()
    {
        GUILayout.Label(connectionState);
    }

    public void SetPlayerName()
    {
        Debug.Log(playerInput.text + "�� �Է� �ϼ̽��ϴ�!");

        if(isGameStart == false && isLoggIn == false)
        {
            playerName = playerInput.text;
            playerInput.text = string.Empty;
            Debug.Log("connect �õ�!" + isGameStart + ", " + isLoggIn);
            Connect();
            
        }
        else
        {
            chatMessage = playerInput.text;
            playerInput.text = string.Empty;
            pv.RPC("ChatInfo", RpcTarget.All, chatMessage);
        }
        
    }

    public void ShowChat(string chat)
    {
        chatText.text += chat + "\n";
        scroll_rect.verticalNormalizedPosition = 0.0f;
    }

    [PunRPC]
    public void ChatInfo(string sChat)
    {
        ShowChat(sChat);
    }

    #region �κ� ���� UI ���� �Լ���
    public void CreateRoomBtnOnClick()
    {
        MakeRoomPanel.SetActive(true);
    }

    public void OKBtnOnClick()
    {
        MakeRoomPanel.SetActive(false);
    }

    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(RoomInput.text == "" ? "Game" + Random.Range(0, 100) : RoomInput.text, new RoomOptions { MaxPlayers = 100 });
        LobbyPanel.SetActive(false);
    }

    public void Disconnect()
    {
        PhotonNetwork.Disconnect();
        RoomPanel.SetActive(false);
        LobbyPanel.SetActive(true);
        connectionState = "������ ������ ������..";

        if (connectionInfoText)
            connectionInfoText.text = connectionState;

        isGameStart = false;
        isLoggIn = false;
        PlayerPrefs.SetInt("LogIn", 0);
    }

    public void CreateNewRoom()
    {
        RoomOptions roomOptions = new RoomOptions();

        int roomMaxNumber = int.Parse(RoomNumberInput.text.ToString());
        roomOptions.MaxPlayers = System.Convert.ToByte(roomMaxNumber);
        roomOptions.CustomRoomProperties = new Hashtable()
        {
            { "password", RoomPwInput.text }
        };
        roomOptions.CustomRoomPropertiesForLobby = new string[] { "password" };

        if (PwToggle.isOn)
        {
            PhotonNetwork.CreateRoom(RoomInput.text == "" ? "Game" + Random.Range(0, 100) : RoomInput.text, roomOptions);
        }
        else
        {
            PhotonNetwork.CreateRoom(RoomInput.text == "" ? "Game" + Random.Range(0, 100) : RoomInput.text, new RoomOptions { MaxPlayers = 100 });
        }
        MakeRoomPanel.SetActive(false);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("OnRoomListUpdate: " + roomList.Count);
        int roomCount = roomList.Count;

        for (int i = 0; i < roomCount; i++)
        {
            if (!roomList[i].RemovedFromList)
            {
                if (!myList.Contains(roomList[i]))
                    myList.Add(roomList[i]);
                else
                    myList[myList.IndexOf(roomList[i])] = roomList[i];
            }
            else if (myList.IndexOf(roomList[i]) != -1)
                myList.RemoveAt(myList.IndexOf(roomList[i]));
        }

        MyListRenewal();
    }

    public void MyListClick(int num)
    {
        if (num == -2)
        {
            --currentPage;
            MyListRenewal();
        }
        else if (num == -1)
        {
            ++currentPage;
            MyListRenewal();
        }
        else if (myList[multiple + num].CustomProperties["password"] != null)
        {
            PwPanel.SetActive(true);
        }
        else
        {
            PhotonNetwork.JoinRoom(myList[multiple + num].Name);
            MyListRenewal();
        }
    }

    public void RoomPw(int num)
    {
        switch (num)
        {
            case 0:
                roomnumber = 0;
                break;
            case 1:
                roomnumber = 1;
                break;
            case 2:
                roomnumber = 2;
                break;
            case 3:
                roomnumber = 3;
                break;

            default:
                break;
        }
    }

    public void EnterRoomWithPW()
    {
        if ((string)myList[multiple + roomnumber].CustomProperties["password"] == PwCheckIF.text)
        {
            PhotonNetwork.JoinRoom(myList[multiple + roomnumber].Name);
            MyListRenewal();
            PwPanel.SetActive(false);
        }
        else
        {
            StartCoroutine(ShowPwWrongMsg());
        }
    }

    IEnumerator ShowPwWrongMsg()
    {
        if (!PwErrorLog.activeSelf)
        {
            PwErrorLog.SetActive(true);
            yield return new WaitForSeconds(3.0f);
            PwErrorLog.SetActive(false);
        }
    }

    void MyListRenewal()
    {
        // �ִ� ������
        maxPage = (myList.Count % CellBtn.Length == 0) ? myList.Count / CellBtn.Length : myList.Count / CellBtn.Length + 1;

        // ����, ���� ��ư
        PreviousBtn.interactable = (currentPage <= 1) ? false : true;
        NextBtn.interactable = (currentPage >= maxPage) ? false : true;

        // �������� �´� ����Ʈ ����
        multiple = (currentPage - 1) * CellBtn.Length;
        for (int i = 0; i < CellBtn.Length; i++)
        {
            CellBtn[i].interactable = (multiple + i < myList.Count) ? true : false;           
            CellBtn[i].transform.GetChild(0).GetComponent<Text>().text = (multiple + i < myList.Count) ? myList[multiple + i].Name : "";
            CellBtn[i].transform.GetChild(1).GetComponent<Text>().text = (multiple + i < myList.Count) ? myList[multiple + i].PlayerCount + "/" + myList[multiple + i].MaxPlayers : "";
            CellBtn[i].transform.GetChild(3).gameObject.SetActive((multiple + i < myList.Count) ? true : false);

            if((int)((multiple + i < myList.Count) ? myList[multiple + i].CustomProperties.Count : 0) != 0)
            {
                CellBtn[i].transform.GetChild(2).gameObject.SetActive((multiple + i < myList.Count) ? true : false);
            }
            
        }

    }

    #endregion
}
