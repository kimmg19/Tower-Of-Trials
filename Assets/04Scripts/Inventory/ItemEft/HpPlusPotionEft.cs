using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemEft/Consumable/HpPotionPlus")]
public class HpPlusPotionEft : ItemEffect
{
    public int HpPlusPoint = 0; // Hp ���� ȸ���� ���� ��ġ
    public override bool ExecuteRole(PlayerStats playerStats)
    {
        if (playerStats != null)
        {
            if (playerStats.HpPotionRate < 50)
            {
                playerStats.HpPotionRate += HpPlusPoint;
                //playerStats.currentMp = Mathf.Clamp(playerStats.currentMp, 0, playerStats.maxMp); // �ִ� ������ �ʰ����� �ʵ��� ����
                Debug.Log("Hp Potion ȸ���� ����: " + HpPlusPoint + "%" + "�� ȸ����: " + playerStats.HpPotionRate);
                return true;
            }
            else
            {
                Debug.Log("Hp Potion ȸ������ �̹� 50��");
                return false;
            }
        }
        else
        {
            Debug.LogError("PlayerStats is null!");
            return false;
        }
    }
}