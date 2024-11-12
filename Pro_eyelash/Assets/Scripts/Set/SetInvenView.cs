using chataan.Scripts.Enums;
using chataan.Scripts.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace chataan.Scripts.Utils
{
    // ━━━━━━━━━━━━━━━━━━━━━━━━━━
    // 뽑을 카드 더미, 버린 카드 더미, 소멸 카드 더미 등을 보여줌
    // ━━━━━━━━━━━━━━━━━━━━━━━━━━
    public class SetInvenView : MonoBehaviour
    {
        [SerializeField] private InvenTypes inventoryType;

        private UIManager UIManager => UIManager.Instance;

        public void OpenInventory()
        {
            switch (inventoryType)      
            {        
                // ─────────────────────────
                // 현재
                // ─────────────────────────
                case InvenTypes.CurrentDeck:
                    UIManager.OpenInventory(CoreManager.Instance.SavePlayData.CurrentCardsList, "Current Cards");
                    break;

                // ─────────────────────────
                // 뽑을
                // ─────────────────────────
                case InvenTypes.DrawPile:
                    UIManager.OpenInventory(PlayerManager.Instance.DrawPile, "Draw Pile");
                    break;

                // ─────────────────────────
                // 버린
                // ─────────────────────────
                case InvenTypes.DiscardPile:
                    UIManager.OpenInventory(PlayerManager.Instance.DiscardPile, "Discard Pile");
                    break;

                // ─────────────────────────
                // 고갈
                // ─────────────────────────
                case InvenTypes.ExhaustPile:
                    UIManager.OpenInventory(PlayerManager.Instance.ExhaustPile, "Exhaust Pile");
                    break;

                // ─────────────────────────
                // 그 외
                // ─────────────────────────
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

    }
}
