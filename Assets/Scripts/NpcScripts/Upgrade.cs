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

        // ���� ���� �� ����� �� �ҷ�����
        WeaponEnhancePoint = PlayerPrefs.GetInt("WeaponEnhancePoint", 0); // �⺻�� 0
        WeaponATK = PlayerPrefs.GetInt("WeaponATK", 20); // �⺻�� 20
        Attack = PlayerPrefs.GetInt("Attack", 40); // �⺻�� 40
        UpgradePos = PlayerPrefs.GetInt("UpgradePos", 90); // �⺻�� 90
        UpgradeGold = PlayerPrefs.GetInt("UpgradeGold", 100); // �⺻�� 100

        Debug.Log("Game data loaded.");
    }

    private void Update()
    {
        InvenWeaponEnhanceText.text = "+" + WeaponEnhancePoint.ToString();
        WeaponEnhanceText.text = "+" + WeaponEnhancePoint.ToString();
        UpgradePosText.text = UpgradePos.ToString() + "%";
        UpgradeGoldText.text = UpgradeGold.ToString();
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

    public void WeaponUpgrade()
    {
        if (WeaponEnhancePoint == 0)
        {

            if (Random.Range(0, 100) < UpgradaePosibility[WeaponEnhancePoint]) // 90% Ȯ��
            {
                Debug.Log(UpgradaePosibility[WeaponEnhancePoint] + "% �� Ȯ���� ����");
                WeaponEnhancePoint = 1;
                WeaponATK = swordEft.SwordAttackPoint + UpgradePerPoint[WeaponEnhancePoint - 1];
                Attack = PlayerAttack + swordEft.SwordAttackPoint + UpgradePerPoint[WeaponEnhancePoint - 1];
                UpgradePos = UpgradaePosibility[WeaponEnhancePoint];
                UpgradeGold = UpgradeGoldArray[WeaponEnhancePoint];
                Debug.Log("���� 1��");
                
            }
            else
            {
                Debug.Log("��ȭ ����");
            }
        }

        else if (WeaponEnhancePoint == 1)
        {

            if (Random.Range(0, 100) < UpgradaePosibility[WeaponEnhancePoint]) // 80% Ȯ��
            {
                Debug.Log(UpgradaePosibility[WeaponEnhancePoint] + "% �� Ȯ���� ����");
                WeaponEnhancePoint = 2;
                WeaponATK = swordEft.SwordAttackPoint + UpgradePerPoint[WeaponEnhancePoint - 1];
                Attack = PlayerAttack + swordEft.SwordAttackPoint + UpgradePerPoint[WeaponEnhancePoint - 1];
                UpgradePos = UpgradaePosibility[WeaponEnhancePoint];
                UpgradeGold = UpgradeGoldArray[WeaponEnhancePoint];
                Debug.Log("���� 2��");
            }
            else
            {
                Debug.Log("��ȭ ����");
            }
        }

        else if (WeaponEnhancePoint == 2)
        {
            // 90% Ȯ���� ���׷��̵�
            if (Random.Range(0, 100) < UpgradaePosibility[WeaponEnhancePoint]) // 70% Ȯ��
            {
                Debug.Log(UpgradaePosibility[WeaponEnhancePoint] + "% �� Ȯ���� ����");
                WeaponEnhancePoint = 3;
                WeaponATK = swordEft.SwordAttackPoint + UpgradePerPoint[WeaponEnhancePoint - 1];
                Attack = PlayerAttack + swordEft.SwordAttackPoint + UpgradePerPoint[WeaponEnhancePoint - 1];
                UpgradePos = UpgradaePosibility[WeaponEnhancePoint];
                UpgradeGold = UpgradeGoldArray[WeaponEnhancePoint];
                Debug.Log("���� 3��");
            }
            else
            {
                Debug.Log("��ȭ ����");
            }
        }

        else if (WeaponEnhancePoint == 3)
        {
            // 90% Ȯ���� ���׷��̵�
            if (Random.Range(0, 100) < UpgradaePosibility[WeaponEnhancePoint]) // 50% Ȯ��
            {
                Debug.Log(UpgradaePosibility[WeaponEnhancePoint] + "% �� Ȯ���� ����");
                WeaponEnhancePoint = 4;
                WeaponATK = swordEft.SwordAttackPoint + UpgradePerPoint[WeaponEnhancePoint - 1];
                Attack = PlayerAttack + swordEft.SwordAttackPoint + UpgradePerPoint[WeaponEnhancePoint - 1];
                UpgradePos = UpgradaePosibility[WeaponEnhancePoint];
                UpgradeGold = UpgradeGoldArray[WeaponEnhancePoint];
                Debug.Log("���� 4��");
            }
            else
            {
                Debug.Log("��ȭ ����");
            }
        }

        else if (WeaponEnhancePoint == 4)
        {
            // 90% Ȯ���� ���׷��̵�
            if (Random.Range(0, 100) < UpgradaePosibility[WeaponEnhancePoint]) // 40% Ȯ��
            {
                Debug.Log(UpgradaePosibility[WeaponEnhancePoint] + "% �� Ȯ���� ����");
                WeaponEnhancePoint = 5;
                WeaponATK = swordEft.SwordAttackPoint + UpgradePerPoint[WeaponEnhancePoint - 1];
                Attack = PlayerAttack + swordEft.SwordAttackPoint + UpgradePerPoint[WeaponEnhancePoint - 1];
                UpgradePos = UpgradaePosibility[WeaponEnhancePoint];
                UpgradeGold = UpgradeGoldArray[WeaponEnhancePoint];
                Debug.Log("���� 5��");
            }
            else
            {
                Debug.Log("��ȭ ����");
            }
        }

        else if (WeaponEnhancePoint == 5)
        {
            // 90% Ȯ���� ���׷��̵�
            if (Random.Range(0, 100) < UpgradaePosibility[WeaponEnhancePoint]) // 30% Ȯ��
            {
                Debug.Log(UpgradaePosibility[WeaponEnhancePoint] + "% �� Ȯ���� ����");
                WeaponEnhancePoint = 6;
                WeaponATK = swordEft.SwordAttackPoint + UpgradePerPoint[WeaponEnhancePoint - 1];
                Attack = PlayerAttack + swordEft.SwordAttackPoint + UpgradePerPoint[WeaponEnhancePoint - 1];
                UpgradePos = UpgradaePosibility[WeaponEnhancePoint];
                UpgradeGold = UpgradeGoldArray[WeaponEnhancePoint];
                Debug.Log("���� 6��");
            }
            else
            {
                Debug.Log("��ȭ ����");
            }
        }

        else if (WeaponEnhancePoint == 6)
        {
            // 90% Ȯ���� ���׷��̵�
            if (Random.Range(0, 100) < UpgradaePosibility[WeaponEnhancePoint]) // 20% Ȯ��
            {
                Debug.Log(UpgradaePosibility[WeaponEnhancePoint] + "% �� Ȯ���� ����");
                WeaponEnhancePoint = 7;
                WeaponATK = swordEft.SwordAttackPoint + UpgradePerPoint[WeaponEnhancePoint - 1];
                Attack = PlayerAttack + swordEft.SwordAttackPoint + UpgradePerPoint[WeaponEnhancePoint - 1];
                UpgradePos = UpgradaePosibility[WeaponEnhancePoint];
                UpgradeGold = UpgradeGoldArray[WeaponEnhancePoint];
                Debug.Log("���� 7��");
            }
            else
            {
                Debug.Log("��ȭ ����");
            }
        }

        else if (WeaponEnhancePoint == 7)
        {
            // 90% Ȯ���� ���׷��̵�
            if (Random.Range(0, 100) < UpgradaePosibility[WeaponEnhancePoint]) // 10% Ȯ��
            {
                Debug.Log(UpgradaePosibility[WeaponEnhancePoint] + "% �� Ȯ���� ����");
                WeaponEnhancePoint = 8;
                WeaponATK = swordEft.SwordAttackPoint + UpgradePerPoint[WeaponEnhancePoint - 1];
                Attack = PlayerAttack + swordEft.SwordAttackPoint + UpgradePerPoint[WeaponEnhancePoint - 1];
                UpgradePos = UpgradaePosibility[WeaponEnhancePoint];
                UpgradeGold = UpgradeGoldArray[WeaponEnhancePoint];
                Debug.Log("���� 8��");
            }
            else
            {
                Debug.Log("��ȭ ����");
            }
        }

        // ���׷��̵� �� ���� ����
        SaveWeaponEnhancePoint();
    }

    public void SaveWeaponEnhancePoint()
    {
        PlayerPrefs.SetInt("WeaponEnhancePoint", WeaponEnhancePoint);
        PlayerPrefs.SetInt("WeaponATK", WeaponATK);
        PlayerPrefs.SetInt("Attack", Attack);
        PlayerPrefs.SetInt("UpgradePos", UpgradePos);
        PlayerPrefs.SetInt("UpgradeGold", UpgradeGold);
        PlayerPrefs.Save(); // ������� ����
        Debug.Log("Game data saved.");
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
