using UnityEngine;

public class testPlayer : MonoBehaviour
{

    public void Start()
    {

        PanelManager.Init();
        PanelManager.Open<LoginPanel>();
        PanelManager.Open<TipPanel>("�û������������");

    }

}
