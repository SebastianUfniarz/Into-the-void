using UnityEngine;

public class Chest_worlds : MonoBehaviour
{
    public Sprite artifactSprite;
    public ArtifactUI artifactUI;
    public Animator animator;
    public bool isOpen = false;

    [TextArea(3, 10)]
    public string artifactName = "Uwi�zione nowe �wiaty";

    [TextArea(5, 20)]
    public string artifactDescription =
        "Uwi�zione Nowe �wiaty to tajemnicza fiolka, w kt�rej wed�ug legend zakl�to nie narodzon� jeszcze rzeczywisto�� � potencjalny �wiat, czekaj�cy na uwolnienie. " +
        "Esencja wewn�trz pulsuje jak �ywa, a ka�dy, kto zbli�y j� do serca, s�yszy szept mo�liwych przysz�o�ci, alternatywnych los�w i dr�g, kt�rych nigdy nie obrano. " +
        "Uwolnienie jej mocy mo�e stworzy� nowe istnienie... albo wymaza� obecne.";

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
