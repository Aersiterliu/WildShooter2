using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour
{
    public float dashSpeed=30f;
    public float dashTime=0.2f;
    float startDashTimer;
    bool isDashing=false;
    public GameObject dashObj;
    public Rigidbody2D playerRigibody2d;
    private void Start()
    {
        playerRigibody2d= GameMain.myPlayer.GetComponent<Rigidbody2D>();
        dashObj = GetComponentInChildren<ParticleSystem>().gameObject;
    }

    private void Update()
    {
        if(!isDashing)
        {
          
            if(Input.GetKeyDown(KeyCode.Q))
            {
                
                dashObj.SetActive(true);
                isDashing = true;
                startDashTimer =dashTime;
            }
        }
        else
        {
            startDashTimer-=Time.deltaTime;
            if(startDashTimer <=0)
            {
                isDashing = false;
                dashObj.SetActive(false);   
            }
            else
            {
                playerRigibody2d.velocity = GameMain.myPlayer.GetComponent<CtrlPlayer>().direction*dashSpeed;
            }
        }
    }
}
