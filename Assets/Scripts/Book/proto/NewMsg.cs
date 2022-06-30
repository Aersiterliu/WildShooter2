using System;
using System.Collections;
using UnityEngine;


public class MsgPickItem : MsgBase
{
    public MsgPickItem() { protoName = "MsgPickItem"; }
    //看捡了哪一个物品
    public string itemName ="";

    //服务端补充 谁捡了什么东西
    public string id = "";

}
