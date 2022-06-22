using UnityEngine;

public class testPlayer : MonoBehaviour
{

    public void Start()
    {

        PanelManager.Init();
        PanelManager.Open<LoginPanel>();
        PanelManager.Open<TipPanel>("用户名或密码错误！");

    }

}
