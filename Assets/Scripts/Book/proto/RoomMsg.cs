using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MsgGetAchieve:MsgBase
{
    public MsgGetAchieve() { protoName = "MsgGetAchieve"; }

    public int win = 0;
    public int lost = 0;
}


[System.Serializable]
public class RoomInfo
{
    public int id = 0;
    public int count = 0;
    public int status = 0;
}

public class MsgGetRoomList:MsgBase
{
    public MsgGetRoomList() { protoName = "MsgGetRoomList"; }
    //服务端回
    public RoomInfo[] rooms;
}

public class MsgCreateRoom:MsgBase
{
    public MsgCreateRoom() { protoName = "MsgCreateRoom"; }
    //服务端回
    public int result = 0;
}

public class MsgEnterRoom:MsgBase
{
    public MsgEnterRoom() { protoName = "MsgEnterRoom"; }
    //客户端发
    public int id = 0;
    //服务端回
    public int result = 0;
}


[System.Serializable]
public class PlayerInfo
{
    public string id = "psy";
    public int camp = 0;
    public int win = 0;
    public int lost = 0;
    public int isOwner = 0;
}

public class MsgGetRoomInfo:MsgBase
{
    public MsgGetRoomInfo() { protoName = "MsgGetRoomInfo"; }
    //服务端回
    public PlayerInfo[] players;
}

public class MsgLeaveRoom:MsgBase
{
    public MsgLeaveRoom() { protoName = "MsgLeaveRoom"; }
    //服务端回
    public int result = 0;
}


public class MsgStartBattle:MsgBase
{
    public MsgStartBattle() { protoName = "MsgStartBattle"; }
    //服务端回
    public int result = 0;

}
