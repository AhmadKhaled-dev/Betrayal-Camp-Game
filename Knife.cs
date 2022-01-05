using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class knife : MonoBehaviour
{
    [SerializeField] Camera cam;
    public string itemName;
    public void damage()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        ray.origin = cam.transform.position;
        if(Physics.Raycast(ray, out RaycastHit hit))
        {
            Debug.Log("We Hit" + hit.collider.gameObject.name);
        }
    }
}
