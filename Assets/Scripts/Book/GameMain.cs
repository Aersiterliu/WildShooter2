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
    public static string MapName = "";
    public static Transform myPlayer=null;
    private void Start()
    {

        NetManager.AddEventListener(NetManager.NetEvent.Close, OnConnectClose);
        NetManager.AddMsgListener("MsgKick", OnMsgKick);
        NetManager.AddMsgListener("MsgPickItem", OnMsgPickItem);

        //��ʼ��
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
        Debug.Log("�Ͽ�����...");
    }

    void OnMsgKick(MsgBase msgBase)
    {
        PanelManager.Open<TipPanel>("��������");
    }


    //�����Ǳ�������ת�������������пͻ��˵�
    void OnMsgPickItem(MsgBase msgBase)
    {
        MsgPickItem msg = (MsgPickItem)msgBase;
        //�ڶ�ӦID���� �������Լ���ID����Ŷ��������

       

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



        //��������
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
