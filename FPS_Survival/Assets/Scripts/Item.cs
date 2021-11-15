using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "New Item/item")]
public class Item : ScriptableObject
{
    public string itemName;
    public ItemType itemType;
    public Sprite itemImage; //image : 캔버스 내에서만 출력가능 / sprite : 캔버스 밖에서도 출력 가능
    public GameObject itemPrefab;

    public string weaponType;

    public enum ItemType { Equipment, Used, Ingredient, ETC }
}
