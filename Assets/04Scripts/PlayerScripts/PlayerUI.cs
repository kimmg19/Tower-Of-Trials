using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUI : MonoBehaviour
{   
    [SerializeField] private PlayerStats playerStats; // 플레이어 스탯 정보
    [SerializeField] private PlayerStatus playerStatus; // 플레이어 상태 정보
    [SerializeField] private Slider hpBar; // 체력바 슬라이더
    [SerializeField] private Slider mpBar; // mp바 슬라이더
    [SerializeField] private Slider staminaBar; // 스태미나바 슬라이더
    [SerializeField] private TextMeshProUGUI hpText; // 체력 텍스트
    [SerializeField] private TextMeshProUGUI mpText; // mp
    [SerializeField] private TextMeshProUGUI staminaText; // 스태미나 텍스트

    private float staminaRecoveryRate = 5f;  // 3초당 스태미나 회복량
    private float staminaRecoveryCooldown = 3f;  // 회복 시작까지 대기 시간
    private float staminaRecoveryTimer = 0f;  // 회복 타이머

    void Start()
    {
        // 초기화
        hpBar.value = (float)playerStats.currentHp / (float)playerStats.maxHp; // 체력바 초기값 설정
        hpText.text = playerStats.currentHp + "/" + playerStats.maxHp; // 체력 텍스트 초기값 설정
        mpText.text = playerStats.currentMp + "/" + playerStats.maxMp; // mp바 텍스트 초기값
        staminaText.text = playerStats.currentStamina + "/" + playerStats.maxStamina; // 스태미나 텍스트 초기값 설정
    }

    void Update()
    {
        // 플레이어 상태 및 스탯에 따라 UI 업데이트
        // if (Input.GetKeyDown(KeyCode.Q)) { playerStatus.TakeDamage(10); } // q 키를 누르면 피 깎임. 테스트용
        if (Input.GetKeyDown(KeyCode.E)) { playerStatus.UseStamina(10); } // e 키를 누르면 스태미나가 소모됨. 테스트용
        if (Input.GetKeyDown(KeyCode.R)) { playerStatus.UseMp(10); } // r 키를 누르면 마나가 소모됨. 테스트용

        HandleHp(); // 체력 UI 업데이트
        HandleMp(); // mp UI 업데이트
        HandleStamina(); // 스태미나 UI 업데이트
        RecoverStamina(); // 스태미나 회복 처리
    }

    // 체력 UI 업데이트
    private void HandleHp()
    {
        hpBar.value = Mathf.Lerp(hpBar.value, (float)playerStats.currentHp / (float)playerStats.maxHp, Time.deltaTime * 10); // 체력바 갱신
        hpText.text = playerStats.currentHp + " / " + playerStats.maxHp; // 체력 텍스트 갱신
    }

    private void HandleMp()
    {
        mpBar.value = Mathf.Lerp(mpBar.value, (float)playerStats.currentMp / (float)playerStats.maxMp, Time.deltaTime * 10); // mp바 갱신
        mpText.text = playerStats.currentMp + " / " + playerStats.maxMp; // 마나 텍스트 갱신
    }
    // 스태미나 UI 업데이트
    private void HandleStamina()
    {
        staminaBar.value = Mathf.Lerp(staminaBar.value, (float)playerStats.currentStamina / (float)playerStats.maxStamina, Time.deltaTime * 10); // 스태미나바 갱신
        staminaText.text = playerStats.currentStamina + " / " + playerStats.maxStamina; // 스태미나 텍스트 갱신
    }

    // 스태미나 회복 처리
    private void RecoverStamina()
    {
        if (playerStats.currentStamina < playerStats.maxStamina) // 현재 스태미나가 최대 스태미나보다 작으면
        {
            staminaRecoveryTimer += Time.deltaTime; // 회복 타이머를 증가시킴
            if (staminaRecoveryTimer >= staminaRecoveryCooldown) // 회복 타이머가 회복 대기 시간을 초과하면
            {
                playerStats.currentStamina += Mathf.RoundToInt(staminaRecoveryRate); // 스태미나 회복량만큼 스태미나를 증가
                if (playerStats.currentStamina > playerStats.maxStamina) // 현재 스태미나가 최대 스태미나를 초과하면
                {
                    playerStats.currentStamina = playerStats.maxStamina; // 현재 스태미나를 최대 스태미나로 설정
                }
                staminaRecoveryTimer = 0f; // 회복 타이머를 초기화하여 다음 회복 주기를 시작함
            }
        }
    }
}
