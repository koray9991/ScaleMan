using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ai : MonoBehaviour
{
    Rigidbody rb;
    public float scale;
    public Animator anim;
    public float moveSpeed;
    public float scaleSpeed;
    float targetScale;
    bool changeScale;
    float changeScaleTimer;
    int choice;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        targetScale = 1;
        scale = 1;
        
        changeScale = true;
    }

    private void FixedUpdate()
    {
        anim.SetFloat("movement", (1 / scale));
    }
    void Update()
    {
        Movement();
        if (changeScale)
        {
            changeScaleTimer += Time.deltaTime;
           
            if (changeScaleTimer > 2)
            {
                choice = Random.Range(0, 2);
                if (choice == 0)//smaller
                {
                    targetScale = Random.Range(0.1f, 0.4f);
                }
                if (choice == 1)//bigger
                {
                    targetScale = Random.Range(1.5f, 3.9f);
                }
                changeScaleTimer = 0;
            }
        }
    }
    private void Movement()
    {
        rb.velocity = new Vector3(0, 0, moveSpeed);

        
        rb.mass = scale * scale * scale;
        if (scale > targetScale)
        {
            scale -= scaleSpeed;
        }
        if (scale < targetScale)
        {
            scale += scaleSpeed;
        }
        transform.localScale = new Vector3(scale, scale, scale);

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Finish")
        {
            anim.SetInteger("choice", 1);
            moveSpeed = 0;
            changeScale = false;
        }
    }
}
