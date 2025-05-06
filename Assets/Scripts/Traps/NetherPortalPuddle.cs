using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetherPortalPuddle : MonoBehaviour
{
    public int damagePerTick;
    public float interval;
    public float duration;
    private Damageable player;
    private bool isActivated;

    private void Start()
    {
        isActivated = true;
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.GetComponent<Damageable>();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && isActivated)
        {
            print("Gracz zosta³ zatruty");
            StartCoroutine(HandleDamageOverTime());
            isActivated = false;
        }
    }

    private IEnumerator HandleDamageOverTime() 
    {
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(player.DealDamageOverTime(player, damagePerTick, interval, 10, "poison"));
        isActivated = true;
    }
}
