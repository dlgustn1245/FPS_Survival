using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionController : MonoBehaviour
{
    public LayerMask layerMask;
    public Text actionText;
    public float range;

    bool pickupActivated = false;

    RaycastHit hitInfo;

    void Update()
    {
        TryAction();
        CheckItem();
    }

    void TryAction()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            CheckItem();
            CanPickUp();
        }
    }

    void CheckItem()
    {
        if (Physics.Raycast(this.transform.position, this.transform.forward, out hitInfo, range, layerMask))
        {
            if (hitInfo.transform.CompareTag("Item"))
            {
                ItemInfoAppear();
            }
        }
        else ItemInfoDisappear();
    }

    void CanPickUp()
    {
        if (pickupActivated)
        {
            if (hitInfo.transform)
            {
                Debug.Log(hitInfo.transform.GetComponent<ItemPcikUp>().item.itemName + "획득");
                Destroy(hitInfo.transform.gameObject);
                ItemInfoDisappear();
            }
        }
    }

    void ItemInfoAppear()
    {
        pickupActivated = true;
        actionText.gameObject.SetActive(true);
        actionText.text = hitInfo.transform.GetComponent<ItemPcikUp>().item.itemName + "획득" + "<color=yellow>" + "(E)" + "</color>";
    }

    void ItemInfoDisappear()
    {
        pickupActivated = false;
        actionText.gameObject.SetActive(false);
    }
}
