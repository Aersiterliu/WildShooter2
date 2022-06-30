using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//������ô�Ӽ����أ�
public class CtrlPlayer : BasePlayer
{
    //��һ�η���ͬ����Ϣ��ʱ��
    private float lastSendSyncTime=0;
    //ͬ��֡��
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

        //����ͬ����Ϣ
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
        //ʱ�����ж�
        if(Time.time - lastSendSyncTime < syncInterval)
        {
            return;
        }
        lastSendSyncTime=Time.time;
        //����ͬ��Э��
        MsgSyncCharacter msg = new MsgSyncCharacter();
        msg.x = transform.position.x;
        msg.y = transform.position.y;
        msg.ey= transform.eulerAngles.y;   
        NetManager.Send(msg);   
    }


}
