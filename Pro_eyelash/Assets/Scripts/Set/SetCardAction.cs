using chataan.Scripts.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace chataan.Scripts.Card
{
    // ━━━━━━━━━━━━━━━━━━━━━━━━━━
    // 카드 효과 설정
    // ━━━━━━━━━━━━━━━━━━━━━━━━━━
    public static class SetCardAction
    {
        // 딕셔너리
        private static readonly Dictionary<CardEffectType, CardActionBase> CardActionDict = new Dictionary<CardEffectType, CardActionBase>();

        public static bool IsInitialized { get; private set; }

        // ─────────────────────────
        // 초기화
        // ─────────────────────────
        public static void Initialize()
        {
            CardActionDict.Clear();

            var allActionCards = Assembly.GetAssembly(typeof(CardActionBase)).GetTypes().Where(t => typeof(CardActionBase).IsAssignableFrom(t) && t.IsAbstract == false);

            foreach (var actionCard in allActionCards)
            {
                CardActionBase action = Activator.CreateInstance(actionCard) as CardActionBase;
                if (action != null)
                {
                    CardActionDict.Add(action.ActionType, action);
                }
            }

            IsInitialized = true;
        }

        // ─────────────────────────
        // 카드 효과 얻기
        // ─────────────────────────
        public static CardActionBase GetAction(CardEffectType targetAction) => CardActionDict[targetAction];

    }
}
