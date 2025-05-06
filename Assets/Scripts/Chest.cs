using UnityEngine;

public class Chest : MonoBehaviour
{
    public Animator animator;
    public bool isOpen = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !isOpen)
        {
            animator.SetTrigger("isOpen");
            Debug.Log("Otwarto skrzynkê");
            isOpen = true;
           
            LootBag lootBag = GetComponent<LootBag>();
            if (lootBag != null)
            {
                lootBag.DropLoot(transform.position);  
            }
        }
    }
    public void RemoveObject()
    {
        Destroy(gameObject);
    }

}
