using System;
using System.Collections;
using UnityEngine;

//RoomMsg里有一个PlayerInfo 是表明玩家的战绩信息啥的 
[System.Serializable]
public class CharacterInfo
{
    public string id = "";
    public int hp = 0;
    public float x = 0;
    public float y = 0;
    public float ey = 0;
}

//进入战场 ，服务端推送
public class MsgEnterBattle:MsgBase
{
    public MsgEnterBattle() { protoName = "MsgEnterBattle"; }
    //服务端回
    public CharacterInfo[] charac;
    public string mapName = ""; //地图 目前只有一张

}


public class MsgBattleResult : MsgBase
{
    public MsgBattleResult() { protoName = "MsgBattleResult"; }
    //服务端回
    public string winCharac = "";//获胜玩家
}

public class MsgLeaveBattle:MsgBase
{ 
    public MsgLeaveBattle() { protoName = "MsgLeaveBattle"; }
    //服务端回
    public string id = "";//玩家id

}
