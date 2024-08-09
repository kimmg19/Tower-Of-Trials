using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShopUI : MonoBehaviour
{
    bool activeShop = false;
    public GameObject invenShopPanel;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            ToggleShop();
        }
    }
    public void ToggleShop()
    {
        activeShop = !activeShop;
        invenShopPanel.SetActive(activeShop);
    }
}
