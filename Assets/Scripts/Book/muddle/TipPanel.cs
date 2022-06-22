using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TipPanel : BasePanel
{
    private TMP_Text text;
    private Button okBtn;

    public override void OnInit()
    {
        skinPath = "TipPanel";
        layer = PanelManager.Layer.Tip;
    }

    public override void OnShow(params object[] args)
    {
        text = skin.transform.Find("Text").GetComponent<TMP_Text>();
        okBtn = skin.transform.Find("OkBtn").GetComponent<Button>();
        //¼àÌý
        okBtn.onClick.AddListener(OnOkClick);
        if(args.Length==1)
        {
            text.text = (string)args[0];
        }
    }

    public override void OnClose()
    {
        
    }

    public void OnOkClick()
    {
        Close();
    }
}

