using UnityEngine;

public class EnemyStats : MonoBehaviour, ICharacterStats
{
    [SerializeField] private int _attack = 10;
    [SerializeField] private int _defense = 5;

    public int Attack => _attack;
    public int Defense => _defense;

    public void SetStats(int attack, int defense)
    {
        _attack = attack;
        _defense = defense;
    }
}