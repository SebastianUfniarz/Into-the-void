using UnityEngine;

public class Chest_memory : MonoBehaviour
{
    public Sprite artifactSprite;
    public ArtifactUI artifactUI;
    public Animator animator;
    public bool isOpen = false;

    [TextArea(3, 10)]
    public string artifactName = "Wspomnienie dawnego imperium";

    [TextArea(5, 20)]
    public string artifactDescription =
        "Wspomnienie Dawnego Imperium to eteryczny artefakt w formie migotliwego kryszta�u lub mg�y, unosz�cej si� nad ziemi� i pulsuj�cej echem minionej chwa�y. " +
        " Zawiera zakl�te obrazy, g�osy i emocje z czas�w, gdy imperium w�ada�o kontynentem " +
        "� triumfy, zdrady i upadki zapisane w magicznej pami�ci. Ka�dy, kto si� z nim po��czy, mo�e ujrze� przesz�o�� oczami dawnych bohater�w...  "+
        "lecz zbyt d�ugie obcowanie grozi zatraceniem w cudzych wspomnieniach.";
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
