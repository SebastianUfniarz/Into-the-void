using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spikes : MonoBehaviour
{
    public int damage;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Vector2 vector = new Vector2(0, 0);
            collision.GetComponent<Damageable>().Hit(damage, vector);
        }
    }
}
