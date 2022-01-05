using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectResources : MonoBehaviour
{
    
    private void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.tag == "Player" && gameObject.tag == "Apple")
        {
            PlayerController.instance.appleCount++;
            Debug.Log(" " + PlayerController.instance.appleCount);
            Destroy(gameObject);
        }
        
        if (collider.gameObject.tag == "Player" && gameObject.tag == "Wood")
        {
            PlayerController.instance.woodCount++;
            Debug.Log(" " + PlayerController.instance.woodCount);
            Destroy(gameObject);
        }
    }
}
