using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowShooter : MonoBehaviour
{
    public float attackCooldown;
    public Transform firePoint;
    public GameObject[] arrows;
    float cooldownTimer;

    private void Attack()
    {
        cooldownTimer = 0;

        arrows[FindArrow()].transform.position = firePoint.position;
        arrows[FindArrow()].GetComponent<ArrowProjectile>().ActivateProjectile();
    }

    private int FindArrow() 
    {
        for (int i = 0; i < arrows.Length; i++)
        {
            if (!arrows[i].activeInHierarchy) 
            {
                return i;
            }
        }

        return 0;
    }

    private void Update() 
    {
        cooldownTimer += Time.deltaTime;
        if (cooldownTimer >= attackCooldown) 
        {
            Attack();
        }
    }
}
