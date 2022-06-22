using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class LoginPanel : BasePanel
{
    private TMP_InputField idInput;
    private TMP_InputField pwInput;
    private Button loginButton;
    private Button regBtn;

    public override void OnInit()
    {
        skinPath = "LoginPanel";
        layer = PanelManager.Layer.Panel;
    }

    public override void OnShow(params object[] para)
    {
        idInput = skin.transform.Find("IdInput").GetComponent<TMP_InputField>();
        pwInput = skin.transform.Find("PwInput").GetComponent<TMP_InputField>();
        loginButton = skin.transform.Find("LoginBtn").GetComponent<Button>();
        regBtn = skin.transform.Find("RegisterBtn").GetComponent<Button>();

        loginButton.onClick.AddListener(OnLoginClick);
        regBtn.onClick.AddListener(OnRegClick);

        //网络协议监听
        NetManager.AddMsgListener("MsgLogin", OnMsgLogin);
        //网络事件监听
        NetManager.AddEventListener(NetManager.NetEvent.ConnectSucc, OnConnectSucc);
        NetManager.AddEventListener(NetManager.NetEvent.ConnectFail, OnConnectFail);
        //Link Start!
        NetManager.Connect("127.0.0.1", 8888);
    }

    public override void OnClose()
    {
        
    }

    void OnConnectSucc(string err)
    {
        Debug.Log("OnConnnectSucc");
    }

    void OnConnectFail(string err)
    {
        PanelManager.Open<TipPanel>(err);
    }

    public void OnLoginClick()
    {
        if(idInput.text==""||pwInput.text=="")
        {
            PanelManager.Open<TipPanel>("用户名和密码不能为空");
            return;
        }
        //发送
        MsgLogin msgLogin=new MsgLogin();
        msgLogin.id=idInput.text;
        msgLogin.pw=pwInput.text;
        NetManager.Send(msgLogin);
    }

    public void OnRegClick()
    {
        PanelManager.Open<RegisterPanel>();
    }

    public void OnMsgLogin(MsgBase msgBase)
    {
        MsgLogin msg = (MsgLogin)msgBase;
        if(msg.result==0)
        {
            Debug.Log("登陆成功！");
            GameObject playerObj = new GameObject("myPlayer");
     

            //!!!!!!!!!!!!!!!!!!!!需要后期更改！！1
            BasePlayer basePlayer = playerObj.AddComponent<BasePlayer>();
            basePlayer.Init("Ninja");
            playerObj.transform.GetChild(0).gameObject.AddComponent<CameraFollow>();

            GameMain.id = msg.id;
            Close();
        }
        else
        {
            PanelManager.Open<TipPanel>("登陆失败！");
        }
    }
}
