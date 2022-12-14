using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashBot : MonoBehaviour
{
    public GameObject armature, mesh;
    public ParticleSystem bloodPs;
    public GameObject character;

    private void Update()
    {
        if (character.transform.localScale.x > 1)
        {
            GetComponent<BoxCollider>().isTrigger = true;
        }
        else
        {
            GetComponent<BoxCollider>().isTrigger = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            armature.SetActive(false);
            mesh.SetActive(false);
            bloodPs.Play();
        }
    }
   
}
