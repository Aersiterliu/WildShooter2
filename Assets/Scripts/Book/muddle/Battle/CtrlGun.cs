using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CtrlGun : BaseGun
{
    //net
    //��һ�η���ͬ����Ϣ��ʱ��
    private float lastSendSyncTime = 0;


    public float flipY;
    public Animator animator;
    protected Vector2 mousePos;
    protected Vector2 direction;

    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        muzzlePos = transform.Find("Muzzle");
        shellPos = transform.Find("BulletShell");
        flipY = transform.localScale.y;
        bulletPrefab = ResManager.LoadPrefab("Bullet");
   
    }

     new void Update()
    {

        mousePos = GameMain.myPlayer.GetComponent<CtrlPlayer>().mousePos;
        direction = GameMain.myPlayer.GetComponent<CtrlPlayer>().direction;

        transform.right = direction;

        if (mousePos.x < transform.position.x)
            transform.localScale = new Vector3(flipY, -flipY, 1);
        else
            transform.localScale = new Vector3(flipY, flipY, 1);

        FireUpdate();


        //����ͬ����Ϣ
        SyncUpdate();
    }

    public void FireUpdate()
    {
        if(GameMain.myPlayer.GetComponent<BasePlayer>().IsDie())
        {
            return;
        }
  
        if (!Input.GetButton("Fire1"))
        {
            return;
        }
        //cd�Ƿ��ж�
        if(Time.time-lastFireTime<fireCd)
        {
            return ;
        }


        Bullet bullet=Fire();
        //����ͬ��Э��
        MsgFire msg = new MsgFire();
        msg.x=  bullet.transform.position.x;
        msg.y = bullet.transform.position.y;
        msg.z = bullet.transform.position.z;
        msg.ex = bullet.transform.eulerAngles.x;
        msg.ey = bullet.transform.eulerAngles.y;
        msg.ez = bullet.transform.eulerAngles.z;
        NetManager.Send(msg);   
    }




    void SyncUpdate()
    {
        //ʱ�����ж�
        if (Time.time - lastSendSyncTime < CtrlPlayer.syncInterval)
        {
            return;
        }
        lastSendSyncTime = Time.time;
        //����ͬ��Э��
        MsgSyncGunRot msg = new MsgSyncGunRot();
        //�����д�������������
        //transform.rotation.z Ϊʲô����Ϊ�Ƕ�Ҫת��
        msg.ex = transform.localEulerAngles.x;
        msg.ez = transform.localEulerAngles.z;
        msg.scaleY=transform.localScale.y;
        NetManager.Send(msg);
    }




}
