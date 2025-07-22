using UnityEngine;

public class Chest_quill : MonoBehaviour
{
    public Sprite artifactSprite;
    public ArtifactUI artifactUI;
    public Animator animator;
    public bool isOpen = false;

    [TextArea(3, 10)]
    public string artifactName = "Pi�ro dawnego czarodzieja";

    [TextArea(5, 20)]
    public string artifactDescription =
        "Pi�ro Dawnego Czarodzieja to delikatne, lecz magicznie nienaruszalne pi�ro, przez wieki s�u��ce jako narz�dzie do spisywania zakl��, kl�tw i proroctw. " +
        "Pozostawione przez nie�yj�cego arcymaga, wci�� tchnie resztkami jego pot�gi � ka�de s�owo, kt�re nim zapiszesz, mo�e sta� si� rzeczywisto�ci�� " +
        "lub przekle�stwem. U�ywane nierozwa�nie, pi�ro nie tylko spe�nia �yczenia, ale te� zdradza intencje serca w�a�ciciela, " +
        "czasem daj�c mu to, czego pragnie � a nie to, czego naprawd� potrzebuje.";

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
