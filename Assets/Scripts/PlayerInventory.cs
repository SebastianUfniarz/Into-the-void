using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour, IInventory
{
    public int Money { get => money; set => money = value; }

    public int money = 0;
    public int Keys { get => keys; set => keys = value; }

    public int keys = 0;

    
}
