using UnityEngine;

public class Chest_grymuar : MonoBehaviour
{
    public Sprite artifactSprite;
    public ArtifactUI artifactUI;
    public Animator animator;
    public bool isOpen = false;

    [TextArea(3, 10)]
    public string artifactName = "Grymuar";

    [TextArea(5, 20)]
    public string artifactDescription =
        "Grymuar to pradawna ksi�ga zawieraj�ca zakl�cia, rytua�y i sekretn� wiedz� magiczn�, " +
        "cz�sto spisan� krwi� lub runami z dawno zapomnianych j�zyk�w. " +
        "Tylko wybrani magowie potrafi� j� odczyta�, a ka�da strona mo�e odmieni� los ca�ej krainy � lub j� zgubi�. " +
        "Ksi�ga emanuje w�asn� �wiadomo�ci� i wybiera w�a�ciciela, kieruj�c si� motywami znanymi jedynie jej mrocznemu duchowi.";

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
