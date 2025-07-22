using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePotionPickup : MonoBehaviour
{
    public float damageMultiplier = 1.2f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            PlayerController playerController = collision.GetComponent<PlayerController>();
            if (playerController != null)
            {
                print("Potka na obrazenia dziala");
                gameObject.SetActive(false);
            }
        }
    }
}
