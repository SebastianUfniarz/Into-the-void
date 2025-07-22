using UnityEngine;

public class Chest_runa : MonoBehaviour
{
    public Sprite artifactSprite;
    public ArtifactUI artifactUI;
    public Animator animator;
    public bool isOpen = false;

    [TextArea(3, 10)]
    public string artifactName = "Kamie� runiczny dawnego czarodzieja";

    [TextArea(5, 20)]
    public string artifactDescription =
        "Kamie� Runiczny Dawnego Czarodzieja to staro�ytny od�amek obelisku, pokryty migocz�cymi runami, kt�re zmieniaj� si� pod wp�ywem magii i emocji. " +
        "Niegdy� nale�a� do pot�nego arcymaga, kt�ry zapiecz�towa� w nim fragment w�asnej mocy oraz wspomnienia sprzed Wielkiego Roz�amu. " +
        "Kamie� szeptem kusi nowych w�a�cicieli, obiecuj�c im pot�g� � lecz ka�dy, kto nie zdo�a ujarzmi� jego woli, ryzykuje utrat� w�asnej to�samo�ci. ";
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
