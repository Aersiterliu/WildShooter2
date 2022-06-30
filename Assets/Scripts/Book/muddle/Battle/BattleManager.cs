using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//为什么客户端有一个players列表，p363 索引战场里的角色
//GenerateCharacter的时候就会AddPlayer

//服务端里有一个Room[] 管理所以房间
//room里有一个playerIds字典 可以用于给房间里的用户转发
public class BattleManager:MonoBehaviour
{
    public static Dictionary<string,BasePlayer>players=new Dictionary<string,BasePlayer>();


  

    public static void Init()
    {
        NetManager.AddMsgListener("MsgEnterBattle", OnMsgEnterBattle);
        NetManager.AddMsgListener("MsgBattleResult", OnMsgBattleResult);
        NetManager.AddMsgListener("MsgLeaveBattle", OnMsgLeaveBattle);


        //
        NetManager.AddMsgListener("MsgSyncCharacter", OnMsgSyncCharacter);
        NetManager.AddMsgListener("MsgSyncGunRot", OnMsgSyncGunRot);
        NetManager.AddMsgListener("MsgFire", OnMsgFire);
        NetManager.AddMsgListener("MsgHit", OnMsgHit);
    }

    //添加玩家AddPlayer
    public static void AddPlayer(string id,BasePlayer player)
    {
        players[id] = player;
    }

    //删除玩家
    public static void RemovePlayer(string id)
    {
        players.Remove(id);
    }

    //获取坦克
    public static  BasePlayer GetPlayer(string id)
    { 
        //Players怎么查不到人？ 这段代码没错
        if(players.ContainsKey(id))
        {
            return players[id]; 
        }
        return null;
    }

    //获取玩家控制的角色
    public static BasePlayer GetCtrlPlayer()
    {
        return GetPlayer(GameMain.id);
    }

    //重置战场
    public static void Reset()
    {
        //场景
        foreach(BasePlayer player in players.Values)
        {
            MonoBehaviour.Destroy(player.gameObject);
        }
        players.Clear();
    }

    public static void OnMsgEnterBattle(MsgBase msgBase)
    {
        MsgEnterBattle msg = (MsgEnterBattle)msgBase;
        EnterBattle(msg);
    }

    //开始战斗
    public static void EnterBattle(MsgEnterBattle msg)
    {
        //重置
        BattleManager.Reset();
        //关闭界面
        PanelManager.Close("RoomPanel");//可以放到房间系统的监听中
        PanelManager.Close("LobbyPanelFix");
        PanelManager.Close("ResultPanel");
        //产生一个地图 后面可以写msg.mapName
        GameObject o = Instantiate(ResManager.LoadPrefab(msg.mapName));
        GameMain.MapName = msg.mapName;
        o.transform.SetParent(GameObject.Find("Map").transform);

        //产生玩家
        for (int i = 0; i < msg.charac.Length; i++)
        {
            GenerateCharac(msg.charac[i],i);
        }
    }

    public static void GenerateCharac(CharacterInfo characInfo,int index)
    {
        string objName = "Charac_" + characInfo.id;
        GameObject characObj = new GameObject(objName);
        //添加组件
        BasePlayer player = null;
        if (characInfo.id == GameMain.id)
        {
            player = characObj.AddComponent<CtrlPlayer>();
        }
        else
        {
            player = characObj.AddComponent<SyncPlayer>();
        }

        //摄像头 设置一些东西
        if (characInfo.id == GameMain.id)
        {
            CameraFollow cf = characObj.AddComponent<CameraFollow>();

            GameMain.myPlayer = player.transform;
        }

        //属性
        player.id = characInfo.id;
        player.hp = characInfo.hp;
        Vector2 pos = new Vector2(characInfo.x+index, characInfo.y+index);
        Vector3 rot=new Vector3(0, characInfo.ey, 0);
        player.transform.position = pos;
        player.transform.eulerAngles = rot;

       

        //初始化--选择人物 还没写 P368

        //在charac_xxx之下创建一个Ninjaa(Clone)
        player.Init("Ninjaa");
        AddPlayer(characInfo.id, player);

    }
    //收到战斗结束协议
    public static void OnMsgBattleResult(MsgBase msgBase)
    {
        MsgBattleResult msg = (MsgBattleResult)msgBase;

        bool isWin = false;

        //判断显示胜利还是失败
        BasePlayer player=GetCtrlPlayer();
        if(player!=null&&player.id==msg.winCharac)
        {
            isWin = true;
        }
        PanelManager.Open<ResultPanel>(isWin);
    }

    //收到玩家退出协议
    public static void OnMsgLeaveBattle(MsgBase msgBase)
    {
        MsgLeaveBattle msg=(MsgLeaveBattle)msgBase;
        //查找玩家
        BasePlayer player = GetPlayer(msg.id);
        if(player==null)
        {
            return;
        }
        //删除坦克
        RemovePlayer(msg.id);
        MonoBehaviour.Destroy(player.gameObject);
    }

    public static void OnMsgSyncCharacter(MsgBase msgBase)
    {
       
        MsgSyncCharacter msg=(MsgSyncCharacter)msgBase;
        //不同步自己
        if(msg.id==GameMain.id)
        {
            return;
        }
        //查找角色  为什么有id 搜不到玩家？
        SyncPlayer player = (SyncPlayer)GetPlayer(msg.id);
        if (player == null)
        {
            return ;
        }
        //移动同步
        player.SyncPos(msg);
    }

    public static void OnMsgSyncGunRot(MsgBase msgBase)
    {
        MsgSyncGunRot msg = (MsgSyncGunRot)msgBase;

        //不同步自己
        if(msg.id==GameMain.id)
        {
            return;
        }
        SyncPlayer player = (SyncPlayer)GetPlayer(msg.id);

        if (player == null)
        {
            return;
        }
        //移动同步
        player.SyncGunRot(msg);
    }

    //根据id找到同步角色，再调用它的SyncFire().
    public static void OnMsgFire(MsgBase msgBase)
    {
        MsgFire msg = (MsgFire)msgBase;
        //不同步自己
        if(msg.id==GameMain.id)
        {
            return;
        }
        //查找角色
        SyncPlayer player = (SyncPlayer)GetPlayer(msg.id);
        if(player == null)
        {
            return;
        }
        //开火
        player.GetComponentInChildren<SyncGun>().SyncFire(msg);
    }


    public static void OnMsgHit(MsgBase msgBase)
    {
        MsgHit msg = (MsgHit)msgBase;
        //查找角色
        BasePlayer player = GetPlayer(msg.targetId);
        if(player==null)
        {
            return;
        }
        //被击中
        player.Attacked(msg.damage);
    }

}
