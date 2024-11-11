using chataan.Scripts.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace chataan.Scripts.Card
{
    // ����������������������������������������������������
    // ī�� ȿ�� ����
    // ����������������������������������������������������
    public static class SetCardAction
    {
        // ��ųʸ�
        private static readonly Dictionary<CardEffectType, CardActionBase> CardActionDict = new Dictionary<CardEffectType, CardActionBase>();

        public static bool IsInitialized { get; private set; }

        // ��������������������������������������������������
        // �ʱ�ȭ
        // ��������������������������������������������������
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

        // ��������������������������������������������������
        // ī�� ȿ�� ���
        // ��������������������������������������������������
        public static CardActionBase GetAction(CardEffectType targetAction) => CardActionDict[targetAction];

    }
}
