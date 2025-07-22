using UnityEngine;

public class Chest_kamien_krwi : MonoBehaviour
{
    public Sprite artifactSprite;
    public ArtifactUI artifactUI;
    public Animator animator;
    public bool isOpen = false;

    [TextArea(3, 10)]
    public string artifactName = "Kamie� zakl�tej krwi ";

    [TextArea(5, 20)]
    public string artifactDescription =
        "Kamie� zakl�tej krwi to tajemniczy, pulsuj�cy artefakt, kt�ry zawiera esencj� pradawnej, spaczonej magii ofiarnej. " +
        "cDawny czarodziej ow�adni�ty jego moc�, popad� w obsesj� i rozpocz�� makabryczne eksperymenty, pr�buj�c stworzy� ��yw� form� czystej mocy�. " +
        " W wyniku jego szale�czych dzia�a� dosz�o do magicznej katastrofy, " +
        "kt�ra poch�on�a ca�� wiosk� � zostawiaj�c po niej jedynie popi�, echa cierpienia i nienazwan� groz�.";

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
