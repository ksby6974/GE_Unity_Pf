using chataan.Scripts.Chara;
using chataan.Scripts.Data.Containers;
using System;
using System.Collections.Generic;
using chataan.Scripts.Gets;
using UnityEngine;
using Random = UnityEngine.Random;
using chataan.Scripts.Enums;

namespace chataan.Scripts.Data.Chara
{
    // ━━━━━━━━━━━━━━━━━━━━━━━━━━
    // 적 데이터
    // ━━━━━━━━━━━━━━━━━━━━━━━━━━
    [CreateAssetMenu(fileName = "EnemyData", menuName = "chataan/Chara/Enemy", order = 1)]
    public class EnemyData : CharaDataBase
    {
        [Header("Settings")]
        [SerializeField] private EnemyBase enemyPrefab;
        [SerializeField] private bool followAbilityPattern;
        [SerializeField] private List<EnemyAbilityData> enemyAbilityList;
        public List<EnemyAbilityData> EnemyAbilityList => enemyAbilityList;

        public EnemyBase EnemyPrefab => enemyPrefab;

        // ─────────────────────────
        // 능력 차례대로
        // ─────────────────────────
        public EnemyAbilityData GetAbility()
        {
            return EnemyAbilityList.GetRandomItem();
        }

        // ─────────────────────────
        // 특정 능력
        // ─────────────────────────
        public EnemyAbilityData GetAbility(int usedAbilityCount)
        {
            if (followAbilityPattern)
            {
                var index = usedAbilityCount % EnemyAbilityList.Count;
                return EnemyAbilityList[index];
            }

            return GetAbility();
        }
    }

    // ━━━━━━━━━━━━━━━━━━━━━━━━━━
    // 적 능력 데이터
    // ━━━━━━━━━━━━━━━━━━━━━━━━━━
    [Serializable]
    public class EnemyAbilityData
    {
        [Header("Settings")]
        [SerializeField] private string name;
        [SerializeField] private EnemyIntentionData intention;
        [SerializeField] private bool hideActionValue;
        [SerializeField] private List<EnemyActionData> actionList;
        public string Name => name;
        public EnemyIntentionData Intention => intention;
        public List<EnemyActionData> ActionList => actionList;
        public bool HideActionValue => hideActionValue;
    }

    // ━━━━━━━━━━━━━━━━━━━━━━━━━━
    // 적 행동 데이터
    // ━━━━━━━━━━━━━━━━━━━━━━━━━━
    [Serializable]
    public class EnemyActionData
    {
        [SerializeField] private EnemyActionType actionType;
        [SerializeField] private int minActionValue;
        [SerializeField] private int maxActionValue;
        public EnemyActionType ActionType => actionType;
        public int ActionValue => Random.Range(minActionValue, maxActionValue);

    }
}
