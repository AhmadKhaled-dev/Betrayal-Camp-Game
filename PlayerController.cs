using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEditor;
using TMPro;
using UnityEngine.UI;

public class PlayerController : MonoBehaviourPunCallbacks, IDamageable
{
    public static PlayerController instance;
    [SerializeField] Camera cam;
    [SerializeField] GameObject cameraHolder;
    [SerializeField] float mouseSensitivity, sprintSpeed, walkSpeed, jumbForce, smoothTime;

    public PlayerHealth PH;
    public GameController gameController;
    public PlayerManager playerManager;
    public Slider ProgressSlider;

    public GameObject WiningPanal;
    public TMP_Text WiningText;
   
    float verticalLookRotation;
    bool _grounded;
    public bool dead = false;
    Vector3 smoothMoveVelocity;
    Vector3 moveAmount;
    
    Rigidbody rb;
    PhotonView PV;
    private Animator anim;

    public const float maxHealth = 100f;
    public float currentHealth = maxHealth;
    public int woodCount = 0;
    public int appleCount = 0;
    [Header("Camp")]
    public GameObject tent;
    public GameObject fire;

    public GameObject AppleGenraitor;
    bool isDestroyed;

    public Text damageText;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        PV = GetComponent<PhotonView>();
        instance = this;
        playerManager = PhotonView.Find((int)PV.InstantiationData[0]).GetComponent<PlayerManager>();
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        ProgressSlider = (Slider)GameObject.FindObjectOfType(typeof(Slider));
        ProgressSlider.maxValue = 10;
        instance.GetComponent<Outline>().enabled = false;
    }

    void Start()
    {
        Mark(false);
        if (!PV.IsMine)
        {
            Destroy(GetComponentInChildren<Camera>().gameObject);
            Destroy(rb);
        }
        if (PV.IsMine)
        {
            anim = GetComponent<Animator>();
        }
        
        //Instantiate(AppleGenraitor, SpawnPlayers.instance.spawnPoints[UnityEngine.Random.Range(0, 10)].position, Quaternion.identity);
        
       
    }
    private void Update()
    {
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        updateTaskStatus();
        if (!PV.IsMine)
            return;

        
        isDestroyed = gameController.isDestroyed;
        if (Input.GetMouseButtonDown(0))
        {
           damage();
        }

        if (transform.position.y < -10f) // Die if you fall out of the world
        {
            Die();
        }

        if (!dead)
        {
            bool pause = Input.GetKeyDown(KeyCode.Escape);

            //Pause
            if (pause)
            {
                GameObject.Find("Pause").GetComponent<Pause>().TogglePause();
            }

            if (Pause.paused) return;

            Look();
            Move();
            if (Input.GetKeyDown(KeyCode.E)  && isDestroyed == false && tent !=null /*&& playerManager.isTraitor == true*/)
            {
                Debug.Log("sabotage CAmp");
                photonView.RPC("sabotageCamp", RpcTarget.All);
               
            }
            if (Input.GetKeyDown(KeyCode.M) && isDestroyed == true)
            {
                Debug.Log("fix CAmp");
                photonView.RPC("fixCamp", RpcTarget.All);
                
            }

            if (Input.GetKeyDown(KeyCode.X))
            {
                markplayer();
            }

        }
    }

    void Look()
    {
        transform.Rotate(Vector3.up * Input.GetAxisRaw("Mouse X") * mouseSensitivity);
        verticalLookRotation += Input.GetAxisRaw("Mouse Y") * mouseSensitivity;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);

        cameraHolder.transform.localEulerAngles = Vector3.left * verticalLookRotation;

    }
    void Move()
    {
        Vector3 moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;

        moveAmount = Vector3.SmoothDamp(moveAmount, moveDir * (Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed), ref smoothMoveVelocity, smoothTime);
        

        //Activate and deactivate the running animation
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
        { 
            anim.SetBool("isRunning", true);
        }
        else if(Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            anim.SetBool("isRunning", false);
        }

        //Activate and deactivate the fighting animation
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            anim.SetBool("isFighting", true);
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            anim.SetBool("isFighting", false);
        }
    }

    public void updateTaskStatus()
    {
        if (woodCount > 4 )
        {
            Debug.Log("Wood Gattherd");
            woodCount = 0;
            GameController.GC.taskStatus++;
            GameController.GC.photonView.RPC("sendTaskStatus", RpcTarget.All, GameController.GC.taskStatus);
        }
        if (appleCount > 4 )
        {
            appleCount = 0;
            GameController.GC.taskStatus++;
            GameController.GC.photonView.RPC("sendTaskStatus", RpcTarget.All, GameController.GC.taskStatus);
        }



    }

    void FixedUpdate()
    {
        if (!PV.IsMine)
            return;
        rb.MovePosition(rb.position + transform.TransformDirection(moveAmount) * Time.deltaTime);
    }


    public void TakeDamage(float damage)
    {
        if(damage == 1)
        {
            Debug.Log("Mark: " + damage);
            PV.RPC("RPC_TakeDamage", RpcTarget.All, damage);
        }
        else
        {
            Debug.Log("Took Damage" + damage);
            PV.RPC("RPC_TakeDamage", RpcTarget.All, damage);
        }
        
    }

    [PunRPC]
    void RPC_TakeDamage(float damage)
    {
        

        if(damage == 1)
        {
            Mark(true);
        }
        else
        {
            if (!PV.IsMine)
                return;


            currentHealth -= damage;

            if (currentHealth <= 0)
            {
                dead = true;
                anim.SetBool("isAlive", false);
                Invoke("Die", 4.0f);
            }
        }
        
    }

    public void Die()
    {
        if (PlayerManager.localPlayer.isTraitor)
            GameController.GC.traitorPlayer--;

        playerManager.Die();
    }

    public void damage()
    {

        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        ray.origin = cam.transform.position;
        int layerMask = 1 << 8;
        if (Physics.Raycast(ray, out RaycastHit hit, 5.0f, layerMask))
        {
            if (!hit.collider.GetComponent<PhotonView>().IsMine)
            {
                hit.collider.gameObject.GetComponent<IDamageable>()?.TakeDamage(25);
                damageT();
                Debug.Log("You Hit " + hit.collider.gameObject.name);
            }
                
        }
        
    }

    void damageT()
    {
        damageText.text = "Damage: " + 25;
        Invoke("unDamage", 0.5f);
    }
    void unDamage()
    {
        damageText.text = " ";
    }
    [PunRPC]
    public void sabotageCamp()
    {
        GameObject.Find("GameController").GetComponent<GameController>().isDestroyed = true;
        gameController.destroyCamp();
    }

    [PunRPC]
    public void fixCamp()
    {
        GameObject.Find("GameController").GetComponent<GameController>().isDestroyed = false;
        gameController.createCamp();
    }

  public void Mark(Boolean m)
    {
        GetComponent<Outline>().enabled = m;
        Invoke("unMark", 10.0f);
    }

    public void unMark()
    {
        GetComponent<Outline>().enabled = false;
    }

    public void markplayer()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f));
        ray.origin = cam.transform.position;
        int layerMask = 1 << 8;
        if (Physics.Raycast(ray, out RaycastHit hit, 5.0f, layerMask))
        {
            if (!hit.collider.GetComponent<PhotonView>().IsMine)
            {
                hit.collider.gameObject.GetComponent<IDamageable>()?.TakeDamage(1);
                Debug.Log("You Marked " + hit.collider.gameObject.name);
            }

        }
    }

}

