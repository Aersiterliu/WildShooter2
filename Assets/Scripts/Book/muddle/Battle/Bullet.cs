using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed=15f;


    //发射者
    public BasePlayer player;

    //子弹prefab
    private GameObject skin;
    private GameObject bulletshell;

    public GameObject explosionPrefab;
    public Rigidbody2D rigidbody2d;



    public void Start()
    {
        rigidbody2d=GetComponent<Rigidbody2D>();
    }

    //Init 用不上
    public void Init()
    {
        GameObject skinRes = ResManager.LoadPrefab("Bullet");


        skin=ObjectPool.Instance.GetObject(skinRes);
        //skin=(GameObject)Instantiate(skinRes);
        //skin.transform.parent=this.transform;
        skin.transform.localScale = Vector3.zero;
        skin.transform.localEulerAngles = Vector3.zero;


        //物理
        rigidbody2d = gameObject.GetComponent<Rigidbody2D>();
        
    }


    public void Update()
    {
        //transform.position += transform.forward * speed * Time.deltaTime;
    }

    

    public void SetSpeed(Vector2 direction)
    {
        rigidbody2d.velocity = direction * speed;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {

  
        //打到的角色
        GameObject collObj = other.gameObject;
        BasePlayer hitPlayer=collObj.GetComponent<BasePlayer>();
        //不能打自己
        if(hitPlayer==player)
        {
            return;
        }

        //打到其他角色
        if(hitPlayer!=null)
        {
            SendMsgHit(player, hitPlayer);
        }

        //生成的爆炸特效 不对 以后再解决
        //GameObject exp = ResManager.LoadPrefab("Explosion");
        //ObjectPool.Instance.GetObject(exp);
        //exp.transform.position = this.transform.position;

        
        //摧毁自身 也就是这个子弹
        ObjectPool.Instance.PushObject(gameObject);

    }

    void SendMsgHit(BasePlayer player,BasePlayer hitPlayer)
    {
        if(hitPlayer==null||player==null)
        {
            return;
        }

        //不是自己发出的炮弹
        if (player.id!=GameMain.id)
        {
            return;
        }
        MsgHit msg = new MsgHit();
        msg.targetId = hitPlayer.id;
        msg.id = player.id;
        //击中点
        msg.x = transform.position.x;
        msg.y = transform.position.y;
        msg.z = transform.position.z;
        NetManager.Send(msg);
    }
  



}
