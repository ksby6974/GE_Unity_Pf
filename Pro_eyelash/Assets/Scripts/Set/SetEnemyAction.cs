using chataan.Scripts.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace chataan.Scripts.EnemyAction
{
    // ����������������������������������������������������
    // �� �ൿ ����
    // ����������������������������������������������������
    public static class SetEnemyAction
    {
        private static readonly Dictionary<EnemyActionType, EnemyActionBase> EnemyActionDict = new Dictionary<EnemyActionType, EnemyActionBase>();

        // �ʱ�ȭ ����
        public static bool IsInitialized { get; private set; }

        // �ʱ�ȭ
        public static void Initialize()
        {
            EnemyActionDict.Clear();

            var allEnemyActions = Assembly.GetAssembly(typeof(EnemyActionBase)).GetTypes().Where(t => typeof(EnemyActionBase).IsAssignableFrom(t) && t.IsAbstract == false);

            foreach (var enemyAction in allEnemyActions)
            {
                // �ൿ�ߴ°�?
                EnemyActionBase action = Activator.CreateInstance(enemyAction) as EnemyActionBase;
                if (action != null)
                {
                    EnemyActionDict.Add(action.ActionType, action);
                }
            }

            IsInitialized = true;
        }

        // �Ϸ��� ���� �ൿ
        public static EnemyActionBase GetAction(EnemyActionType targetAction) => EnemyActionDict[targetAction];
    }
}
