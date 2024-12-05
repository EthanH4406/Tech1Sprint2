using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    Dictionary<int, PlayerItem> inventory = new Dictionary<int, PlayerItem>();

    public bool AddItemToInventory(PlayerItem item)
    {
        if(inventory.ContainsKey(item.id))
        {
            return true;
        }

        inventory.Add(item.id, item);
        return true;
    }

    public bool RemoveItemFromInventory(int id)
    {
        if(inventory.ContainsKey(id))
        {
            return false;
        }

        inventory.Remove(id);
        return true;
    }

    public void DropItem(Vector3 playerPosition, int id)
    {
        float itemDropHeightOffset = 0.3f;
        float newY = playerPosition.y + itemDropHeightOffset;

        Vector3 newSpawnPos = new Vector3(playerPosition.x, newY, playerPosition.z);

        PlayerItem item;
        inventory.TryGetValue(id, out item);

        bool success = RemoveItemFromInventory(id);

        if(success)
        {
            Instantiate(item.itemPickup, newSpawnPos, Quaternion.identity);
        }

    }
    
}
