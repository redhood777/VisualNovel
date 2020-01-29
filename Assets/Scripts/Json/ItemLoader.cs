using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemLoader : MonoBehaviour
{
    public List<Item> itemList;
    public Dictionary<int, Item> items;
    private void Awake()
    {
        items = new Dictionary<int, Item>();
        ItemDictionary dictionary = JsonUtility.FromJson<ItemDictionary>(JsonFileReader.LoadJsonAsResource("Items/ItemDictionary.json"));
        foreach(string dictionaryItem in dictionary.items)
        {
            LoadItem(dictionaryItem);
        }

        foreach (KeyValuePair<int, Item> entry in items)
        {
            Item temp = entry.Value;
            itemList.Add(temp);
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadItem(string path)
    {
        string myLoadedItem = JsonFileReader.LoadJsonAsResource(path);
        Item myItem = JsonUtility.FromJson<Item>(myLoadedItem);

        if (items.ContainsKey(myItem.itemID))
        {
            Debug.Log("Item:" + myItem.itemName + " ||| Key already exists same as :" + items[myItem.itemID].itemName);
        }
        else
        {
            items.Add(myItem.itemID, myItem);
        }
        
    }
}
