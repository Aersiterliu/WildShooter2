using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LobbyPanelFix : BasePanel
{
    private TMP_Text scoreText;
    //�򿪷����б�ť
    private Button openButton;

    public override void OnInit()
    {
        skinPath = "LobbyPanelFix";
        layer = PanelManager.Layer.Panel;
    }
    public override void OnShow(params object[] args)
    {
        scoreText = skin.transform.Find("InfoPanel/ScoreText").GetComponent<TMP_Text>();
        openButton = skin.transform.Find("openButton").GetComponent<Button>();
        NetManager.AddMsgListener("MsgGetAchieve", OnMsgGetAchieve);
        openButton.onClick.AddListener(OnOpenClick);
    }

    public override void OnClose()
    {
        NetManager.RemoveMsgListener("MsgGetAchieve", OnMsgGetAchieve);
    }

    public void OnMsgGetAchieve(MsgBase msgBase)
    {
        MsgGetAchieve msg = (MsgGetAchieve)msgBase;
        scoreText.text = msg.win + "ʤ" + msg.lost + "��";
    }

    public void OnOpenClick()
    {
        PanelManager.Open<RoomListPanel>();
    }
}
