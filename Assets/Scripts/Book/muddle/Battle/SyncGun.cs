using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SyncGun : BaseGun
{

    public Animator animator;
    public void Start()
    {
        animator = GetComponent<Animator>();
        muzzlePos = transform.Find("Muzzle");
        shellPos = transform.Find("BulletShell");
        bulletPrefab = ResManager.LoadPrefab("Bullet");
    }



    //����ͬ���ٶ� ֱ��ͬ��λ�ã���������������
    public void SyncFire(MsgFire msg)
   {
        Bullet bullet = Fire();

        //��������
        Vector3 pos = new Vector3(msg.x, msg.y, msg.z);
        Vector3 rot=new Vector3(msg.ex, msg.ey, msg.ez);
        bullet.transform.position = pos;
        bullet.transform.eulerAngles = rot;

   }

}
