using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Upgrade : MonoBehaviour
{
    [SerializeField] private GameObject upgradePanel;
    public int WeaponEnhancePoint = 0;
    public int WeaponATK = 0;
    public int Attack = 0;
    public int PlayerAttack = 20;
    int UpgradePos = 90;
    int UpgradeGold = 100;
    public SwordEft swordEft;
    public PlayerStats playerstats;

    public Text WeaponEnhanceText;
    public Text InvenWeaponEnhanceText;
    public Text UpgradePosText;
    public Text UpgradeGoldText;
    private int[] UpgradePerPoint = new int [10] { 10, 15, 20, 25, 30, 35, 40, 45, 50, 100 };
    private int[] UpgradaePosibility = new int[11] { 90, 80, 70, 60, 50, 40, 30, 20, 10, 5, 0 };
    private int[] UpgradeGoldArray = new int[11] { 100, 200, 300, 400, 500, 600, 700, 800, 900, 1000, 0 };

    void Start()
    {
        if (upgradePanel != null)
        {
            //Debug.Log("Awake");
            upgradePanel.SetActive(false);
        }

        // 게임 시작 시 저장된 값 불러오기
        WeaponEnhancePoint = PlayerPrefs.GetInt("WeaponEnhancePoint", 0); // 기본값 0
        WeaponATK = PlayerPrefs.GetInt("WeaponATK", 20); // 기본값 20
        Attack = PlayerPrefs.GetInt("Attack", 40); // 기본값 40
        UpgradePos = PlayerPrefs.GetInt("UpgradePos", 90); // 기본값 90
        UpgradeGold = PlayerPrefs.GetInt("UpgradeGold", 100); // 기본값 100

        //Debug.Log("Game data loaded.");

        InvenWeaponEnhanceText.text = "+" + WeaponEnhancePoint.ToString();
        WeaponEnhanceText.text = "+" + WeaponEnhancePoint.ToString();
        UpgradePosText.text = UpgradePos.ToString() + "%";
        UpgradeGoldText.text = UpgradeGold.ToString() + " / " + playerstats.Gold.ToString();

        // UI 강제 업데이트
        Canvas.ForceUpdateCanvases();
    }
    /*
    private void Update()
    {
        InvenWeaponEnhanceText.text = "+" + WeaponEnhancePoint.ToString();
        WeaponEnhanceText.text = "+" + WeaponEnhancePoint.ToString();
        UpgradePosText.text = UpgradePos.ToString() + "%";
        UpgradeGoldText.text = UpgradeGold.ToString() + " / " + playerstats.Gold.ToString();
    }
    */
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

    public void WeaponUpgrade()
    {
        if (WeaponEnhancePoint >= UpgradeGoldArray.Length || playerstats.Gold < UpgradeGoldArray[WeaponEnhancePoint])
        {
            Debug.Log("강화 불가능: 강화 포인트가 최대이거나 골드가 부족합니다.");
            return;
        }

        // 강화 비용 차감
        playerstats.Gold -= UpgradeGoldArray[WeaponEnhancePoint];

        // 강화 성공 여부 결정
        bool isUpgradeSuccessful = Random.Range(0, 100) < UpgradaePosibility[WeaponEnhancePoint];
        if (isUpgradeSuccessful)
        {
            WeaponEnhancePoint++;
            Debug.Log(UpgradaePosibility[WeaponEnhancePoint - 1] + "% 의 확률을 뚫음");
            WeaponATK = swordEft.SwordAttackPoint + UpgradePerPoint[WeaponEnhancePoint - 1];
            Attack = PlayerAttack + WeaponATK;

            Debug.Log("무기 " + WeaponEnhancePoint + "강");
        }
        else
        {
            Debug.Log("강화 실패");
        }

        // 강화 확률과 비용 정보 업데이트
        if (WeaponEnhancePoint < UpgradeGoldArray.Length)
        {
            UpgradePos = UpgradaePosibility[WeaponEnhancePoint];
            UpgradeGold = UpgradeGoldArray[WeaponEnhancePoint];
        }
        else
        {
            UpgradePos = 0; // 강화가 최대치에 도달했을 때 확률 표시를 0으로 설정
            UpgradeGold = 0; // 비용도 0으로 설정
        }

        // 강화 포인트 저장
        SaveWeaponEnhancePoint();
    }



    public void SaveWeaponEnhancePoint()
    {
        playerstats.OnApplicationQuit();

        PlayerPrefs.SetInt("WeaponEnhancePoint", WeaponEnhancePoint);
        PlayerPrefs.SetInt("WeaponATK", WeaponATK);
        PlayerPrefs.SetInt("Attack", Attack);
        PlayerPrefs.SetInt("UpgradePos", UpgradePos);
        PlayerPrefs.SetInt("UpgradeGold", UpgradeGold);
        PlayerPrefs.Save(); // 변경사항 저장
        //Debug.Log("Game data saved.");

        InvenWeaponEnhanceText.text = "+" + WeaponEnhancePoint.ToString();
        WeaponEnhanceText.text = "+" + WeaponEnhancePoint.ToString();
        UpgradePosText.text = UpgradePos.ToString() + "%";
        UpgradeGoldText.text = UpgradeGold.ToString() + " / " + playerstats.Gold.ToString();

        // UI 강제 업데이트
        Canvas.ForceUpdateCanvases();
    }


    public void ResetUpgrade()
    {
        WeaponEnhancePoint = 0;
        WeaponATK = 20;
        Attack = 40;
        UpgradePos = 90;
        UpgradeGold = 100;
        SaveWeaponEnhancePoint();
    }
}
