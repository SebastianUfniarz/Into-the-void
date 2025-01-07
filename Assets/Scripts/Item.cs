using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] internal int quantity;
    [SerializeField] internal string itemName;
    [SerializeField] internal Sprite sprite;
    [SerializeField] internal string itemDescription;
    public InventoryManager.ItemType itemType;

    private InventoryManager inventoryManager;

    private ItemSo itemSo;  // Przechowuje dane o przedmiocie

    // Dodajemy mo�liwo�� inicjalizacji z ItemSo
    public void Initialize(ItemSo itemSo)
    {
        this.itemName = itemSo.itemName;
        this.sprite = itemSo.sprite;
        this.itemDescription = "Opis przedmiotu: " + itemSo.itemName;  // Mo�esz dostosowa� opis
        this.itemType = itemSo.type;
        this.quantity = 1; // Zak�adaj�c, �e przedmioty maj� zawsze ilo�� 1 w lootbagu (mo�esz to zmieni�)
    }

    // Ustawienie danych przedmiotu z ItemSo
    public void SetItem(ItemSo item)
    {
        itemSo = item;  // Zapisujemy ItemSo w polu

        // Ustawiamy w�a�ciwo�ci na podstawie ItemSo
        this.itemName = item.itemName;
        this.sprite = item.sprite;
        this.itemDescription = "Opis przedmiotu: " + item.itemName;  // Mo�esz dostosowa� opis
        this.itemType = item.type;
        this.quantity = 1; // Zak�adaj�c, �e przedmioty maj� zawsze ilo�� 1 w lootbagu (mo�esz to zmieni�)
    }

    private void Awake()
    {
        inventoryManager = GameObject.Find("InventoryCanvas").GetComponent<InventoryManager>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Przedmiot dodany do inwentarza, reszta jest usuwana
            int leftOverItems = inventoryManager.AddItem(itemName, quantity, sprite, itemDescription, itemType);
            if (leftOverItems <= 0)
            {
                Destroy(gameObject);  // Zniszczenie obiektu, je�li dodano do inwentarza
            }
            else
            {
                quantity = leftOverItems;  // Pozostawiamy pozosta�� ilo��
            }
        }
    }
}
