using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RegisterPanel : BasePanel
{
    private TMP_InputField idInput;
    private TMP_InputField pwInput;
    private TMP_InputField repInput;
    private Button regBtn;
    private Button closeBtn;

    public override void OnInit()
    {
        skinPath = "RegisterPanel";
        layer = PanelManager.Layer.Panel;
    }

    public override void OnShow(params object[] para)
    {
        idInput = skin.transform.Find("IdInput").GetComponent<TMP_InputField>();
        pwInput = skin.transform.Find("PwInput").GetComponent<TMP_InputField>();
        repInput = skin.transform.Find("RepInput").GetComponent<TMP_InputField>();
        regBtn= skin.transform.Find("RegisterBtn").GetComponent<Button>();
        closeBtn = skin.transform.Find("CloseBtn").GetComponent<Button>();

        //����
        regBtn.onClick.AddListener(OnRegClick);
        closeBtn.onClick.AddListener(OnCloseClick);

        //����Э�����
        NetManager.AddMsgListener("MsgRegister", OnMsgRegister);
    }

    public override void OnClose()
    {
        NetManager.RemoveMsgListener("MsgRegister", OnMsgRegister);
    }

    public void OnRegClick()
    {
        if(idInput.text==""||pwInput.text=="")
        {
            PanelManager.Open<TipPanel>("�û��������벻��Ϊ��");
            return;
        }
        //�������벻ͬ
        if(repInput.text!=pwInput.text)
        {
            PanelManager.Open<TipPanel>("������������벻ͬ");
            return;
        }
        //����
        MsgRegister msgReg=new MsgRegister();
        msgReg.id=idInput.text;
        msgReg.pw=pwInput.text;
        NetManager.Send(msgReg);
    }

    public void OnCloseClick()
    {
        Close();
    }

    public void OnMsgRegister(MsgBase msgBase)
    {
        MsgRegister msg=(MsgRegister)msgBase;
        if(msg.result==0)
        {
            Debug.Log("ע��ɹ�");
            PanelManager.Open<TipPanel>("ע��ɹ�");
            Close();
        }
        else
        {
            PanelManager.Open<TipPanel>("ע��ʧ��");
        }
    }
}
