using UnityEngine;

[CreateAssetMenu]
public class ItemSo : ScriptableObject
{
    public string itemName;
    public StatToChange statToChange = new StatToChange();
    public int amountToChangeStat;
    public Sprite sprite;
    public InventoryManager.ItemType type;
    public AttributesToChange attributeToChange = new AttributesToChange();
    public int amountToChangeAttribute;

    public int dropChance;

    public bool UseItem()
    {
        var player = GameObject.FindWithTag("Player").GetComponent<Damageable>();

        if (statToChange == StatToChange.health)
        {
            if (player.Health >= player.Maxhealth)
            {
                Debug.Log("Zdrowie gracza jest ju¿ maksymalne, przedmiot nie zosta³ u¿yty.");
                return false;
            }

            int healAmount = Mathf.Min(amountToChangeStat, player.Maxhealth - player.Health);

            if (healAmount > 0)
            {
                player.Heal(healAmount); 
                return true; 
            }
        }
        else if (statToChange == StatToChange.maxHealth)
        {
            player.IncreaseMaxHealth(amountToChangeStat);  
            return true; 
        }

        return false;
    }

    public enum StatToChange
    {
        none,
        health,
        maxHealth,
        mana,
        stamina
    };
    public enum AttributesToChange
    {
        none,
        strength,
        defense,
        inteligence,
        agility
    };


}
