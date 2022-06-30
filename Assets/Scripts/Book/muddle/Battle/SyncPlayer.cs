using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//我的想法 不做预测 直接插值

public class SyncPlayer : BasePlayer
{
    //预测信息，那个时间到哪个位置
    public float forecastTime;
    public Vector3 targetPos;

    public override void Init(string skinPath)
    {
        base.Init(skinPath);

        animator = GetComponentInChildren<Animator>();
        //不受物理运动影响
        rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;

        //初始化预测信息
        forecastTime = Time.time;
        
    }

    new void Update()
    {
        base.Update();
        //更新位置
        ForecastUpdate();
    }


    //会让玩家在指定时间移动到预测位置 不过我改成不预测 直接插值了
    public void ForecastUpdate()
    {
        float t = (Time.time - forecastTime) / CtrlPlayer.syncInterval;
        t= Mathf.Clamp(t,0f,1f);
        transform.position = Vector3.Lerp(transform.position, targetPos, t);
    }


    //更新预测位置
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
        //第一步得到枪！
        //前面已经排除了 不同步自己
        //找到SyncPlayer 再找到他的枪
        //怎么找到枪呢？
        
        Transform NeedSyncGun = GetComponentInChildren<SyncGun>().transform;
 
        NeedSyncGun.rotation= Quaternion.Euler(msg.ex, 0.0f, msg.ez);
        NeedSyncGun.localScale = new Vector3(1.0f, msg.scaleY, 1.0f);
    }

  
}
