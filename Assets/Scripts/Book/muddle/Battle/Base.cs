using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    public GameObject skin;

    protected Rigidbody2D rigidbody2d;
    public AnimationClip[] weaponAnimationClip;
    protected Animator animator;
    protected AnimatorOverrideController animatorOverrideController;

    public float speed = 10.0f;

    public void Start()
    {
        
    }

    public virtual void Init(string skinPath)
    {

        //∆§∑Ù
        GameObject skinRes = ResManager.LoadPrefab(skinPath);
        skin = (GameObject)Instantiate(skinRes);
        skin.transform.parent = transform;
        skin.transform.localPosition = Vector3.zero;
        skin.transform.localEulerAngles = Vector3.zero;

        //ŒÔ¿Ì
        animator = skin.transform.GetComponent<Animator>();
        
        rigidbody2d = skin.transform.GetComponent<Rigidbody2D>();
        BoxCollider2D boxCollider = gameObject.AddComponent<BoxCollider2D>();
    }

    public void Update()
    {
        
    }
}
