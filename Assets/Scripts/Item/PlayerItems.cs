using System.Collections.Generic;
using UnityEngine;

public class PlayerItems : MonoBehaviour
{
    public List<SupportItem> items = new List<SupportItem>();  // list item

    void Start()
    {
        // Tạo 1 item để test
        SupportItem testItem = GetComponent<UmbrellaShieldItem>();
        if (testItem != null)
            items.Add(testItem);
    }

    void Update()
    {
        // Nhấn F để dùng item đầu tiên
        if (Input.GetKeyDown(KeyCode.F) && items.Count > 0)
        {
            SupportItem itemToUse = items[0];
            //items.RemoveAt(0);       // xóa khỏi list
            itemToUse.Activate(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        SupportItem item = other.GetComponent<SupportItem>();
        if (item != null)
        {
            items.Add(item);               // thêm vào list
            item.gameObject.SetActive(false);
            item.transform.SetParent(transform); // gắn vào player để không mất
        }
    }
}
