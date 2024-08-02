using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrade : MonoBehaviour
{
    [SerializeField] private GameObject upgradePanel;


    void Start()
    {
        if (upgradePanel != null)
        {
            Debug.Log("Awake");
            upgradePanel.SetActive(false);
        }
    }
    
     public void ShowUpgradePanel()
    {
        if (upgradePanel != null)
        {
            upgradePanel.SetActive(true);
            Debug.Log("tq.");
        }
    }

        public void HideUpgradePanel()
    {
        if (upgradePanel != null)
        {
            upgradePanel.SetActive(false);
        }
    }
}
