using UnityEngine;

public class Chest_egg : MonoBehaviour
{
    public Sprite artifactSprite;
    public ArtifactUI artifactUI;
    public Animator animator;
    public bool isOpen = false;

    [TextArea(3, 10)]
    public string artifactName = "Smocze jajo";

    [TextArea(5, 20)]
    public string artifactDescription =
        "Smocze Jajo to rzadki, ciep�y w dotyku artefakt, pokryty �uskowat� skorup� o metalicznym po�ysku i pulsuj�cym wn�trzu, jakby skrywa� bij�ce serce. " +
        "W jego wn�trzu dojrzewa istota o niepoj�tej sile � smok, kt�ry po wykluciu zwi��e si� z pierwszym, kt�rego uzna za godnego. " +
        "Wielu marzy, by je zdoby� ale niewielu rozumie, �e opieka nad smokiem to nie tylko pot�ga � to odpowiedzialno�� i gro�ba przebudzenia pradawnej furii. ";
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
