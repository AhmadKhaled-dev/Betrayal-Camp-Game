using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class DirectToTask : MonoBehaviourPunCallbacks
{
    public static DirectToTask direct;
    PhotonView PV;

    public PlayerController PC;
    private Image iconImg;
    private Text distanceText;

    public Transform player;
    public Vector3 target1;
    public Vector3 target2;
    public Vector3 target3;
    public Camera cam;
    public float closeEnoughDist = 3.0f;

    void Awake()
    {
        PV = GetComponentInParent<PhotonView>();
        direct = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        iconImg = GetComponent<Image>();
        distanceText = GetComponentInChildren<Text>();
        target1 = GameObject.Find("NumberOrderTaskLocation").transform.position;
        target2 = GameObject.Find("CollectAppleTaskLocation").transform.position;
        target3 = GameObject.Find("CollectWoodTaskLocation").transform.position;
        if (!PV.IsMine)
            return;
        //cam = GetComponent<Camera>();
    }

    // Update is called once per frame
   void Update()
    {
            if (target1 != null && target1 != Vector3.zero)
            {
                GetDistance(target1);
                CheckOnScreen(target1);
            }
        
    }

    private void GetDistance(Vector3 target)
    {
        if (target != null)
        {
            float dist = Vector3.Distance(player.position, target);
            distanceText.text = dist.ToString("f1") + "m";

            if (dist < closeEnoughDist)
            {
                Destroy(gameObject);
            }
        }
    }

    private void CheckOnScreen(Vector3 target)
    {
        if (cam != null)
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
    }

    private void ToggleUI(bool _value)
    {
        iconImg.enabled = _value;
        distanceText.enabled = _value;
    }
}
