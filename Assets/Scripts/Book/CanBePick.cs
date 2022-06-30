using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanBePick : MonoBehaviour
{
    public int index = 1;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag=="Player")
        {
            MsgPickItem msg=new MsgPickItem();
            msg.itemName=this.gameObject.name;
            NetManager.Send(msg);
        }
    }
}
