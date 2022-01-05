using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEditor;
using TMPro;
using UnityEngine.UI;

public class MiniGame : MonoBehaviour
{
    [SerializeField] int nextButton;
    [SerializeField] GameObject GamePanal;
    [SerializeField] GameObject[] myObjects;
    bool isDone = false;
    // Start is called before the first frame update
    void Start()
    {
        nextButton = 0;
    }

    private void OnEnable()
    {
        nextButton = 0;
        for(int i = 0; i < myObjects.Length; i++)
        {
            myObjects[i].transform.SetSiblingIndex(UnityEngine.Random.Range(0, 9));
                
        }
    }
    
    public void ButtonOrder(int button)
    {
        if(button == nextButton)
        {
            nextButton++;
        }
        else
        {
            nextButton = 0;
            OnEnable();
        }
        if(button == 9 && nextButton == 10)
        {

                GameController.GC.taskStatus++;
                GameController.GC.photonView.RPC("sendTaskStatus", RpcTarget.All, GameController.GC.taskStatus);
                isDone = true;
            nextButton = 0;
            ButtonOrderPanalClose();
        }
    }

    public void ButtonOrderPanalOpen()
    {

            GamePanal.SetActive(true);

    }
    public void ButtonOrderPanalClose()
    {
        GamePanal.SetActive(false);
    }

}
