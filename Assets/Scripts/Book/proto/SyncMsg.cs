using System;
using System.Collections.Generic;

public class MsgSyncCharacter:MsgBase
{
    public MsgSyncCharacter() { protoName = "MsgSyncCharacter"; }
    public float x = 0f;
    public float y = 0f;
    public float z = 0f;    
    public float ey=0f;

    //服务端补充
    public string id = "";
}

public class MsgSyncGunRot:MsgBase
{
    public MsgSyncGunRot() { protoName = "MsgSyncGunRot"; }
    public float ex = 0f;
    public float ez = 0f;
    public float scaleY = 0f;
    //服务端补充
    public string id = "";
}


public class MsgFire : MsgBase
{ 
    public MsgFire() { protoName = "MsgFire"; }
    //炮弹初始位置 旋转
    public float x=0f;
    public float y=0f;
    public float z=0f;
    public float ex = 0f;
    public float ey = 0f;
    public float ez = 0f;


    //服务端补充
    public string id = "";//哪个坦克

}

public class MsgHit : MsgBase
{
    public MsgHit() { protoName = "MsgHit"; }
    //击中谁
    public string targetId = "";
    //击中点
    public float x = 0f;
    public float y = 0f;
    public float z = 0f;

    //服务端补充
    public string id = "";
    public int hp = 0;
    public int damage = 0;

}


