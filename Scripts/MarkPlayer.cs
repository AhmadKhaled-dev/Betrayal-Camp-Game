using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class MarkPlayer : MonoBehaviourPunCallbacks
{
    public static MarkPlayer markPlayer;
    PhotonView PV;

    public PlayerController PC;
    private Image iconImg;
    private Text distanceText;

    public Transform player;
    public Vector3 target;
    public Camera cam;
    public float timeRemaining = 15;
    public float closeEnoughDist;

    /*public void setTarget(Vector3 t)
    {
        target = t; 
    }

    public Vector3 getTarget()
    {
        return target;
    }*/
    void Awake()
    {
        PV = GetComponent<PhotonView>();
        markPlayer = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        iconImg = GetComponent<Image>();
        distanceText = GetComponentInChildren<Text>();

    }

    // Update is called once per frame
    void Update()
    {
        
        if (target != null && target != Vector3.zero)
            {
                GetDistance();
                CheckOnScreen();
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                Debug.Log("Time " + timeRemaining);
            }
        }
    }

    private void GetDistance()
    {
        float dist = Vector3.Distance(player.position, target);
        distanceText.text = dist.ToString("f1") + "m";

        if (timeRemaining < 0)
        {
            Destroy(gameObject);
        }
    }

    private void CheckOnScreen()
    {
        float thing = Vector3.Dot((target - cam.transform.position).normalized, cam.transform.forward);

        if (thing <= 0)
        {
            ToggleUI(false);
        }
        else
        {
            ToggleUI(true);
            transform.position = cam.WorldToScreenPoint(target);
        }
    }

    private void ToggleUI(bool _value)
    {
        iconImg.enabled = _value;
        distanceText.enabled = _value;
    }
}
