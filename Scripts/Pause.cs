using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Photon.Pun;
using UnityEngine;


public class Pause : MonoBehaviourPunCallbacks
{
    public static Pause pause;
    public static bool paused = false;
    private bool disconnecting = false;

    public void TogglePause()
    {
        if (disconnecting) return;
    
        paused = !paused;
    
        transform.GetChild(0).gameObject.SetActive(paused);
    }
    
    public void Quit()
    {
        Debug.Log("You have quit");
        disconnecting = true;
        Destroy(RoomManager.Instance.gameObject);
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene(0);
        base.OnLeftRoom();
    }
}
