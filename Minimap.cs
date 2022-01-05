using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Minimap : MonoBehaviourPunCallbacks
{
    public Transform player;
    PhotonView PV;
    void Awake()
    {
        PV = GetComponent<PhotonView>();
        Debug.Log("Player's Grand parent: " + transform.parent.transform);
        if (!PV.IsMine)
        {
            Destroy(GetComponent<Camera>().gameObject);
        }

        if (PV.IsMine)
        {
            player = transform.parent.transform;
        }
    }

    void LateUpdate()
    {

        Vector3 newPosition = player.position;
        newPosition.y = transform.position.y;
        transform.position = newPosition;
        transform.rotation = Quaternion.Euler(90f, player.eulerAngles.y, 0f);
        
    }
}
