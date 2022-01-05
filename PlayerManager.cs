using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.IO;
using UnityEngine.SceneManagement;
public class PlayerManager : MonoBehaviourPunCallbacks, IPunObservable
{
    public static PlayerManager localPlayer;
    PhotonView PV;
    public bool isTraitor;

    GameObject controller;
    

    int mySelectedPlayer;

    Player[] allPlayers;
    int myNumInRoom;
    int my = 0;

    void Awake()
    {
        PV = GetComponent<PhotonView>();
    }
    private void  OnEnable()
    {
        if (PlayerManager.localPlayer == null)
        {
            PlayerManager.localPlayer = this;
        }
        else
        {
            if (PlayerManager.localPlayer != this)
            {
                //Destroy(PlayerManager.localPlayer.gameObject);
                PlayerManager.localPlayer = this;
            }
        }
        //DontDestroyOnLoad(this.gameObject);
    }
    void Start()
    {
        if (PlayerPrefs.HasKey("MyPlayer"))
        {
            mySelectedPlayer = PlayerPrefs.GetInt("MyPlayer");

        }

        else
        {
            mySelectedPlayer = 0;
            PlayerPrefs.GetInt("MyPlayer", mySelectedPlayer);
        }
        allPlayers = PhotonNetwork.PlayerList;

        foreach (Player p in allPlayers)
        {
            if (p != PhotonNetwork.LocalPlayer)
            {
                myNumInRoom++;
            }
        }

        if (PV.IsMine) //this will be for a specific player and it will not afect others
        {
            CreatController();
            while (GameController.GC.traitors > 0 && my==0)
            {
                PlayerManager.localPlayer.pickTraitor();
            }
        }

    }

    // This Function will Creat the Controllers For Each Player
    void CreatController()
    {
        controller = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerController"), SpawnPlayers.instance.spawnPoints[Random.Range(0, 10)].position, Quaternion.identity, 0, new object[] { PV.ViewID });
    }
    

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(isTraitor);
        }
        else
        {
            this.isTraitor = (bool)stream.ReceiveNext();
        }
    }
    public void pickTraitor()
    {
        if (GameController.GC.traitors > 0)
        {
            int traitor = Random.Range(0, 2);
            PV.RPC("RPC_syncTraitor", RpcTarget.All, traitor);
        }
    }

    [PunRPC]
    void RPC_syncTraitor(int traitor)
    {
        if(traitor == 1 && GameController.GC.traitors > 0)
        {
            Debug.Log("This Player is a Traitor :" + PV.Owner);
            this.isTraitor = true;
            GameController.GC.traitors -= 1;
            my++;
            Debug.Log("Traitors Num#" + GameController.GC.traitors);
        }
        Debug.Log("Traitors Num#" + GameController.GC.traitors);

    }
    

    public void Die()
    {
        Destroy(RoomManager.Instance.gameObject);
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene(0);
        base.OnLeftRoom();
    }
    
}