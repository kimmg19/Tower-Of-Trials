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
    private int[] UpgradePerPoint = new int [8] { 10, 20, 30, 40, 50, 60, 70, 80 };
    private int[] UpgradaePosibility = new int[9] { 90, 80, 70, 50, 40, 30, 20, 10, 0 };
    private int[] UpgradeGoldArray = new int[9] { 100, 200, 300, 400, 500, 600, 700, 800, 0 };

    void Start()
    {
        if (upgradePanel != null)
        {
            Debug.Log("Awake");
            upgradePanel.SetActive(false);
        }

        // 게임 시작 시 저장된 값 불러오기
        WeaponEnhancePoint = PlayerPrefs.GetInt("WeaponEnhancePoint", 0); // 기본값 0
        WeaponATK = PlayerPrefs.GetInt("WeaponATK", 20); // 기본값 20
        Attack = PlayerPrefs.GetInt("Attack", 40); // 기본값 40
        UpgradePos = PlayerPrefs.GetInt("UpgradePos", 90); // 기본값 90
        UpgradeGold = PlayerPrefs.GetInt("UpgradeGold", 100); // 기본값 100

        Debug.Log("Game data loaded.");

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
        if (WeaponEnhancePoint == 0 && playerstats.Gold >= UpgradeGoldArray[WeaponEnhancePoint])
        {

            if (Random.Range(0, 100) < UpgradaePosibility[WeaponEnhancePoint]) // 90% 확률
            {
                playerstats.Gold -= UpgradeGoldArray[WeaponEnhancePoint];
                Debug.Log(UpgradaePosibility[WeaponEnhancePoint] + "% 의 확률을 뚫음");
                WeaponEnhancePoint = 1;
                WeaponATK = swordEft.SwordAttackPoint + UpgradePerPoint[WeaponEnhancePoint - 1];
                Attack = PlayerAttack + swordEft.SwordAttackPoint + UpgradePerPoint[WeaponEnhancePoint - 1];
                UpgradePos = UpgradaePosibility[WeaponEnhancePoint];
                UpgradeGold = UpgradeGoldArray[WeaponEnhancePoint];
                Debug.Log("무기 1강");

                SaveWeaponEnhancePoint(); // 강화 성공 시 즉시 저장
            }
            else
            {
                playerstats.Gold -= UpgradeGoldArray[WeaponEnhancePoint];
                Debug.Log("강화 실패");

                SaveWeaponEnhancePoint(); // 강화 성공 시 즉시 저장
            }
        }

        else if (WeaponEnhancePoint == 1 && playerstats.Gold >= UpgradeGoldArray[WeaponEnhancePoint])
        {

            if (Random.Range(0, 100) < UpgradaePosibility[WeaponEnhancePoint]) // 80% 확률
            {
                playerstats.Gold -= UpgradeGoldArray[WeaponEnhancePoint];
                Debug.Log(UpgradaePosibility[WeaponEnhancePoint] + "% 의 확률을 뚫음");
                WeaponEnhancePoint = 2;
                WeaponATK = swordEft.SwordAttackPoint + UpgradePerPoint[WeaponEnhancePoint - 1];
                Attack = PlayerAttack + swordEft.SwordAttackPoint + UpgradePerPoint[WeaponEnhancePoint - 1];
                UpgradePos = UpgradaePosibility[WeaponEnhancePoint];
                UpgradeGold = UpgradeGoldArray[WeaponEnhancePoint];
                Debug.Log("무기 2강");

                SaveWeaponEnhancePoint(); // 강화 성공 시 즉시 저장
            }
            else
            {
                playerstats.Gold -= UpgradeGoldArray[WeaponEnhancePoint];
                Debug.Log("강화 실패");

                SaveWeaponEnhancePoint(); // 강화 성공 시 즉시 저장
            }
        }

        else if (WeaponEnhancePoint == 2 && playerstats.Gold >= UpgradeGoldArray[WeaponEnhancePoint])
        {
            if (Random.Range(0, 100) < UpgradaePosibility[WeaponEnhancePoint]) // 70% 확률
            {
                playerstats.Gold -= UpgradeGoldArray[WeaponEnhancePoint];
                Debug.Log(UpgradaePosibility[WeaponEnhancePoint] + "% 의 확률을 뚫음");
                WeaponEnhancePoint = 3;
                WeaponATK = swordEft.SwordAttackPoint + UpgradePerPoint[WeaponEnhancePoint - 1];
                Attack = PlayerAttack + swordEft.SwordAttackPoint + UpgradePerPoint[WeaponEnhancePoint - 1];
                UpgradePos = UpgradaePosibility[WeaponEnhancePoint];
                UpgradeGold = UpgradeGoldArray[WeaponEnhancePoint];
                Debug.Log("무기 3강");

                SaveWeaponEnhancePoint(); // 강화 성공 시 즉시 저장
            }
            else
            {
                playerstats.Gold -= UpgradeGoldArray[WeaponEnhancePoint];
                Debug.Log("강화 실패");

                SaveWeaponEnhancePoint(); // 강화 성공 시 즉시 저장
            }
        }

        else if (WeaponEnhancePoint == 3 && playerstats.Gold >= UpgradeGoldArray[WeaponEnhancePoint])
        {
            if (Random.Range(0, 100) < UpgradaePosibility[WeaponEnhancePoint]) // 50% 확률
            {
                playerstats.Gold -= UpgradeGoldArray[WeaponEnhancePoint];
                Debug.Log(UpgradaePosibility[WeaponEnhancePoint] + "% 의 확률을 뚫음");
                WeaponEnhancePoint = 4;
                WeaponATK = swordEft.SwordAttackPoint + UpgradePerPoint[WeaponEnhancePoint - 1];
                Attack = PlayerAttack + swordEft.SwordAttackPoint + UpgradePerPoint[WeaponEnhancePoint - 1];
                UpgradePos = UpgradaePosibility[WeaponEnhancePoint];
                UpgradeGold = UpgradeGoldArray[WeaponEnhancePoint];
                Debug.Log("무기 4강");

                SaveWeaponEnhancePoint(); // 강화 성공 시 즉시 저장
            }
            else
            {
                playerstats.Gold -= UpgradeGoldArray[WeaponEnhancePoint];
                Debug.Log("강화 실패");

                SaveWeaponEnhancePoint(); // 강화 성공 시 즉시 저장
            }
        }

        else if (WeaponEnhancePoint == 4 && playerstats.Gold >= UpgradeGoldArray[WeaponEnhancePoint])
        {
            if (Random.Range(0, 100) < UpgradaePosibility[WeaponEnhancePoint]) // 40% 확률
            {
                playerstats.Gold -= UpgradeGoldArray[WeaponEnhancePoint];
                Debug.Log(UpgradaePosibility[WeaponEnhancePoint] + "% 의 확률을 뚫음");
                WeaponEnhancePoint = 5;
                WeaponATK = swordEft.SwordAttackPoint + UpgradePerPoint[WeaponEnhancePoint - 1];
                Attack = PlayerAttack + swordEft.SwordAttackPoint + UpgradePerPoint[WeaponEnhancePoint - 1];
                UpgradePos = UpgradaePosibility[WeaponEnhancePoint];
                UpgradeGold = UpgradeGoldArray[WeaponEnhancePoint];
                Debug.Log("무기 5강");

                SaveWeaponEnhancePoint(); // 강화 성공 시 즉시 저장
            }
            else
            {
                playerstats.Gold -= UpgradeGoldArray[WeaponEnhancePoint];
                Debug.Log("강화 실패");

                SaveWeaponEnhancePoint(); // 강화 성공 시 즉시 저장
            }
        }

        else if (WeaponEnhancePoint == 5 && playerstats.Gold >= UpgradeGoldArray[WeaponEnhancePoint])
        {
            if (Random.Range(0, 100) < UpgradaePosibility[WeaponEnhancePoint]) // 30% 확률
            {
                playerstats.Gold -= UpgradeGoldArray[WeaponEnhancePoint];
                Debug.Log(UpgradaePosibility[WeaponEnhancePoint] + "% 의 확률을 뚫음");
                WeaponEnhancePoint = 6;
                WeaponATK = swordEft.SwordAttackPoint + UpgradePerPoint[WeaponEnhancePoint - 1];
                Attack = PlayerAttack + swordEft.SwordAttackPoint + UpgradePerPoint[WeaponEnhancePoint - 1];
                UpgradePos = UpgradaePosibility[WeaponEnhancePoint];
                UpgradeGold = UpgradeGoldArray[WeaponEnhancePoint];
                Debug.Log("무기 6강");

                SaveWeaponEnhancePoint(); // 강화 성공 시 즉시 저장
            }
            else
            {
                playerstats.Gold -= UpgradeGoldArray[WeaponEnhancePoint];
                Debug.Log("강화 실패");
                SaveWeaponEnhancePoint(); // 강화 성공 시 즉시 저장
            }
        }

        else if (WeaponEnhancePoint == 6 && playerstats.Gold >= UpgradeGoldArray[WeaponEnhancePoint])
        {
            if (Random.Range(0, 100) < UpgradaePosibility[WeaponEnhancePoint]) // 20% 확률
            {
                playerstats.Gold -= UpgradeGoldArray[WeaponEnhancePoint];
                Debug.Log(UpgradaePosibility[WeaponEnhancePoint] + "% 의 확률을 뚫음");
                WeaponEnhancePoint = 7;
                WeaponATK = swordEft.SwordAttackPoint + UpgradePerPoint[WeaponEnhancePoint - 1];
                Attack = PlayerAttack + swordEft.SwordAttackPoint + UpgradePerPoint[WeaponEnhancePoint - 1];
                UpgradePos = UpgradaePosibility[WeaponEnhancePoint];
                UpgradeGold = UpgradeGoldArray[WeaponEnhancePoint];
                Debug.Log("무기 7강");

                SaveWeaponEnhancePoint(); // 강화 성공 시 즉시 저장
            }
            else
            {
                playerstats.Gold -= UpgradeGoldArray[WeaponEnhancePoint];
                Debug.Log("강화 실패");
                SaveWeaponEnhancePoint(); // 강화 성공 시 즉시 저장
            }
        }

        else if (WeaponEnhancePoint == 7 && playerstats.Gold >= UpgradeGoldArray[WeaponEnhancePoint])
        {
            if (Random.Range(0, 100) < UpgradaePosibility[WeaponEnhancePoint]) // 10% 확률
            {
                playerstats.Gold -= UpgradeGoldArray[WeaponEnhancePoint];
                Debug.Log(UpgradaePosibility[WeaponEnhancePoint] + "% 의 확률을 뚫음");
                WeaponEnhancePoint = 8;
                WeaponATK = swordEft.SwordAttackPoint + UpgradePerPoint[WeaponEnhancePoint - 1];
                Attack = PlayerAttack + swordEft.SwordAttackPoint + UpgradePerPoint[WeaponEnhancePoint - 1];
                UpgradePos = UpgradaePosibility[WeaponEnhancePoint];
                UpgradeGold = UpgradeGoldArray[WeaponEnhancePoint];
                Debug.Log("무기 8강");

                SaveWeaponEnhancePoint(); // 강화 성공 시 즉시 저장
            }
            else
            {
                playerstats.Gold -= UpgradeGoldArray[WeaponEnhancePoint];
                Debug.Log("강화 실패");
                SaveWeaponEnhancePoint(); // 강화 성공 시 즉시 저장
            }
        }

        // 업그레이드 후 값을 저장
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
