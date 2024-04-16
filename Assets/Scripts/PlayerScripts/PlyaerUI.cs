using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlyaerUI : MonoBehaviour
{
    [SerializeField] private Slider hpbar;
    [SerializeField] private Slider mpbar;
    [SerializeField] private PlayerStats playerStats;
    void Start()
    {
        hpbar.value = (float)playerStats.currentHp / (float)playerStats.maxHp;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)){playerStats.TakeDamage(10);} // q 누르면 피 깎임. 테스트용
        if (Input.GetKeyDown(KeyCode.E)){playerStats.UseMana(10);} // e누르면 mp 깎임. 테스트용
        HandleHp();
        HandleMp();
    }

    private void HandleHp(){
    
        hpbar.value = Mathf.Lerp(hpbar.value, (float)playerStats.currentHp / (float)playerStats.maxHp, Time.deltaTime *10);

    }

    private void HandleMp(){
    
        mpbar.value = Mathf.Lerp(mpbar.value, (float)playerStats.currentMp / (float)playerStats.maxMp, Time.deltaTime *10);

    }
}
