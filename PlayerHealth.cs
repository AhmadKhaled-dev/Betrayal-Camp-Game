using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class PlayerHealth : MonoBehaviourPunCallbacks
{

    public static PlayerHealth PH;

    public PlayerController PC;
    public Image healthBar;
    public Image backHealthBar;
    public Text foodRemaining;
    public Text traitorText;

    private float health;
    private float lerpTimer;
    public const float maxHP = 100f;
    public float chipSpeed = 2f;
    private int food;
    private bool traitor;
    PhotonView PV;


    private void Awake()
    {
        PV = (PhotonView)PC.GetComponent<PhotonView>();
        if (!PV.IsMine)
        {
            Destroy(GetComponent<Canvas>().gameObject);
        }
    }

    // Start is called before the first frame update 
    void Start()
    {
        health = PC.currentHealth;
        food = 3;
        traitor = PlayerManager.localPlayer.isTraitor;
        foodRemaining.text = "Food remaining: " + food;
        traitorText.text = "Traitor: " + traitor;
    }

    // Update is called once per frame
    void Update()
    {
        health = PC.currentHealth;
        health = Mathf.Clamp(health, 0, maxHP);
        updateHealthUI();
        if (Input.GetKeyDown(KeyCode.H))
        {
            healUp();
        } 
        foodRemaining.text = "Food remaining: " + food;
        traitorText.text = "Traitor: " + traitor;
    }

    public void healUp()
    {
        if (food != 0 && health != maxHP)
        {
            PC.currentHealth += 20;
            Debug.Log("New Health " + PC.currentHealth);
            food--;
            lerpTimer = 0f;
        }

            
    }

public void updateHealthUI()
    {
        //Debug.Log(health);
        float fillHealthF = healthBar.fillAmount;
        float fillHealthB = backHealthBar.fillAmount;
        float healthFraction = health / maxHP;
        if (fillHealthB > healthFraction)
        {
            healthBar.fillAmount = healthFraction;
            backHealthBar.color = Color.red;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            backHealthBar.fillAmount = Mathf.Lerp(fillHealthB, healthFraction, percentComplete);
        } else if (fillHealthF < healthFraction)
        {
            backHealthBar.fillAmount = healthFraction;
            backHealthBar.color = Color.green;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / chipSpeed;
            percentComplete = percentComplete * percentComplete;
            healthBar.fillAmount = Mathf.Lerp(fillHealthF, healthFraction, percentComplete);

        }
    }
}
