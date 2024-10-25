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

        // ���� ���� �� ����� �� �ҷ�����
        WeaponEnhancePoint = PlayerPrefs.GetInt("WeaponEnhancePoint", 0); // �⺻�� 0
        WeaponATK = PlayerPrefs.GetInt("WeaponATK", 20); // �⺻�� 20
        Attack = PlayerPrefs.GetInt("Attack", 40); // �⺻�� 40
        UpgradePos = PlayerPrefs.GetInt("UpgradePos", 90); // �⺻�� 90
        UpgradeGold = PlayerPrefs.GetInt("UpgradeGold", 100); // �⺻�� 100

        //Debug.Log("Game data loaded.");

        InvenWeaponEnhanceText.text = "+" + WeaponEnhancePoint.ToString();
        WeaponEnhanceText.text = "+" + WeaponEnhancePoint.ToString();
        UpgradePosText.text = UpgradePos.ToString() + "%";
        UpgradeGoldText.text = UpgradeGold.ToString() + " / " + playerstats.Gold.ToString();

        // UI ���� ������Ʈ
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
            Debug.Log("��ȭ �Ұ���: ��ȭ ����Ʈ�� �ִ��̰ų� ��尡 �����մϴ�.");
            return;
        }

        // ��ȭ ��� ����
        playerstats.Gold -= UpgradeGoldArray[WeaponEnhancePoint];

        // ��ȭ ���� ���� ����
        bool isUpgradeSuccessful = Random.Range(0, 100) < UpgradaePosibility[WeaponEnhancePoint];
        if (isUpgradeSuccessful)
        {
            WeaponEnhancePoint++;
            Debug.Log(UpgradaePosibility[WeaponEnhancePoint - 1] + "% �� Ȯ���� ����");
            WeaponATK = swordEft.SwordAttackPoint + UpgradePerPoint[WeaponEnhancePoint - 1];
            Attack = PlayerAttack + WeaponATK;

            Debug.Log("���� " + WeaponEnhancePoint + "��");
        }
        else
        {
            Debug.Log("��ȭ ����");
        }

        // ��ȭ Ȯ���� ��� ���� ������Ʈ
        if (WeaponEnhancePoint < UpgradeGoldArray.Length)
        {
            UpgradePos = UpgradaePosibility[WeaponEnhancePoint];
            UpgradeGold = UpgradeGoldArray[WeaponEnhancePoint];
        }
        else
        {
            UpgradePos = 0; // ��ȭ�� �ִ�ġ�� �������� �� Ȯ�� ǥ�ø� 0���� ����
            UpgradeGold = 0; // ��뵵 0���� ����
        }

        // ��ȭ ����Ʈ ����
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
        PlayerPrefs.Save(); // ������� ����
        //Debug.Log("Game data saved.");

        InvenWeaponEnhanceText.text = "+" + WeaponEnhancePoint.ToString();
        WeaponEnhanceText.text = "+" + WeaponEnhancePoint.ToString();
        UpgradePosText.text = UpgradePos.ToString() + "%";
        UpgradeGoldText.text = UpgradeGold.ToString() + " / " + playerstats.Gold.ToString();

        // UI ���� ������Ʈ
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
