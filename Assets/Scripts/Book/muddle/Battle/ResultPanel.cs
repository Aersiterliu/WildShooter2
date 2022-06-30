using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResultPanel : BasePanel
{

    //ȷ����ť
    private Button okBtn;
    public override void OnInit()
    {
        skinPath = "ResultPanel";
        layer = PanelManager.Layer.Tip;
    }

    //��ʾ
    public override void OnShow(params object[] para)
    {
        //Ѱ�����
        okBtn = skin.transform.Find("OkBtn").GetComponent<Button>();

        //����
        okBtn.onClick.AddListener(OnOkClick);

    }

    public override void OnClose()
    {
     
    }

    public void OnOkClick()
    {
        PanelManager.Open<RoomPanel>();
        Close();
    }
}
