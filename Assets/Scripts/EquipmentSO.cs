using UnityEngine;

[CreateAssetMenu]
public class EquipmentSO : ScriptableObject
{
    public string itemName;
    public int attack, defense, mana;
    [SerializeField] private Sprite itemSprite;

    public void PreviewEquipment()
    {
        GameObject.Find("StatManager").GetComponent<PlayerStats>().
            PreviewEquipmentStats(attack, defense, mana);
    }

    public void EquipItem()
    {
        PlayerStats playerStats = GameObject.Find("StatManager").GetComponent<PlayerStats>();
        playerStats.attack += attack;
        playerStats.defense += defense;
        playerStats.mana += mana;
        playerStats.UpdateEquipmentStats();
    }
    public void UnEquipItem()
    {
        PlayerStats playerStats = GameObject.Find("StatManager").GetComponent<PlayerStats>();
        playerStats.attack -= attack;
        playerStats.defense -= defense;
        playerStats.mana -= mana;
        playerStats.UpdateEquipmentStats();
    }
}
