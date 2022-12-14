using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class Player : MonoBehaviour
{


    public float moveSpeed;
    public float scaleSpeed;
    public float scale;
    public float minScale, maxScale;
    public Animator anim;
    public CinemachineVirtualCamera vCam;
    CinemachineTransposer transposer;
     float scaleDelay;
    public float camSpeed;
    Rigidbody rb;
    public GameObject moveImage;
    public float animationSpeed;
    public float camIndexX,camIndexY,camIndexZ;
    bool onBridge;
    public bool fail; 
    public float gravityScale = 3;
    float bridgeStopTimer;
    bool bridgeDestroy;

    public static float globalGravity = -9.81f;

   

   

  

    private void Awake()
    {
     
    }
    void Start()
    {
        transposer = vCam.GetCinemachineComponent<CinemachineTransposer>();
        rb = GetComponent<Rigidbody>();
        scale = 1;
      //  scale = (minScale + maxScale) / 2;
        scaleDelay = scale;
       
    }

    
    void Update()
    {
        Movement();
       
      
       
    }
    private void FixedUpdate()
    {
        Vector3 gravity = globalGravity * gravityScale * Vector3.up;
        rb.AddForce(gravity, ForceMode.Acceleration);
        if (scale <= minScale)
        {
            scale = minScale;
        }
        if (scale > maxScale)
        {
            scale = maxScale;
        }
        transform.localScale = new Vector3(scale, scale, scale);
        if (scale > scaleDelay)
        {
            scaleDelay += Time.deltaTime * camSpeed;
        }
        if (scale < scaleDelay)
        {
            scaleDelay -= Time.deltaTime * camSpeed;
        }

        if (onBridge && !fail)
        {
            if (scale < 0.5f)
            {
                moveSpeed = 4;
                anim.SetInteger("choice", 0);
                bridgeStopTimer =0;
            }
            else
            {
                moveSpeed = 0;
                anim.SetInteger("choice", 2);
                bridgeStopTimer += Time.deltaTime;
                if (bridgeStopTimer > 2)
                {
                    bridgeDestroy = true;
                }
            }
            
        }
        anim.SetFloat("movement", (1 / scale));
        moveImage.GetComponent<RectTransform>().localPosition = new Vector3(scale - 1, moveImage.GetComponent<RectTransform>().localPosition.y, moveImage.GetComponent<RectTransform>().localPosition.z);
    }
    private void Movement()
    {
        rb.velocity = new Vector3(0, 0, moveSpeed);
       
        transposer.m_FollowOffset = new Vector3(scaleDelay* camIndexX, scaleDelay * camIndexY, scaleDelay * camIndexZ);
        rb.mass = scale * scale * scale;
        if (Input.GetMouseButton(0))
        {
            scale += Input.GetAxis("Mouse X") * scaleSpeed;
           
           

        }
       
       

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Finish")
        {
            anim.SetInteger("choice", 1);
            moveSpeed = 0;
            GameManager.instance.canvas.SetActive(true);
        }
        if (other.tag == "Bridge")
        {
            onBridge = true;

        }
        if (other.tag == "Respawn")
        {
            fail = true;
            moveSpeed = 0;
            anim.SetInteger("choice", 3);
            vCam.gameObject.SetActive(false);
        }

    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Wall" && scale > 1)
        {
            collision.gameObject.GetComponent<BoxCollider>().enabled = false;
            for (int i = collision.gameObject.transform.childCount - 1; i >= 0; i--)
            {
                collision.gameObject.transform.GetChild(i).gameObject.AddComponent<Rigidbody>();
                collision.gameObject.transform.GetChild(i).gameObject.AddComponent<BoxCollider>();
                collision.gameObject.transform.GetChild(i).GetComponent<Rigidbody>().AddForce(Random.Range(-500, 500), Random.Range(200, 500), Random.Range(0, 1));
                collision.gameObject.transform.GetChild(i).transform.parent = null;
            }
        }
        if (collision.gameObject.tag == "Window" && scale > 1)
        {
            collision.gameObject.GetComponent<BoxCollider>().enabled = false;
            collision.gameObject.GetComponent<MeshRenderer>().enabled = false;
            for (int i = collision.gameObject.transform.childCount - 1; i >= 0; i--)
            {
                collision.gameObject.transform.GetChild(i).gameObject.AddComponent<Rigidbody>();
                collision.gameObject.transform.GetChild(i).gameObject.AddComponent<BoxCollider>();
                collision.gameObject.transform.GetChild(i).GetComponent<Rigidbody>().AddForce(Random.Range(-500, 500), Random.Range(200, 500), Random.Range(0, 500));
                collision.gameObject.transform.GetChild(i).GetComponent<MeshRenderer>().enabled = true;
                collision.gameObject.transform.GetChild(i).transform.parent = null;
            }
        }
        if (collision.gameObject.tag == "WindowGround" && scale > 1)
        {
            collision.gameObject.GetComponent<BoxCollider>().enabled = false;
            collision.gameObject.GetComponent<MeshRenderer>().enabled = false;
            for (int i = collision.gameObject.transform.childCount - 1; i >= 0; i--)
            {
                collision.gameObject.transform.GetChild(i).gameObject.AddComponent<Rigidbody>();
                collision.gameObject.transform.GetChild(i).gameObject.AddComponent<BoxCollider>();
             
                collision.gameObject.transform.GetChild(i).GetComponent<MeshRenderer>().enabled = true;
                collision.gameObject.transform.GetChild(i).transform.parent = null;
            }
        }
        if (collision.gameObject.tag == "Bridge" && bridgeDestroy)
        {
            collision.gameObject.GetComponent<BoxCollider>().enabled = false;
            collision.gameObject.GetComponent<MeshCollider>().enabled = false;
            collision.gameObject.GetComponent<MeshRenderer>().enabled = false;
            for (int i = collision.gameObject.transform.childCount - 1; i >= 0; i--)
            {
                collision.gameObject.transform.GetChild(i).gameObject.AddComponent<Rigidbody>();
                collision.gameObject.transform.GetChild(i).gameObject.AddComponent<BoxCollider>();
                collision.gameObject.transform.GetChild(i).GetComponent<Rigidbody>().AddForce(Random.Range(-100, 100), Random.Range(-100, 100), Random.Range(-100, 100));
                collision.gameObject.transform.GetChild(i).transform.parent = null;
            }
        }
    }
  
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Bridge")
        {
            onBridge = false;
            moveSpeed = 4;
            bridgeStopTimer = 0;
        }
       
    }
}
