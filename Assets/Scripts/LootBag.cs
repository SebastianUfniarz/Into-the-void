using System.Collections.Generic;
using UnityEngine;

public class LootBag : MonoBehaviour
{
    public List<ItemSo> lootList = new List<ItemSo>();  
   
    public List<ItemSo> GetDroppedItems()
    {
        List<ItemSo> possibleItems = new List<ItemSo>();

        foreach (ItemSo item in lootList)
        {
            int randomNumber = Random.Range(1, 101);
            if (randomNumber <= item.dropChance)
            {
                possibleItems.Add(item);
            }
        }

        return possibleItems;
    }

    public void DropLoot(Vector3 spawnPosition)
    {
        List<ItemSo> droppedItems = GetDroppedItems();
        if (droppedItems != null && droppedItems.Count > 0)
        {           
            foreach (ItemSo item in droppedItems)
            {              
                GameObject lootGameObject = new GameObject(item.itemName);  

                SpriteRenderer spriteRenderer = lootGameObject.AddComponent<SpriteRenderer>();
                spriteRenderer.sprite = item.sprite;

                Rigidbody2D rb = lootGameObject.AddComponent<Rigidbody2D>();
                rb.gravityScale = 1;

                BoxCollider2D collider = lootGameObject.AddComponent<BoxCollider2D>();
                lootGameObject.transform.position = new Vector3(spawnPosition.x, spawnPosition.y + 2, spawnPosition.z);

                Item itemComponent = lootGameObject.AddComponent<Item>();
                itemComponent.SetItem(item);  
            }
        }
    }
}
