using UnityEngine;

public class Chest_leszy : MonoBehaviour
{
    public Sprite artifactSprite;
    public ArtifactUI artifactUI;
    public Animator animator;
    public bool isOpen = false;

    [TextArea(3, 10)]
    public string artifactName = "Korona leszego";

    [TextArea(5, 20)]
    public string artifactDescription =
        "Korona Leszego to dziki, �yj�cy artefakt spleciony z poro�a, korzeni i mchu, pulsuj�cy moc� pradawnych duch�w puszczy. " +
        "Nosz�cy j� zyskuje w�adz� nad le�nymi istotami, burzami i cyklami natury � ale w zamian musi odda� cz�� w�asnej duszy dzikowi i chaosowi. " +
        "Je�li korona trafi w niepowo�ane r�ce, las zaczyna si� buntowa�, a granica mi�dzy �wiatem ludzi a pierwotn� dzicz� zaczyna zanika�. ";
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
