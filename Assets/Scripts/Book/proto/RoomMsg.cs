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
    //����˻�
    public RoomInfo[] rooms;
}

public class MsgCreateRoom:MsgBase
{
    public MsgCreateRoom() { protoName = "MsgCreateRoom"; }
    //����˻�
    public int result = 0;
}

public class MsgEnterRoom:MsgBase
{
    public MsgEnterRoom() { protoName = "MsgEnterRoom"; }
    //�ͻ��˷�
    public int id = 0;
    //����˻�
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
    //����˻�
    public PlayerInfo[] players;
}

public class MsgLeaveRoom:MsgBase
{
    public MsgLeaveRoom() { protoName = "MsgLeaveRoom"; }
    //����˻�
    public int result = 0;
}


public class MsgStartBattle:MsgBase
{
    public MsgStartBattle() { protoName = "MsgStartBattle"; }
    //����˻�
    public int result = 0;

}
