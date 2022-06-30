using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 选择一开始展示的面板
 *保存一些玩家id等数据 就不用从服务端拿了
 *GameMain从外部调用NetWorkManager的Update方法
 *完成一些通用事件处理 如网络断开 玩家被下线等
 */


public class GameMain : MonoBehaviour
{

    public static string id = "";
    public static string MapName = "";
    public static Transform myPlayer=null;
    private void Start()
    {

        NetManager.AddEventListener(NetManager.NetEvent.Close, OnConnectClose);
        NetManager.AddMsgListener("MsgKick", OnMsgKick);
        NetManager.AddMsgListener("MsgPickItem", OnMsgPickItem);

        //初始化
        PanelManager.Init();
        BattleManager.Init();
        PanelManager.Open<LoginPanel>();
       
    }

    private void Update()
    {
        NetManager.Update();
    }

    void OnConnectClose(string err)
    {
        Debug.Log("断开连接...");
    }

    void OnMsgKick(MsgBase msgBase)
    {
        PanelManager.Open<TipPanel>("被踢下线");
    }


    //这条是被服务器转发到房间里所有客户端的
    void OnMsgPickItem(MsgBase msgBase)
    {
        MsgPickItem msg = (MsgPickItem)msgBase;
        //在对应ID生成 不是在自己的ID生成哦！！！！

       

        if(BattleManager.GetPlayer(msg.id))
        {
            BasePlayer pickedplayer = BattleManager.GetPlayer(msg.id);
            GameObject pickeditemreal = GameObject.Instantiate(ResManager.LoadPrefab(msg.itemName + "real"));

            if (msg.id == GameMain.id)
            {
                pickeditemreal.AddComponent<CtrlGun>();
            }
            else
            {
                pickeditemreal.AddComponent<SyncGun>();
            }

            pickeditemreal.transform.parent = pickedplayer.GetComponentInChildren<Animator>().transform.Find("weaponHolder");

        }



        //这样销毁
        Map map=GameObject.Find("Map").GetComponentInChildren<Map>();
        for(int i=0;i<map.NetItems.Count;i++)
        {
            if (map.NetItems[i].name==msg.itemName)
            {
                Destroy(map.NetItems[i]);
                map.NetItems.Remove(map.NetItems[i]);
            }
        }
   
    }



}
