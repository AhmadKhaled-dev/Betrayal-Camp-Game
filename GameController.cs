using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System.IO;

public class GameController : MonoBehaviourPunCallbacks
{

    public static GameController GC;
    PhotonView PV;

    public Text timerUI;

    public int traitorPlayer;
    public int traitors;
    public int inoccentNumber;
    public int minutes;
    public int seconds;
    public int end = 0;
    public Text teamText;
    public int taskStatus = 0;

    GameObject tent;
    GameObject fire;

    public int numOfTraitroes;

    public bool isDestroyed = false;
    Vector3 tentLocation = new Vector3 { x = 334.5f, y = 0.1f, z = 500.5f };
    Vector3 fireLocation = new Vector3 { x = 337f, y = 0.5f, z = 500f };

    void Awake()
    {
        GameController.GC = this;
        PV = GetComponent<PhotonView>();
    }
    private void OnEnable()
    {
        if (GameController.GC == null)
        {
            GameController.GC = this;
        }
        else
        {
            if (GameController.GC != this)
            {
                //Destroy(PlayerManager.localPlayer.gameObject);
                GameController.GC = this;
            }
        }
        //DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    { 
        createCamp();
        if (PhotonNetwork.IsMasterClient)
        {
            numOfTraitroes = ((int)Launcher.Instance.TraitorsNum.value)+1;
            Debug.Log("There Should Be This Number of Traitors "+numOfTraitroes);
            traitors = numOfTraitroes;
        }
        StartCoroutine("timerGame");
    }

    public void Update()
    {
        inoccentNumber = PhotonNetwork.CurrentRoom.PlayerCount;
        inoccentNumber -= numOfTraitroes;
        teamText.text = "Innocents: " + inoccentNumber + "\n Traitors: " + numOfTraitroes;
        if (end <= 0)
        {
            wins();
        }

    }

    public void destroyCamp()
    {
        PhotonNetwork.Destroy(tent);
        PhotonNetwork.Destroy(fire);
    }

    public void createCamp()
    {
        tent = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "tent"), tentLocation, Quaternion.identity);
        fire = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "fire"), fireLocation, Quaternion.identity);
    }

    [PunRPC]
    void WinGame(string team)
    {
        if (PlayerController.instance.WiningPanal != null)
        {
            PlayerController.instance.WiningPanal.SetActive(true);
            PlayerController.instance.WiningText.SetText(team + " Won");
            Debug.Log(team);
            Invoke("endGame", 5.0f);
        }
        
    }

    [PunRPC]
    public void sendTaskStatus(int newTask)
    {
        taskStatus = newTask;
        Debug.Log("Enterd RPC " + newTask);
        PlayerController.instance.ProgressSlider.value = newTask;

    }

    public void wins()
    {
            //-------------------Bug--------------------
            if (taskStatus >= 10 && inoccentNumber >= numOfTraitroes)
            {
                WinGame("Innocent");
                end++;
            }

            if (numOfTraitroes > inoccentNumber)
            {
               WinGame("Traitors");
               end++;
            }
            //-------------------Bug--------------------
    }
    public IEnumerator timerGame()
    {
        int minute = minutes;
        while (minute >= 0)
        {
            if(seconds == 0)
            {
                minute--;
                for(int second = 59; second >= 0; second--)
                {
                    if(second > 10 && minute > 10)
                    {
                        timerUI.text = "Time " + minute + ":" + second;
                        //Debug.Log("Time " + minute + ":" + second);
                        yield return new WaitForSeconds(1);
                    }
                    else if(second < 10 && minute > 10)
                    {
                        timerUI.text = "Time " + minute + ":0" + second;
                        //Debug.Log("Time " + minute + ":" + second);
                        yield return new WaitForSeconds(1);
                    }
                    else if(second < 10 && minute < 10)
                    {
                        timerUI.text = "Time 0" + minute + ":0" + second;
                        //Debug.Log("Time " + minute + ":" + second);
                        yield return new WaitForSeconds(1);
                    }
                    else
                    {
                        timerUI.text = "Time 0" + minute + ":" + second;
                        //Debug.Log("Time " + minute + ":" + second);
                        yield return new WaitForSeconds(1);
                    }
                    
                }
                if(minute == 0)
                {
                    WinGame("Traitors");
                }
            }
            else
            {
                for (int second = seconds; second >= 0; second--)
                {
                    /*timerUI.text = "Time " + minute + ":" + second;
                    Debug.Log("Time " + minute + ":" + second);
                    yield return new WaitForSeconds(1);*/

                    if (second > 10 && minute > 10)
                    {
                        timerUI.text = "Time " + minute + ":" + second;
                        //Debug.Log("Time " + minute + ":" + second);
                        yield return new WaitForSeconds(1);
                    }
                    else if (second < 10 && minute > 10)
                    {
                        timerUI.text = "Time " + minute + ":0" + second;
                        //Debug.Log("Time " + minute + ":" + second);
                        yield return new WaitForSeconds(1);
                    }
                    else if (second < 10 && minute < 10)
                    {
                        timerUI.text = "Time 0" + minute + ":0" + second;
                        //Debug.Log("Time " + minute + ":" + second);
                        yield return new WaitForSeconds(1);

                        if(second == 0 && minute == 0)
                        {
                            WinGame("Traitors");
                        }
                    }
                    else
                    {
                        timerUI.text = "Time 0" + minute + ":" + second;
                        //Debug.Log("Time " + minute + ":" + second);
                        yield return new WaitForSeconds(1);
                    }

                }
            }
        }
    }

    void endGame()
    {
        Debug.Log("End: " + end);
        PlayerController.instance.Die();
    }
}