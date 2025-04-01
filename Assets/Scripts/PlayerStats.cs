using TMPro;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int attack, defense, mana;

    [SerializeField] private TMP_Text attackText, defenseText, manaText;
    [SerializeField] private TMP_Text attackPreText, defensePreText;
    [SerializeField] private TMP_Text attackInfoPanel, defenseInfoPanel;
    [SerializeField] private GameObject attackLabel, defenseLabel;
    private PlayerController playerController;
    private Damageable damageable;

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        damageable = GetComponent<Damageable>();
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