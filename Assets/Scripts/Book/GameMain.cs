using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * ѡ��һ��ʼչʾ�����
 *����һЩ���id������ �Ͳ��ôӷ��������
 *GameMain���ⲿ����NetWorkManager��Update����
 *���һЩͨ���¼����� ������Ͽ� ��ұ����ߵ�
 */


public class GameMain : MonoBehaviour
{

    public static string id = "";
    private void Start()
    {
        NetManager.AddEventListener(NetManager.NetEvent.Close, OnConnectClose);
        NetManager.AddMsgListener("MsgKick", OnMsgKick);

        //��ʼ��
        PanelManager.Init();
        PanelManager.Open<LoginPanel>();
    }

    private void Update()
    {
        NetManager.Update();
    }

    void OnConnectClose(string err)
    {
        Debug.Log("�Ͽ�����...");
    }

    void OnMsgKick(MsgBase msgBase)
    {
        PanelManager.Open<TipPanel>("��������");
    }

  
}
