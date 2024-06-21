using UnityEngine;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
   
    public List<Item> items = new List<Item>();
   

    public void AddItem(Item item)
    {
            Debug.Log("Item get!");
            items.Add(item);
       
    }

    public bool HasItem(string itemName)//���� ������ ���� ���� üũ �޼���
    {
        foreach(Item item in items)
        {
            if(item.itemName==itemName)//������ �̸��� �޾� �̸� ������ �̸��� �����ϸ� true�� ��ȯ
            {
                return true;
            }
        }
        return false;
    }
}
