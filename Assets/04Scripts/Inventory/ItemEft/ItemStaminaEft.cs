using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ItemEft/Consumable/Stamina")]
public class ItemStaminaEft : ItemEffect
{
    public int staminaPoint = 0; // ���¹̳� ȸ�� ��ġ
    public override bool ExecuteRole(PlayerStats playerStats)
    {
        if (playerStats != null)
        {
            if (playerStats.maxStamina < 100)
            {
                playerStats.maxStamina += staminaPoint;
                //playerStats.maxStamina = Mathf.Clamp(playerStats.maxStamina, 50, 100); // �ִ� ���¹̳��� 100���� ����
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
}
