using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public GameObject InventoryMenu;
    public GameObject EquipmentMenu;
    public GameObject ArtifactPanel; 
    public GameObject EquipmentButton;
    public GameObject InventoryButton;
    public GameObject ArtifactButton;

    private bool isInventoryOpen = false;
    internal bool isEquipmentOpen = false;
    private bool isArtifactPanelOpen = false;

    public ItemSlot[] itemSlot;
    public EquipmentSlot[] equipmentSlot;
    public EquippedSlot[] equippedSlot;
    public ItemSo[] itemSOs;
    public ArtifactSlot[] artifactSlot;

    public bool IsEquipmentOpen => isEquipmentOpen;
    public bool IsInventoryOpen => isInventoryOpen;
    public bool IsArtifactPanelOpen => isArtifactPanelOpen;

    public void SwitchToInventory()
    {
        CloseAllPanels();
        isInventoryOpen = true;
        InventoryMenu.SetActive(true);
        UpdateButtonVisibility();
    }

    public void SwitchToEquipment()
    {
        CloseAllPanels();
        isEquipmentOpen = true;
        EquipmentMenu.SetActive(true);
        UpdateButtonVisibility();
    }

    public void SwitchToArtifactPanel()
    {
        CloseAllPanels();
        isArtifactPanelOpen = true;
        ArtifactPanel.SetActive(true);
        UpdateButtonVisibility();
    }

    public void ToggleInventory()
    {
        if (isInventoryOpen)
        {
            CloseInventory();
        }
        else
        {
            CloseAllPanels();
            isInventoryOpen = true;
            InventoryMenu.SetActive(true);
        }

        UpdateButtonVisibility();
    }

    public void ToggleEquipment()
    {
        if (isEquipmentOpen)
        {
            CloseEquipment();
        }
        else
        {
            CloseAllPanels();
            isEquipmentOpen = true;
            EquipmentMenu.SetActive(true);
        }

        UpdateButtonVisibility();

    }

    public void ToggleArtifactPanel()
    {
        if (isArtifactPanelOpen)
        {
            CloseArtifactPanel();
        }
        else
        {
            CloseAllPanels();
            isArtifactPanelOpen = true;
            ArtifactPanel.SetActive(true);
        }

        UpdateButtonVisibility();
    }

    public void CloseInventory()
    {
        isInventoryOpen = false;
        InventoryMenu.SetActive(false);
        UpdateButtonVisibility();
    }

    public void CloseEquipment()
    {
        isEquipmentOpen = false;
        EquipmentMenu.SetActive(false);
        UpdateButtonVisibility();
    }

    public void CloseArtifactPanel()
    {
        isArtifactPanelOpen = false;
        ArtifactPanel.SetActive(false);
        UpdateButtonVisibility();
    }

    private void CloseAllPanels()
    {
        CloseInventory();
        CloseEquipment();
        CloseArtifactPanel();
    }

    private void UpdateButtonVisibility()
    {
        bool shouldShowButtons = isInventoryOpen || isEquipmentOpen || isArtifactPanelOpen;
        EquipmentButton.SetActive(shouldShowButtons);
        InventoryButton.SetActive(shouldShowButtons);
        ArtifactButton.SetActive(shouldShowButtons);
    }

    public bool UseItem(string itemName)
    {
        for (int i = 0; i < itemSlot.Length; i++)
        {
            if (itemSlot[i].isFull && itemSlot[i].itemName == itemName)
            {
                bool usable = false;
                for (int j = 0; j < itemSOs.Length; j++)
                {
                    if (itemSOs[j].name == itemName)
                    {
                        usable = itemSOs[j].UseItem();
                        break;
                    }
                }

                if (usable)
                {
                    itemSlot[i].quantity--;

                    if (itemSlot[i].quantity <= 0)
                    {
                        itemSlot[i].ClearSlot();
                    }

                    return true;
                }
            }
        }
        return false;
    }

    public int AddItem(string itemName, int quantity, Sprite itemSprite, string itemDescription, ItemType itemType)
    {
        if (itemType == ItemType.consumable || itemType == ItemType.crafting || itemType == ItemType.collectible)
        {
            for (int i = 0; i < itemSlot.Length; i++)
            {
                if (!itemSlot[i].isFull || itemSlot[i].quantity == 0)
                {
                    int leftOverItems = itemSlot[i].AddItem(itemName, quantity, itemSprite, itemDescription, itemType);
                    return leftOverItems;
                }
            }
        }
        else if (itemType == ItemType.artifact)  // ?? Nowe sprawdzenie dla artefaktów
        {
            for (int i = 0; i < artifactSlot.Length; i++)  // Zak³adamy, ¿e masz tablicê artifactSlot[]
            {
                if (!artifactSlot[i].isFull || artifactSlot[i].quantity == 0)
                {
                    int leftOverItems = artifactSlot[i].AddArtifact(itemName, quantity, itemSprite, itemDescription, itemType);
                    return leftOverItems;
                }
            }
        }
        else  // Dla ekwipunku (zbroja, broñ itp.)
        {
            for (int i = 0; i < equipmentSlot.Length; i++)
            {
                if (!equipmentSlot[i].isFull || equipmentSlot[i].quantity == 0)
                {
                    int leftOverItems = equipmentSlot[i].AddItem(itemName, quantity, itemSprite, itemDescription, itemType);
                    return leftOverItems;
                }
            }
        }

        return quantity; // Jeœli nie by³o miejsca, zwraca pozosta³¹ iloœæ
    }


    public void DeselectAllSlots()
    {
        for (int i = 0; i < itemSlot.Length; i++)
        {
            itemSlot[i].selectedShader.SetActive(false);
            itemSlot[i].thisItemSelected = false;
        }
        for (int i = 0; i < equipmentSlot.Length; i++)
        {
            equipmentSlot[i].selectedShader.SetActive(false);
            equipmentSlot[i].thisItemSelected = false;
        }
        for (int i = 0; i < equippedSlot.Length; i++)
        {
            equippedSlot[i].selectedShader.SetActive(false);
            equippedSlot[i].thisItemSelected = false;
        }
    }

    public enum ItemType
    {
        consumable,
        crafting,
        collectible,
        head,
        body,
        legs,
        feet,
        hand,
        amulet,
        ring,
        artifact,
        none
    };
}
