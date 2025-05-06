using UnityEngine;

[System.Serializable]
public class ItemSet
{
    public string setName;
    public EquipmentSO[] requiredEquipment; 
    public int armorBonus;

    [HideInInspector] public bool isActive;
}