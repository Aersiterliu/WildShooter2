using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePlayer : MonoBehaviour
{




    private GameObject skin;

    //服务端resetPlayer的时候设置了真正的血量
    public float hp = 120;
    public float speed = 5.0f;

    public string id = "";


    public Vector2 input;

    //玩家的鼠标方向 每个玩家都有 但是只有CtrlPlayer才能使用它，每个CtrlGun必有CtrlPlayer
    public Vector2 mousePos;
    public Vector2 direction;

    public Animator animator;
    new public Rigidbody2D rigidbody;

    HealthBar healthBar;

    public virtual void Init(string skinPath)
    {
        GameObject skinRes=ResManager.LoadPrefab(skinPath);
        skin=(GameObject)Instantiate(skinRes);
        skin.transform.parent = transform;
        skin.transform.localPosition = Vector3.zero;
        skin.transform.localEulerAngles = Vector3.zero;
        gameObject.tag = "Player";

        //animator = gameObject.AddComponent<Animator>();

        
        rigidbody = gameObject.AddComponent<Rigidbody2D>();
        //rigidbody.bodyType = RigidbodyType2D.Static;
        rigidbody.gravityScale = 0f;
        rigidbody.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;


        BoxCollider2D boxCollider=gameObject.AddComponent<BoxCollider2D>();
        boxCollider.offset = new Vector2(0, -0.25f);
        boxCollider.size = new Vector2(1, 0.5f);

        healthBar=GetComponentInChildren<HealthBar>();
    }
    public void Update()
    {
      
    }

    public bool IsDie()
    {
        return hp <=0;
    }

    public void Attacked(float att)
    {
        if(IsDie())
        {
            return;
        }
        Debug.Log(id + "被击中！");

        healthBar.hp-=att;
        hp -= att;

        
        if(IsDie())
        {
            Debug.Log(id + "死了！");
        }
    }
}
