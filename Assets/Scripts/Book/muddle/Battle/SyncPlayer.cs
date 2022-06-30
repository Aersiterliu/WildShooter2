using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�ҵ��뷨 ����Ԥ�� ֱ�Ӳ�ֵ

public class SyncPlayer : BasePlayer
{
    //Ԥ����Ϣ���Ǹ�ʱ�䵽�ĸ�λ��
    public float forecastTime;
    public Vector3 targetPos;

    public override void Init(string skinPath)
    {
        base.Init(skinPath);

        animator = GetComponentInChildren<Animator>();
        //���������˶�Ӱ��
        rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;

        //��ʼ��Ԥ����Ϣ
        forecastTime = Time.time;
        
    }

    new void Update()
    {
        base.Update();
        //����λ��
        ForecastUpdate();
    }


    //���������ָ��ʱ���ƶ���Ԥ��λ�� �����Ҹĳɲ�Ԥ�� ֱ�Ӳ�ֵ��
    public void ForecastUpdate()
    {
        float t = (Time.time - forecastTime) / CtrlPlayer.syncInterval;
        t= Mathf.Clamp(t,0f,1f);
        transform.position = Vector3.Lerp(transform.position, targetPos, t);
    }


    //����Ԥ��λ��
    public void SyncPos(MsgSyncCharacter msg)
    {

        targetPos = new Vector3(msg.x, msg.y, 0);

        if (transform.position != new Vector3(msg.x,msg.y,0))
        {
            animator.SetBool("isMoving", true);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }
        forecastTime= Time.time;

        this.transform.rotation = Quaternion.Euler(new Vector3(0, msg.ey, 0));

    }


    public void SyncGunRot(MsgSyncGunRot msg)
    {
        //��һ���õ�ǹ��
        //ǰ���Ѿ��ų��� ��ͬ���Լ�
        //�ҵ�SyncPlayer ���ҵ�����ǹ
        //��ô�ҵ�ǹ�أ�
        
        Transform NeedSyncGun = GetComponentInChildren<SyncGun>().transform;
 
        NeedSyncGun.rotation= Quaternion.Euler(msg.ex, 0.0f, msg.ez);
        NeedSyncGun.localScale = new Vector3(1.0f, msg.scaleY, 1.0f);
    }

  
}
