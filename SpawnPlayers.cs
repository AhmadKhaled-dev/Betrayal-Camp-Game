using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.IO;

public class SpawnPlayers : MonoBehaviour
{
    public static SpawnPlayers instance;

    public Transform[] spawnPoints;
    // Start is called before the first frame update 
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
