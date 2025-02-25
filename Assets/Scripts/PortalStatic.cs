using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalStatic : MonoBehaviour
{
    public Animator animator;
    public GameObject StaticPortal;
    public GameObject PortalGIF;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PortalGIF.SetActive(false);
        if (collision.tag == "Player")
        {
            StaticPortal.SetActive(false);
            PortalGIF.SetActive(true);
        
        }
    }
    public void RemoveObject()
    {
        Destroy(gameObject);
    }

}
