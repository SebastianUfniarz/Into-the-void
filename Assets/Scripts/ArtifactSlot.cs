using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using static InventoryManager;

public class ArtifactSlot : MonoBehaviour
{
    public string itemName;
    public Sprite itemSprite;
    public string itemDescription;
    public bool isFull;
    public int quantity;

    [SerializeField] private Image itemImage;
    [SerializeField] private TMP_Text itemNameText;
    [SerializeField] private TMP_Text itemDescriptionText;
    [SerializeField] private Sprite emptySprite;

    public int AddArtifact(string itemName, int quantity, Sprite itemSprite, string itemDescription, ItemType itemType)
    {
        if (isFull)
            return quantity; // Je�li slot jest zaj�ty, zwraca ca�� ilo�� (nic nie dodaje)

        this.itemName = itemName;
        this.itemSprite = itemSprite;
        this.itemDescription = itemDescription;
        isFull = true;

        UpdateSlotUI();

        return 0; // Artefakty nie s� stackowalne, wi�c ca�a ilo�� zosta�a dodana
    }


    private void UpdateSlotUI()
    {
        if (isFull)
        {
            itemImage.sprite = itemSprite;
        }
        else
        {
            ClearSlot();
        }
    }

    private void ClearSlot()
    {
        itemName = string.Empty;
        itemSprite = null;
        itemDescription = string.Empty;
        isFull = false;

        itemImage.sprite = emptySprite;
        itemNameText.text = "";
        itemDescriptionText.text = "";
    }
}
