using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : MonoBehaviour
{
    public int damage;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Vector2 vector = new Vector2(0f, 20f);
            collision.GetComponent<Damageable>().Hit(damage, vector);
        }
    }
}
