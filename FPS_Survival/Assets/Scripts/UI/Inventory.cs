using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static bool inventoryActivated = false;

    public GameObject inventoryBase;
    public GameObject slotsParent; // Grid Setting

    Slot[] slots;

    void Start()
    {
        slots = slotsParent.GetComponentsInChildren<Slot>();
    }

    void Update()
    {
        TryOpenInventory();
    }

    void TryOpenInventory()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryActivated = !inventoryActivated;

            if (inventoryActivated) OpenInventory();
            
            else CloseInventory();
        }
    }

    void OpenInventory()
    {
        inventoryBase.SetActive(true);
    }

    void CloseInventory()
    {
        inventoryBase.SetActive(false);
    }

    public void AcquireItem(Item getItem, int count = 1)
    {
        if(getItem.itemType != Item.ItemType.Equipment)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].item)
                {
                    if (slots[i].item.itemName == getItem.itemName)
                    {
                        slots[i].SetSlotCount(count);
                        return;
                    }
                }
            }
        }
        for (int i = 0; i < slots.Length; i++)
        {
            if (!slots[i].item)
            {
                slots[i].AddItem(getItem, count);
                return;
            }
        }
    }
}
