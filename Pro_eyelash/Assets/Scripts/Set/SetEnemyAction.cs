using chataan.Scripts.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace chataan.Scripts.EnemyAction
{
    // ━━━━━━━━━━━━━━━━━━━━━━━━━━
    // 적 행동 과정
    // ━━━━━━━━━━━━━━━━━━━━━━━━━━
    public static class SetEnemyAction
    {
        private static readonly Dictionary<EnemyActionType, EnemyActionBase> EnemyActionDict = new Dictionary<EnemyActionType, EnemyActionBase>();

        // 초기화 여부
        public static bool IsInitialized { get; private set; }

        // 초기화
        public static void Initialize()
        {
            EnemyActionDict.Clear();

            var allEnemyActions = Assembly.GetAssembly(typeof(EnemyActionBase)).GetTypes().Where(t => typeof(EnemyActionBase).IsAssignableFrom(t) && t.IsAbstract == false);

            foreach (var enemyAction in allEnemyActions)
            {
                // 행동했는가?
                EnemyActionBase action = Activator.CreateInstance(enemyAction) as EnemyActionBase;
                if (action != null)
                {
                    EnemyActionDict.Add(action.ActionType, action);
                }
            }

            IsInitialized = true;
        }

        // 하려는 적의 행동
        public static EnemyActionBase GetAction(EnemyActionType targetAction) => EnemyActionDict[targetAction];
    }
}
