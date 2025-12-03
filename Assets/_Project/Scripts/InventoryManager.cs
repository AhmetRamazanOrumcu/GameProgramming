using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public GameObject inventoryPanel;
    public static Inventory Instance;

    public List<Item> Items = new List<Item>();

    public Transform ItemContent;
    public GameObject InventoryItem;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
    }

    public void Add(Item item)
    {
        Items.Add(item);
    }

    public void Remove(Item item)
    {
        Items.Remove(item);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            bool aktifMi = inventoryPanel.activeSelf;
            inventoryPanel.SetActive(!aktifMi);

            if (!aktifMi) // Panel açılıyorsa
                Instance.ListItems(); // UI öğelerini listele
        }
    }



    public void ListItems()
    {
        //foreach (var item in Items)
        //{
        //    GameObject obj = Instantiate(InventoryItem, ItemContent);
        //    var itemName = obj.transform.Find("ItemName").GetComponent<TMP_Text>();
        //    var itemIcon = obj.transform.Find("ItemIcon").GetComponent<Image>();

        //    itemName.text = item.itemName;
        //    itemIcon.sprite = item.icon;
        //}
        // 1️⃣ Önce eski UI öğelerini temizle
        foreach (Transform child in ItemContent)
        {
            Destroy(child.gameObject);
        }

        // 2️⃣ Sonra güncel Items listesini ekle
        foreach (var item in Items)
        {
            GameObject obj = Instantiate(InventoryItem, ItemContent);
            var itemName = obj.transform.Find("ItemName").GetComponent<TMP_Text>();
            var itemIcon = obj.transform.Find("ItemIcon").GetComponent<Image>();

            itemName.text = item.itemName;
            itemIcon.sprite = item.icon;

        }
    }
}
