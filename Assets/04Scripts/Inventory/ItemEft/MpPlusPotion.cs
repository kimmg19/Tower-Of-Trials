using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemEft/Consumable/MpPotionPlus")]
public class MpPlusPotionEft : ItemEffect
{
    public int MpPlusPoint = 0; // Hp ���� ȸ���� ���� ��ġ
    public override bool ExecuteRole(PlayerStats playerStats)
    {
        if (playerStats != null)
        {
            if (playerStats.MpPotionRate < 50)
            {
                playerStats.MpPotionRate += MpPlusPoint;
                //playerStats.currentMp = Mathf.Clamp(playerStats.currentMp, 0, playerStats.maxMp); // �ִ� ������ �ʰ����� �ʵ��� ����
                Debug.Log("Mp Potion ȸ���� ����: " + MpPlusPoint + "%" + "�� ȸ����: " + playerStats.MpPotionRate);
                return true;
            }
            else
            {
                Debug.Log("Mp Potion ȸ������ �̹� 50��");
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