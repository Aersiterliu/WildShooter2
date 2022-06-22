using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePlayer : MonoBehaviour
{
    private GameObject skin;

    public float hp = 100;
    public float speed = 5.0f;

    public Vector2 input;
    public Vector2 mousePos;
    public Animator animator;
    new public Rigidbody2D rigidbody;

    public void Start()
    {

    }
    public virtual void Init(string skinPath)
    {
        GameObject skinRes=ResManager.LoadPrefab(skinPath);
        skin=(GameObject)Instantiate(skinRes);
        skin.transform.parent = transform;
        skin.transform.localPosition = Vector3.zero;
        skin.transform.localEulerAngles = Vector3.zero;

        //animator = gameObject.AddComponent<Animator>();

        
        //rigidbody = gameObject.AddComponent<Rigidbody2D>();
        //rigidbody.bodyType = RigidbodyType2D.Kinematic;
        //rigidbody.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        //rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
    }
    public void Update()
    {

    }

    public bool IsDie()
    {
        return hp <=0;
    }
}
