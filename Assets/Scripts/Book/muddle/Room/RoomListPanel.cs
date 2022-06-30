using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RoomListPanel : BasePanel
{

    private Button createButton;
    private Button reflashButton;
    private Button returnButton;
    private Transform content;
    private GameObject roomObj;

    public override void OnInit()
    {
        skinPath = "RoomListPanel";
        layer = PanelManager.Layer.Panel;
    }

    public override void OnShow(params object[] para)
    {
        createButton = skin.transform.Find("CreateButton").GetComponent<Button>();
        reflashButton = skin.transform.Find("ReflashButton").GetComponent<Button>();
        returnButton = skin.transform.Find("ReturnButton").GetComponent<Button>();
        content = skin.transform.Find("Scroll View/Viewport/Content");
        roomObj = skin.transform.Find("Room").gameObject;
        //按钮事件
        createButton.onClick.AddListener(OnCreateClick);
        reflashButton.onClick.AddListener(OnReflashClick);
        returnButton.onClick.AddListener(OnReturnClick);

        //不激活房间
        roomObj.SetActive(false);



        //协议监听
        NetManager.AddMsgListener("MsgGetRoomList", OnMsgGetRoomList);
        NetManager.AddMsgListener("MsgCreateRoom", OnMsgCreateRoom);
        NetManager.AddMsgListener("MsgEnterRoom", OnMsgEnterRoom);

        //发送查询
        MsgGetAchieve msgGetAchieve=new MsgGetAchieve();
        NetManager.Send(msgGetAchieve);
        MsgGetRoomList msgGetRoomList= new MsgGetRoomList();
        NetManager.Send(msgGetRoomList);

    }

    public override void OnClose()
    {
        NetManager.RemoveMsgListener("MsgGetRoomList", OnMsgGetRoomList);
        NetManager.RemoveMsgListener("MsgCreateRoom", OnMsgCreateRoom);
        NetManager.RemoveMsgListener("MsgEnterRoom", OnMsgEnterRoom);
    }

    public void OnMsgGetRoomList(MsgBase msgBase)
    {
        MsgGetRoomList msg = (MsgGetRoomList)msgBase;
        //清除房间列表
        for(int i=content.childCount-1;i>=0;i--)
        {
            GameObject o = content.GetChild(i).gameObject;
            Destroy(o);
        }
        //如果没有房间 不需要进一步处理
        if(msg.rooms==null)
        {
            return;
        }
        for(int i=0;i<msg.rooms.Length;i++)
        {
            GenerateRoom(msg.rooms[i]);
        }
    }

    public void GenerateRoom(RoomInfo roomInfo)
    {
        //创建物体
        GameObject o = Instantiate(roomObj);
        o.transform.SetParent(content);
        o.SetActive(true);
        o.transform.localScale=Vector3.one;

        //获取组件
        Transform trans=o.transform;
        TMP_Text idText = trans.Find("IdText").GetComponent<TMP_Text>();
        TMP_Text countText = trans.Find("CountText").GetComponent<TMP_Text>();
        TMP_Text statustText = trans.Find("StatusText").GetComponent<TMP_Text>();
        Button btn = trans.Find("JoinButton").GetComponent<Button>();

        //填充信息
        idText.text = roomInfo.id.ToString();
        countText.text = roomInfo.count.ToString();
        if(roomInfo.status==0)
        {
            statustText.text = "准备中";
        }
        else
        {
            statustText.text = "战斗中";
        }

        //按钮事件?????????????????????????????????????不是很懂
        btn.name=idText.text;
        btn.onClick.AddListener(delegate ()
        {
            OnJoinClick(btn.name);
        });

    }

    //点击加入房间按钮
    public void OnJoinClick(string idString)
    {
        MsgEnterRoom msg = new MsgEnterRoom();
        msg.id=int.Parse(idString);
        NetManager.Send(msg);
    }

    //收到有人加入房间协议
    public void OnMsgEnterRoom(MsgBase msgBase)
    {
        MsgEnterRoom msg = (MsgEnterRoom)msgBase;
        //成功进入房间
        if(msg.result==0)
        {
            PanelManager.Open<RoomPanel>();
            Close();
        }
        else
        {
            PanelManager.Open<TipPanel>("进入房间失败！");
        }
    }

    public void OnCreateClick()
    {
        MsgCreateRoom msg = new MsgCreateRoom();
        NetManager.Send(msg);
    }

    public void OnMsgCreateRoom(MsgBase msgBase)
    {
        MsgCreateRoom msg = (MsgCreateRoom)msgBase;
        //成功创建房间
        if(msg.result == 0)
        {
            PanelManager.Open<TipPanel>("创建成功！");
            PanelManager.Open<RoomPanel>();
            Close();
        }
        else
        {
            PanelManager.Open<TipPanel>("进入房间失败");
        }
    }

    public void OnReflashClick()
    {
        MsgGetRoomList msg = new MsgGetRoomList();
        NetManager.Send(msg);
    }

    public void OnReturnClick()
    {
        PanelManager.Close("RoomListPanel");
    }

 


}
