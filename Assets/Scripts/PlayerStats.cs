using TMPro;
using UnityEngine;

public class PlayerStats : MonoBehaviour, ICharacterStats
{
    public int Attack => attack;
    public int Defense => defense;

    public int attack;
    public int defense;
    public int mana;
    public float criticalMultiplier = 2f;
    public int baseCriticalChance = 4;

    [SerializeField] private TMP_Text attackText, defenseText, manaText;
    [SerializeField] private TMP_Text attackPreText, defensePreText;
    [SerializeField] private TMP_Text attackInfoPanel, defenseInfoPanel;
    [SerializeField] private GameObject attackLabel, defenseLabel;


    private void Start()
    {
        UpdateEquipmentStats();
    }

    public void UpdateEquipmentStats()
    {
        attackText.text = attack.ToString();
        defenseText.text = defense.ToString();
        manaText.text = mana.ToString();

    }

    public void PreviewEquipmentStats(int attack, int defense, int mana)
    {
        if (attack != 0)
        {
            attackPreText.text = attack.ToString();
            attackLabel.SetActive(true);
        }
        else
        {
            attackInfoPanel.text = "";
            attackLabel.SetActive(false);
        }
        if (defense != 0)
        {
            defensePreText.text = defense.ToString();
            defenseLabel.SetActive(true);
        }
        else
        {
            defenseInfoPanel.text = "";
            defenseLabel.SetActive(false);
        }
    }

}