using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePanel : MonoBehaviour
{
    public string skinPath;
    public GameObject skin;

    public PanelManager.Layer layer = PanelManager.Layer.Panel;

    public void Init()
    {
        GameObject skinPrefab=ResManager.LoadPrefab(skinPath);
        skin=(GameObject)Instantiate(skinPrefab);
    }

    public void Close()
    {
        string name = this.GetType().ToString();
        PanelManager.Close(name);
    }

    public virtual void OnInit()
    {

    }

    public virtual void OnShow(params object[] para)
    {

    }

    public virtual void OnClose()
    {

    }
}
