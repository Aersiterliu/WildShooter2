using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Ϊʲô�ͻ�����һ��players�б�p363 ����ս����Ľ�ɫ
//GenerateCharacter��ʱ��ͻ�AddPlayer

//���������һ��Room[] �������Է���
//room����һ��playerIds�ֵ� �������ڸ���������û�ת��
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

    //������AddPlayer
    public static void AddPlayer(string id,BasePlayer player)
    {
        players[id] = player;
    }

    //ɾ�����
    public static void RemovePlayer(string id)
    {
        players.Remove(id);
    }

    //��ȡ̹��
    public static  BasePlayer GetPlayer(string id)
    { 
        //Players��ô�鲻���ˣ� ��δ���û��
        if(players.ContainsKey(id))
        {
            return players[id]; 
        }
        return null;
    }

    //��ȡ��ҿ��ƵĽ�ɫ
    public static BasePlayer GetCtrlPlayer()
    {
        return GetPlayer(GameMain.id);
    }

    //����ս��
    public static void Reset()
    {
        //����
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

    //��ʼս��
    public static void EnterBattle(MsgEnterBattle msg)
    {
        //����
        BattleManager.Reset();
        //�رս���
        PanelManager.Close("RoomPanel");//���Էŵ�����ϵͳ�ļ�����
        PanelManager.Close("LobbyPanelFix");
        PanelManager.Close("ResultPanel");
        //����һ����ͼ �������дmsg.mapName
        GameObject o = Instantiate(ResManager.LoadPrefab(msg.mapName));
        GameMain.MapName = msg.mapName;
        o.transform.SetParent(GameObject.Find("Map").transform);

        //�������
        for (int i = 0; i < msg.charac.Length; i++)
        {
            GenerateCharac(msg.charac[i],i);
        }
    }

    public static void GenerateCharac(CharacterInfo characInfo,int index)
    {
        string objName = "Charac_" + characInfo.id;
        GameObject characObj = new GameObject(objName);
        //������
        BasePlayer player = null;
        if (characInfo.id == GameMain.id)
        {
            player = characObj.AddComponent<CtrlPlayer>();
        }
        else
        {
            player = characObj.AddComponent<SyncPlayer>();
        }

        //����ͷ ����һЩ����
        if (characInfo.id == GameMain.id)
        {
            CameraFollow cf = characObj.AddComponent<CameraFollow>();

            GameMain.myPlayer = player.transform;
        }

        //����
        player.id = characInfo.id;
        player.hp = characInfo.hp;
        Vector2 pos = new Vector2(characInfo.x+index, characInfo.y+index);
        Vector3 rot=new Vector3(0, characInfo.ey, 0);
        player.transform.position = pos;
        player.transform.eulerAngles = rot;

       

        //��ʼ��--ѡ������ ��ûд P368

        //��charac_xxx֮�´���һ��Ninjaa(Clone)
        player.Init("Ninjaa");
        AddPlayer(characInfo.id, player);

    }
    //�յ�ս������Э��
    public static void OnMsgBattleResult(MsgBase msgBase)
    {
        MsgBattleResult msg = (MsgBattleResult)msgBase;

        bool isWin = false;

        //�ж���ʾʤ������ʧ��
        BasePlayer player=GetCtrlPlayer();
        if(player!=null&&player.id==msg.winCharac)
        {
            isWin = true;
        }
        PanelManager.Open<ResultPanel>(isWin);
    }

    //�յ�����˳�Э��
    public static void OnMsgLeaveBattle(MsgBase msgBase)
    {
        MsgLeaveBattle msg=(MsgLeaveBattle)msgBase;
        //�������
        BasePlayer player = GetPlayer(msg.id);
        if(player==null)
        {
            return;
        }
        //ɾ��̹��
        RemovePlayer(msg.id);
        MonoBehaviour.Destroy(player.gameObject);
    }

    public static void OnMsgSyncCharacter(MsgBase msgBase)
    {
       
        MsgSyncCharacter msg=(MsgSyncCharacter)msgBase;
        //��ͬ���Լ�
        if(msg.id==GameMain.id)
        {
            return;
        }
        //���ҽ�ɫ  Ϊʲô��id �Ѳ�����ң�
        SyncPlayer player = (SyncPlayer)GetPlayer(msg.id);
        if (player == null)
        {
            return ;
        }
        //�ƶ�ͬ��
        player.SyncPos(msg);
    }

    public static void OnMsgSyncGunRot(MsgBase msgBase)
    {
        MsgSyncGunRot msg = (MsgSyncGunRot)msgBase;

        //��ͬ���Լ�
        if(msg.id==GameMain.id)
        {
            return;
        }
        SyncPlayer player = (SyncPlayer)GetPlayer(msg.id);

        if (player == null)
        {
            return;
        }
        //�ƶ�ͬ��
        player.SyncGunRot(msg);
    }

    //����id�ҵ�ͬ����ɫ���ٵ�������SyncFire().
    public static void OnMsgFire(MsgBase msgBase)
    {
        MsgFire msg = (MsgFire)msgBase;
        //��ͬ���Լ�
        if(msg.id==GameMain.id)
        {
            return;
        }
        //���ҽ�ɫ
        SyncPlayer player = (SyncPlayer)GetPlayer(msg.id);
        if(player == null)
        {
            return;
        }
        //����
        player.GetComponentInChildren<SyncGun>().SyncFire(msg);
    }


    public static void OnMsgHit(MsgBase msgBase)
    {
        MsgHit msg = (MsgHit)msgBase;
        //���ҽ�ɫ
        BasePlayer player = GetPlayer(msg.targetId);
        if(player==null)
        {
            return;
        }
        //������
        player.Attacked(msg.damage);
    }

}
