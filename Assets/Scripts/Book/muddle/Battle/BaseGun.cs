using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseGun : MonoBehaviour
{ 

    private GameObject skin;

    public GameObject bulletPrefab;
    public GameObject shellPrefab;

    protected Transform muzzlePos;
    protected Transform shellPos;

    


    public float fireCd = 0.5f;
    public float lastFireTime = 0;

    //用这把枪的player
    public BasePlayer player;


    public float interval = 0.5f;

    protected float timer;

    public void Update()
    {

    }

    public virtual void Init(string skinPath)
    {
        
    }


    public Bullet Fire()
    {
        if (GameMain.myPlayer.GetComponent<BasePlayer>().IsDie())
        {
            return null;
        }


        Bullet bullet = ObjectPool.Instance.GetObject(ResManager.LoadPrefab("Bullet")).GetComponent<Bullet>();

        bullet.player = GameMain.myPlayer.GetComponent<BasePlayer>();//需要改
        //位置
        bullet.transform.position = muzzlePos.position;
        bullet.transform.rotation = muzzlePos.rotation;

        bullet.rigidbody2d = bullet.GetComponent<Rigidbody2D>();
        bullet.rigidbody2d.velocity = transform.right * bullet.speed;

        GameObject shell = ObjectPool.Instance.GetObject(ResManager.LoadPrefab("BulletShell"));
        shell.transform.position = shellPos.position;
        shell.transform.rotation = shellPos.rotation;

        lastFireTime = Time.time;

        return bullet;
    }

}
