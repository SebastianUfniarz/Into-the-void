using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hedgehog : MonoBehaviour
{
    public int damage;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Vector2 vector = new Vector2(Random.Range(-5f, 5f), 20f);
            collision.GetComponent<Damageable>().Hit(damage, vector);
        }
    }
}
