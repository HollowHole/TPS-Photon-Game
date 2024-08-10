using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;
using System.Linq;
public class Launcher : MonoBehaviourPunCallbacks
{
    public static Launcher instance;
    [SerializeField] InputField roomNameInputField;
    [SerializeField] Text roomNameText;
    [SerializeField] Text errorText;

    [SerializeField] Transform roomListContent;
    [SerializeField] GameObject roomListItemPrefab;

    [SerializeField] Transform PlayerListContent;
    [SerializeField] GameObject PlayerListItemPrefab;
    public GameObject StartGameButtom;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        Debug.Log("Connecting to Master...");
        MenuManager.instance.OpenMenu("LoadingMenu");
        PhotonNetwork.ConnectUsingSettings();
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connecting to Lobby...");
        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public override void OnJoinedLobby()
    {
        MenuManager.instance.OpenMenu("TitleMenu");
        PhotonNetwork.NickName = "Player" + Random.Range(0,10000).ToString("0000");
    }
    public void CreateRoom()
    {
        if(string.IsNullOrEmpty(roomNameInputField.text)) return;
        PhotonNetwork.CreateRoom(roomNameInputField.text);
        MenuManager.instance.OpenMenu("LoadingMenu");
    }
    public override void OnJoinedRoom()
    {
        MenuManager.instance.OpenMenu("RoomMenu");
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;

        Player[] players = PhotonNetwork.PlayerList;
        foreach (Transform child in PlayerListContent)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < players.Length; i++)
        {
            Instantiate(PlayerListItemPrefab, PlayerListContent).GetComponent<PlayerListItem>().SetUp(players[i]);
        }
        StartGameButtom.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        StartGameButtom.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnJoinRoomFailed(short returnCode, string errorMessage)
    {
        errorText.text = "Fail to generate room!" + errorMessage;
        MenuManager.instance.OpenMenu("ErrorMenu");
    }
    public void JoinRoom(RoomInfo roomInfo)
    {
        PhotonNetwork.JoinRoom(roomInfo.Name);
        MenuManager.instance.OpenMenu("LoadingMenu");
    }

    public void StartGame()
    {
        PhotonNetwork.LoadLevel(1);   
    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        MenuManager.instance.OpenMenu("LoadingMenu0");
    }
    public override void OnLeftRoom()
    {
        MenuManager.instance.OpenMenu("TitleMenu");
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("Destroy rooms...");
        foreach(Transform trans in roomListContent)
        {
            Destroy(trans.gameObject);
        }
        
        for (int i = 0; i < roomList.Count; i++)
        {
            
            if (roomList[i].RemovedFromList) continue;
            Debug.Log("Instanciating..."+ roomList[i].Name);
            Instantiate(roomListItemPrefab, roomListContent).GetComponent<RoomListItem>().SetUp(roomList[i]);
        }
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Instantiate(PlayerListItemPrefab,PlayerListContent).GetComponent<PlayerListItem>().SetUp(newPlayer);
    }
}
