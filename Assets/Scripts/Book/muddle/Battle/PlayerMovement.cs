using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletHell
{
    public class PlayerMovement : MonoBehaviour
    {
        public float speed;
        private Vector2 input;
        private Vector2 mousePos;
        private Animator animator;
        new private Rigidbody2D rigidbody;
        void Start()
        {
            animator = GetComponent<Animator>();
            rigidbody = GetComponent<Rigidbody2D>();
        }

        void Update()
        {
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            rigidbody.velocity = input.normalized * speed;
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (mousePos.x > transform.position.x)
            {
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
            }
            else
            {
                transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
            }

            if (input != Vector2.zero)
                animator.SetBool("isMoving", true);
            else
                animator.SetBool("isMoving", false);
        }


    }
}