using chataan.Scripts.Enums;
using chataan.Scripts.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace chataan.Scripts.Utils
{
    // ����������������������������������������������������
    // ���� ī�� ����, ���� ī�� ����, �Ҹ� ī�� ���� ���� ������
    // ����������������������������������������������������
    public class SetInvenView : MonoBehaviour
    {
        [SerializeField] private InvenTypes inventoryType;

        private UIManager UIManager => UIManager.Instance;

        public void OpenInventory()
        {
            switch (inventoryType)      
            {        
                // ��������������������������������������������������
                // ����
                // ��������������������������������������������������
                case InvenTypes.CurrentDeck:
                    UIManager.OpenInventory(CoreManager.Instance.SavePlayData.CurrentCardsList, "Current Cards");
                    break;

                // ��������������������������������������������������
                // ����
                // ��������������������������������������������������
                case InvenTypes.DrawPile:
                    UIManager.OpenInventory(PlayerManager.Instance.DrawPile, "Draw Pile");
                    break;

                // ��������������������������������������������������
                // ����
                // ��������������������������������������������������
                case InvenTypes.DiscardPile:
                    UIManager.OpenInventory(PlayerManager.Instance.DiscardPile, "Discard Pile");
                    break;

                // ��������������������������������������������������
                // ��
                // ��������������������������������������������������
                case InvenTypes.ExhaustPile:
                    UIManager.OpenInventory(PlayerManager.Instance.ExhaustPile, "Exhaust Pile");
                    break;

                // ��������������������������������������������������
                // �� ��
                // ��������������������������������������������������
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

    }
}
