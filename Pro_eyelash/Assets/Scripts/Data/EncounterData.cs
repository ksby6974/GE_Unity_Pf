using System;
using System.Collections.Generic;
using System.Linq;
using chataan.Scripts.Enums;
using chataan.Scripts.Gets;
using UnityEngine;

namespace chataan.Scripts.Data.Encounter
{
    // ━━━━━━━━━━━━━━━━━━━━━━━━━━
    // 조우 데이터
    // ━━━━━━━━━━━━━━━━━━━━━━━━━━
    [CreateAssetMenu(fileName = "EncounterData", menuName = "Chataan/Containers/EncounterData", order = 4)]
    public class EncounterData : ScriptableObject
    {
        [Header("Settings")]
        [SerializeField] private bool encounterRandomlyAtStage;
        [SerializeField] private List<EnemyEncounterStage> enemyEncounterList;

        public bool EncounterRandomlyAtStage => encounterRandomlyAtStage;
        public List<EnemyEncounterStage> EnemyEncounterList => enemyEncounterList;

        public EnemyEncounter GetEnemyEncounter(int stageId = 0, int encounterId = 0, bool isFinal = false)
        {
            var selectedStage = EnemyEncounterList.First(x => x.StageId == stageId);
            if (isFinal) return selectedStage.BossEncounterList.GetRandomItem();

            return EncounterRandomlyAtStage
                ? selectedStage.EnemyEncounterList.GetRandomItem()
                : selectedStage.EnemyEncounterList[encounterId] ?? selectedStage.EnemyEncounterList.GetRandomItem();
        }

    }

    // ━━━━━━━━━━━━━━━━━━━━━━━━━━
    // 조우 스테이지
    // ━━━━━━━━━━━━━━━━━━━━━━━━━━
    [Serializable]
    public class EnemyEncounterStage
    {
        [SerializeField] private string name;
        [SerializeField] private int stageId;
        [SerializeField] private List<EnemyEncounter> bossEncounterList;
        [SerializeField] private List<EnemyEncounter> enemyEncounterList;
        public string Name => name;
        public int StageId => stageId;
        public List<EnemyEncounter> BossEncounterList => bossEncounterList;
        public List<EnemyEncounter> EnemyEncounterList => enemyEncounterList;
    }

    // ━━━━━━━━━━━━━━━━━━━━━━━━━━
    // 조우 기본
    // ━━━━━━━━━━━━━━━━━━━━━━━━━━
    [Serializable]
    public class EnemyEncounter : EncounterBase
    {
        //[SerializeField] private List<EnemyCharacterData> enemyList;
        //public List<EnemyCharacterData> EnemyList => enemyList;
    }

    // ━━━━━━━━━━━━━━━━━━━━━━━━━━
    // 조우 기본
    // ━━━━━━━━━━━━━━━━━━━━━━━━━━
    [Serializable]
    public abstract class EncounterBase
    {
        [SerializeField] private BackgroundTypes targetBackgroundType;

        public BackgroundTypes TargetBackgroundType => targetBackgroundType;
    }
}