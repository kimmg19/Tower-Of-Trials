using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        Inventory.instance.LoadInventory();
    }

    private void OnApplicationQuit()
    {
        Inventory.instance.SaveInventory();
    }
}
