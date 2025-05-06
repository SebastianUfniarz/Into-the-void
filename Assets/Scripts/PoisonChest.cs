using System.Collections;
using UnityEngine;

public class PoisonChest : MonoBehaviour
{
    public Animator animator;
    public bool isOpen = false;
    public int poisonDamage = 5;
    public float damageInterval = 1f;
    public float damageDuration = 4f;
    private Damageable player;

    private void Start()
    {
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.GetComponent<Damageable>();
            if (player == null)
            {
                Debug.LogError("Player object does not have a Damageable component!");
            }
        }
        else
        {
            Debug.LogError("Object with tag 'Player' not found!");
        }
    }

   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !isOpen)
        {
            animator.SetTrigger("isOpen");
            Debug.Log("Otwarto skrzynkê");
            isOpen = true;
            StartCoroutine(player.DealDamageOverTime(player, poisonDamage, damageInterval, damageDuration, "poison"));
        }
    }

    public void RemoveObject()
    {
        Destroy(gameObject);
    }
}
