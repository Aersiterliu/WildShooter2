using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResultPanel : BasePanel
{

    //确定按钮
    private Button okBtn;
    public override void OnInit()
    {
        skinPath = "ResultPanel";
        layer = PanelManager.Layer.Tip;
    }

    //显示
    public override void OnShow(params object[] para)
    {
        //寻找组件
        okBtn = skin.transform.Find("OkBtn").GetComponent<Button>();

        //监听
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
