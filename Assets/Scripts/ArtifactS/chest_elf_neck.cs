using UnityEngine;

public class Chest_elf_neck : MonoBehaviour
{
    public Sprite artifactSprite;
    public ArtifactUI artifactUI;
    public Animator animator;
    public bool isOpen = false;

    [TextArea(3, 10)]
    public string artifactName = "Naszyjnik kr�lowej elf�w";

    [TextArea(5, 20)]
    public string artifactDescription =
        "Naszyjnik Kr�lowej Elf�w to misternie utkany klejnot ze s�onecznego z�ota i �ez porannej rosy, " +
        "przekazywany z pokolenia na pokolenie w�r�d elfiej arystokracji. " +
        "Skrywa b�ogos�awie�stwo pradawnej kr�lowej, kt�re wzmacnia wi� z natur� i pozwala s�ysze� szept lasu oraz pie�ni gwiazd. ";
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isOpen)
        {
            OpenChest();
        }
    }

    private void OpenChest()
    {
        isOpen = true;

        if (animator != null)
            animator.SetTrigger("isOpen");

        if (artifactUI != null)
        {
            artifactUI.ShowArtifact(artifactName, artifactDescription, artifactSprite);
        }   

        LootBag lootBag = GetComponent<LootBag>();
        if (lootBag != null)
        {
            lootBag.DropLoot(transform.position);
        }

    
    }

    public void RemoveObject()
    {
        Destroy(gameObject);
    }
}
