using UnityEngine;
using UnityEngine.Events;


public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private InventoryManager inventory; //drag inventory here

    private void Awake()
    {
        if (inventory == null) inventory = FindObjectOfType<InventoryManager>();
    }


    public void Collect(ItemClass item, int quantity)
    {
        if (item == null || inventory == null) return;
        inventory.Add(item, quantity);
    }
}


