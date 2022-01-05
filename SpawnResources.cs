using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnResources : MonoBehaviour
{

    public GameObject resourcePrefab;
    public Vector3 center, size;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            spawn();
        }
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    void spawn()
    {
        Vector3 position = new Vector3(Random.Range(-size.x / 2, size.x / 2), 1, Random.Range(-size.z / 2, size.z / 2));
        Instantiate(resourcePrefab, transform.localPosition + position, Quaternion.identity);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1,0,0,0.5f);
        Gizmos.DrawCube(transform.localPosition + center, size);

    }

}
