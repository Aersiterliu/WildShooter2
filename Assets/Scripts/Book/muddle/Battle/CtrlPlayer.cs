using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//想想怎么加技能呢？
public class CtrlPlayer : BasePlayer
{
    //上一次发送同步信息的时间
    private float lastSendSyncTime=0;
    //同步帧率
    public static float syncInterval = 0.02f;

     private void Start()
    {
        animator=GetComponentInChildren<Animator>();
    }


    new void Update()
    {


        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        direction = (mousePos - new Vector2(transform.position.x, transform.position.y)).normalized;


        base.Update();
        MoveUpdate();

        //发送同步信息
        SyncUpdate();
    }



    public void MoveUpdate()
    {
        if(IsDie())
        {
            return;
        }
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");

        rigidbody.velocity = input.normalized * speed;
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (mousePos.x > transform.position.x)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }
        else
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
        }

        if (input != Vector2.zero)
            animator.SetBool("isMoving", true);
        else
            animator.SetBool("isMoving", false);
    }


    public void SyncUpdate()
    {
        //时间间隔判断
        if(Time.time - lastSendSyncTime < syncInterval)
        {
            return;
        }
        lastSendSyncTime=Time.time;
        //发送同步协议
        MsgSyncCharacter msg = new MsgSyncCharacter();
        msg.x = transform.position.x;
        msg.y = transform.position.y;
        msg.ey= transform.eulerAngles.y;   
        NetManager.Send(msg);   
    }


}
