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
        //��ť�¼�
        createButton.onClick.AddListener(OnCreateClick);
        reflashButton.onClick.AddListener(OnReflashClick);
        returnButton.onClick.AddListener(OnReturnClick);

        //�������
        roomObj.SetActive(false);



        //Э�����
        NetManager.AddMsgListener("MsgGetRoomList", OnMsgGetRoomList);
        NetManager.AddMsgListener("MsgCreateRoom", OnMsgCreateRoom);
        NetManager.AddMsgListener("MsgEnterRoom", OnMsgEnterRoom);

        //���Ͳ�ѯ
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
        //��������б�
        for(int i=content.childCount-1;i>=0;i--)
        {
            GameObject o = content.GetChild(i).gameObject;
            Destroy(o);
        }
        //���û�з��� ����Ҫ��һ������
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
        //��������
        GameObject o = Instantiate(roomObj);
        o.transform.SetParent(content);
        o.SetActive(true);
        o.transform.localScale=Vector3.one;

        //��ȡ���
        Transform trans=o.transform;
        TMP_Text idText = trans.Find("IdText").GetComponent<TMP_Text>();
        TMP_Text countText = trans.Find("CountText").GetComponent<TMP_Text>();
        TMP_Text statustText = trans.Find("StatusText").GetComponent<TMP_Text>();
        Button btn = trans.Find("JoinButton").GetComponent<Button>();

        //�����Ϣ
        idText.text = roomInfo.id.ToString();
        countText.text = roomInfo.count.ToString();
        if(roomInfo.status==0)
        {
            statustText.text = "׼����";
        }
        else
        {
            statustText.text = "ս����";
        }

        //��ť�¼�?????????????????????????????????????���Ǻܶ�
        btn.name=idText.text;
        btn.onClick.AddListener(delegate ()
        {
            OnJoinClick(btn.name);
        });

    }

    //������뷿�䰴ť
    public void OnJoinClick(string idString)
    {
        MsgEnterRoom msg = new MsgEnterRoom();
        msg.id=int.Parse(idString);
        NetManager.Send(msg);
    }

    //�յ����˼��뷿��Э��
    public void OnMsgEnterRoom(MsgBase msgBase)
    {
        MsgEnterRoom msg = (MsgEnterRoom)msgBase;
        //�ɹ����뷿��
        if(msg.result==0)
        {
            PanelManager.Open<RoomPanel>();
            Close();
        }
        else
        {
            PanelManager.Open<TipPanel>("���뷿��ʧ�ܣ�");
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
        //�ɹ���������
        if(msg.result == 0)
        {
            PanelManager.Open<TipPanel>("�����ɹ���");
            PanelManager.Open<RoomPanel>();
            Close();
        }
        else
        {
            PanelManager.Open<TipPanel>("���뷿��ʧ��");
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
