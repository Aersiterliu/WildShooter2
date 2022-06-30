using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RoomPanel : BasePanel
{

    private Button startButton;
    private Button closeButton;
    private Transform content;
    private GameObject playerObj;
    public override void OnInit()
    {
        skinPath = "RoomPanel";
        layer = PanelManager.Layer.Panel;
    }

    public override void OnShow(params object[] args)
    {
        startButton=skin.transform.Find("CtrlPanel/StartButton").GetComponent<Button>();
        closeButton = skin.transform.Find("CtrlPanel/CloseButton").GetComponent<Button>();
        content = skin.transform.Find("ListPanel/Scroll View/Viewport/Content");
        playerObj = skin.transform.Find("Player").gameObject;
        //不激活玩家信息

        playerObj.SetActive(false);
        //按钮事件
        startButton.onClick.AddListener(OnStartClick);
        closeButton.onClick.AddListener(OnCloseClick);

        //协议监听
        NetManager.AddMsgListener("MsgGetRoomInfo", OnMsgGetRoomInfo);
        NetManager.AddMsgListener("MsgLeaveRoom", OnMsgLeaveRoom);
        NetManager.AddMsgListener("MsgStartBattle", OnMsgStartBattle);

        //发送查询
        Debug.Log("发送MsgGetRoomInfo");
        MsgGetRoomInfo msg = new MsgGetRoomInfo();
        NetManager.Send(msg);


    }

    public override void OnClose()
    {
        NetManager.RemoveMsgListener("MsgGetRoomInfo", OnMsgGetRoomInfo);
        NetManager.RemoveMsgListener("MsgLeaveRoom", OnMsgLeaveRoom);
        NetManager.RemoveMsgListener("MsgStartBattle", OnMsgStartBattle);
    }



    public void OnMsgGetRoomInfo(MsgBase msgBase)
    {
        MsgGetRoomInfo msg=(MsgGetRoomInfo)msgBase;
        //清除玩家列表
        for(int i=content.childCount-1;i>=0;i--)
        {
            GameObject o = content.GetChild(i).gameObject;
            Destroy(o);
        }
        //重新生成列表
        if(msg.players==null)
        {
            return;
        }
        for(int i=0;i<msg.players.Length; i++)
        {
            GeneratePlayerInfo(msg.players[i]);
        }
    }

    public void GeneratePlayerInfo(PlayerInfo playerInfo)
    {
        GameObject o=Instantiate(playerObj);
        o.transform.SetParent(content);
        o.SetActive(true);
        o.transform.localScale = Vector3.one;

        //获取组件
        Transform trans = o.transform;
        TMP_Text idText = trans.Find("IdText").GetComponent<TMP_Text>();
        TMP_Text scoreText = trans.Find("ScoreText").GetComponent<TMP_Text>();

        //填充信息
        idText.text = playerInfo.id;
        if(playerInfo.isOwner==1)
        {
            idText.text = idText.text + "!";
        }
        scoreText.text = playerInfo.win + "胜" + playerInfo.lost + "负";

    }

    public void OnCloseClick()
    {
        MsgLeaveRoom msg = new MsgLeaveRoom();
        NetManager.Send(msg);
    }

    public void OnMsgLeaveRoom(MsgBase msgBase)
    {
        MsgLeaveRoom msg = (MsgLeaveRoom)msgBase;
        //成功退出房间
        if(msg.result==0)
        {
            PanelManager.Open<TipPanel>("退出房间");
            PanelManager.Open<RoomListPanel>();
            Close();               
        }
        else
        {
            PanelManager.Open<TipPanel>("退出房间失败");
        }
    }

    public void OnStartClick()
    {
        MsgStartBattle msg = new MsgStartBattle();
        NetManager.Send(msg);
    }

    public void OnMsgStartBattle(MsgBase msgBase)
    {
        MsgStartBattle msg = (MsgStartBattle)msgBase;
        //开战
        if(msg.result==0)
        {
            Close();
        }
        else
        {
            PanelManager.Open<TipPanel>("开战失败，两边至少需要一名玩家，只有房主可以开始!");
        }
    }

}
