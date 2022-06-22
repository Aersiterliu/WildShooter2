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
    private void Start()
    {
        NetManager.AddEventListener(NetManager.NetEvent.Close, OnConnectClose);
        NetManager.AddMsgListener("MsgKick", OnMsgKick);

        //初始化
        PanelManager.Init();
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

  
}
