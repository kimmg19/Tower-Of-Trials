using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private Slider hpbar;
    [SerializeField] private Slider staminabar;
    [SerializeField] PlayerStats playerStats;
    [SerializeField] PlayerStatus playerStatus;
    void Start()
    {
        hpbar.value = (float)playerStats.currentHp / (float)playerStats.maxHp;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q)){playerStatus.TakeDamage(10);} // q 누르면 피 깎임. 테스트용
        if (Input.GetKeyDown(KeyCode.E)){playerStatus.UseStamina(10);} // e누르면 stamina 깎임. 테스트용
        HandleHp();
        HandleStamina();
    }

    private void HandleHp(){
    
        hpbar.value = Mathf.Lerp(hpbar.value, (float)playerStats.currentHp / (float)playerStats.maxHp, Time.deltaTime *10);

    }

    private void HandleStamina(){
    
        staminabar.value = Mathf.Lerp(staminabar.value, (float)playerStats.currentStamina / (float)playerStats.maxStamina, Time.deltaTime *10);

    }
}
