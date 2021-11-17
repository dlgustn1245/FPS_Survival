using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Item item; //획득한 아이템
    public int itemCount;
    public Image getItemImage;

    public Text countText;
    public GameObject countImage;

    void SetColor(float alpha)
    {
        Color color = getItemImage.color;
        color.a = alpha;
        getItemImage.color = color;
    }

    public void AddItem(Item getItem, int count = 1)
    {
        this.item = getItem;
        itemCount = count;
        getItemImage.sprite = getItem.itemImage;

        if(getItem.itemType != Item.ItemType.Equipment)
        {
            countImage.SetActive(true);
            countText.text = itemCount.ToString();
        }
        else
        {
            countText.text = "0";
            countImage.SetActive(false);       
        }

        SetColor(1.0f);
    }

    public void SetSlotCount(int count)
    {
        itemCount += count;
        countText.text = itemCount.ToString();

        if (itemCount <= 0) ClearSlot();
    }

    void ClearSlot()
    {
        item = null;
        itemCount = 0;
        getItemImage.sprite = null;
        SetColor(0.0f);

        countText.text = "0";
        countImage.SetActive(false);
    }
}
