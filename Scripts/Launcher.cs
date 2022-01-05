using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using System.Linq;

public class Launcher : MonoBehaviourPunCallbacks
{
    public static Launcher Instance;

    public AudioSource menuMusic;
    public GameObject MainMenu;
    public GameObject StartMenu;
    public GameObject CreatRoomMenu;
    public GameObject RoomMenu;
    public GameObject RoomListMenu;
    public GameObject CreateButton;
    public TMP_Dropdown GameType;
    public TMP_Dropdown TraitorsNum;
    public int traitorsNum;
    [SerializeField] TMP_InputField codeInputField;
    [SerializeField] TMP_Text roomNameText;
    [SerializeField] Transform roomListContent;
    [SerializeField] Transform PlayerListContent;
    [SerializeField] GameObject roomListItemPrefab;
    [SerializeField] GameObject playerListItemPrefab;
    [SerializeField] GameObject startButton;
    [SerializeField] Button StartButton;
    

    private const int MaxPlayersPerRoom = 10;

    void Awake()
    {
        Instance = this;
    }


    //connect to Photon server
    void Start()
    {
        Pause.paused = false;
        Debug.Log("Connecting...");
        PhotonNetwork.ConnectUsingSettings();
        menuMusic.Play();
    }

    //When You connect to the master server do the following
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master");
        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true;
        
    }

    //When Joining the Lobby do the following
    public override void OnJoinedLobby()
    {
        StartMenu.SetActive(false);
        MainMenu.SetActive(true);
        Debug.Log("Joined Lobby");
        
    }

    //To Creat a Room
    public void CreateRoom()
    {
        int roomCode = Random.Range(10000, 99999);
        string roomName = roomCode.ToString();
        RoomOptions roomOps = new RoomOptions();
        roomOps.IsVisible = true;
        roomOps.IsOpen = true;
        roomOps.MaxPlayers = MaxPlayersPerRoom ;

        if (GameType.value == 0){
        PhotonNetwork.CreateRoom(roomName, roomOps, null);
        Debug.Log("Creating Public Room");
        }
        else
        {
        roomOps.IsVisible = false;
        PhotonNetwork.CreateRoom(roomName, roomOps, null);
        Debug.Log("Creating Private Room");
        }
        traitorsNum = TraitorsNum.value + 1;
        if (traitorsNum == 1)
            Debug.Log("Number of Traitors is 1");
        else if (traitorsNum == 2)
        {
            Debug.Log("Number of Traitors is 2");
        }
        else
        {
            Debug.Log("Number of Traitors is 3");
        }
    }

    //When the Room is created
    public override void OnCreatedRoom()
    {
        roomNameText.text = "Welcome to " + PhotonNetwork.CurrentRoom.Name;
        CreatRoomMenu.SetActive(false);
        RoomMenu.SetActive(true);
        CreateButton.SetActive(true);
        
        Debug.Log("Room Created");
    }

    //When a Plyer Clicks On the Room from the Room List
    public void JoinRoom(RoomInfo info)
    { 
        PhotonNetwork.JoinRoom(info.Name);
        Debug.Log("Joinning Room");
    }

    public void JoinRoomOnClick()
    {
        PhotonNetwork.JoinRoom(codeInputField.text);
        Debug.Log("Joinning Room");
    }

    //When a Player Joines a Room
    public override void OnJoinedRoom()
    {
        
        RoomListMenu.SetActive(false);
        RoomMenu.SetActive(true);
        roomNameText.text = "Welcome to " + PhotonNetwork.CurrentRoom.Name;

        Player[] player = PhotonNetwork.PlayerList;
        StartButton.interactable = true;
        if(PhotonNetwork.CurrentRoom.PlayerCount == MaxPlayersPerRoom)
            {
                PhotonNetwork.CurrentRoom.IsOpen  = false;
                Debug.Log("Match is ready to begin ");

            }
        
        foreach (Transform child in PlayerListContent)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < player.Count(); i++)
        {
            Instantiate(playerListItemPrefab, PlayerListContent).GetComponent<PlayerListItem>().SetUp(player[i]);
        }
        startButton.SetActive(PhotonNetwork.IsMasterClient); //Activate the "Start" Button to the Host only

        Debug.Log("Joined Room");
    }

    //To add a new Player to the List of Players
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Instantiate(playerListItemPrefab, PlayerListContent).GetComponent<PlayerListItem>().SetUp(newPlayer);
    }

    //if the Host leaves the Room another Player will beacome the Host and the "Start Button will be Activated"
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        startButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    //Leave the Room
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        Debug.Log("Leaving Room...");
    }

    //When Leaving the Room
    public override void OnLeftRoom()
    {
        RoomMenu.SetActive(false);
        MainMenu.SetActive(true);
        Debug.Log("Left Room"); 
    }

    //When a new Room is Created or Closed
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (Transform trans in roomListContent)
        {
            Destroy(trans.gameObject);
        }
        for (int i = 0; i < roomList.Count; i++)
        {
            if (roomList[i].RemovedFromList)
            continue;
        Instantiate(roomListItemPrefab, roomListContent).GetComponent<RoomListItem>().SetUp(roomList[i]);
        }
    }

    //When a Host Start the Game
    public void StartGame()
    {
        PhotonNetwork.CurrentRoom.IsOpen = false; // makes room close 
        PhotonNetwork.CurrentRoom.IsVisible = false; // makes room invisible to random match
        PhotonNetwork.LoadLevel(1);
        menuMusic.Stop();
    }

    //When Clicking on the Quit Button in the Main Menu, This will Close the Game
    public void QuitGame()
    {
        Debug.Log(PhotonNetwork.NickName + " Quited");
        Application.Quit();
    }

}
