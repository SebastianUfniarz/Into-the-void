using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderWeb : MonoBehaviour
{
    public float slow;
    private float normalSpeed;
    private float normalJumpImpulse;
    private PlayerController player;
    private void Start()
    {
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.GetComponent<PlayerController>();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            normalSpeed = player.moveSpeed;
            normalJumpImpulse = player.jumpImpulse;
            player.moveSpeed = normalSpeed - slow;
            player.jumpImpulse = normalJumpImpulse - slow;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    { 
        if(collision.tag == "Player") 
        {
            player.moveSpeed = normalSpeed;
            player.jumpImpulse = normalJumpImpulse;
        }
    }


}
