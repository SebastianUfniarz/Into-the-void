using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] internal int quantity;
    [SerializeField] internal string itemName;
    [SerializeField] internal Sprite sprite;
    [SerializeField] internal string itemDescription;
    
    public InventoryManager.ItemType itemType;
    private InventoryManager inventoryManager;
    private ItemSo itemSo;

    public void Initialize(ItemSo itemSo)
    {
        this.itemName = itemSo.itemName;
        this.sprite = itemSo.sprite;
        this.itemDescription = "Opis przedmiotu: " + itemSo.itemName;
        this.itemType = itemSo.type;
        this.quantity = 1;
    }

    public void SetItem(ItemSo item)
    {
        itemSo = item;

        this.itemName = item.itemName;
        this.sprite = item.sprite;
        this.itemDescription = "Opis przedmiotu: " + item.itemName;
        this.itemType = item.type;
        this.quantity = 1; 
    }

    private void Awake()
    {
        inventoryManager = GameObject.Find("InventoryCanvas").GetComponent<InventoryManager>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            int leftOverItems = inventoryManager.AddItem(itemName, quantity, sprite, itemDescription, itemType);
            if (leftOverItems <= 0)
            {
                Destroy(gameObject);
            }
            else
            {
                quantity = leftOverItems;
            }
        }
    }
}
