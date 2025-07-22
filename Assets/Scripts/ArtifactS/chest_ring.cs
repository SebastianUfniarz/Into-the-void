using UnityEngine;

public class Chest_ring : MonoBehaviour
{
    public Sprite artifactSprite;
    public ArtifactUI artifactUI;
    public Animator animator;
    public bool isOpen = false;

    [TextArea(3, 10)]
    public string artifactName = "Brzemi� pierwszego w�adcy";

    [TextArea(5, 20)]
    public string artifactDescription =
        "Brzemi� Pierwszego W�adcy to staro�ytny pier�cie� wykuty ze z�ota i zdobiony pojedynczym ametystem. " +
        "Nale�a� do legendarnego za�o�yciela kr�lestwa, kt�ry zwi�za� sw� dusz� z losem ziem, nad kt�rymi panowa�  " +
        "� jego moc daje nosz�cemu niezwyk�� charyzm� i autorytet, ale te� niesie ci�ar odpowiedzialno�ci i cierpienia ludu. " +
        "Ka�dy w�adca, kt�ry go nosi, zaczyna s�ysze� g�osy przodk�w... i czuje na palcu nie pier�cie�, lecz �a�cuch.";

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
