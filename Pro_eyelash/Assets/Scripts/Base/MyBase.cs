using chataan.Scripts.Data.Chara;
using chataan.Scripts.Interface;
using System;
using System.Collections;
using UnityEngine;

namespace chataan.Scripts.Chara
{
    // ━━━━━━━━━━━━━━━━━━━━━━━━━━
    // 플레이어 객체
    // ━━━━━━━━━━━━━━━━━━━━━━━━━━
    public abstract class MyBase : CharaBase, IMy
    {
        [Header("Settings")]
        [SerializeField] private MyCanvas myCanvas;
        [SerializeField] private MyCharaData myCharaData;
        public MyCanvas MyCanvas => myCanvas;
        public MyCharaData MyCharaData => myCharaData;

        // ─────────────────────────
        // 플레이어 생성
        // ─────────────────────────
        public override void BuildCharacter()
        {
            base.BuildCharacter();
            myCanvas.InitCanvas();
            CharacterStats = new CharacterStats(myCharaData.MaxHealth, myCanvas);

            if (!CoreManager)
            {
                throw new Exception("CoreManager?");
            }

            var data = CoreManager.SavePlayData.AllyHealthDataList.Find(x => x.CharacterId == MyCharaData.CharacterID);

            if (data != null)
            {
                CharacterStats.CurrentHealth = data.CurrentHealth;
                CharacterStats.MaxHealth = data.MaxHealth;
            }
            else
            {
                CoreManager.SavePlayData.SetAllyHealthData(MyCharaData.CharacterID, CharacterStats.CurrentHealth, CharacterStats.MaxHealth);
            }

            CharacterStats.OnDeath += OnDeath;
            CharacterStats.SetCurrentHealth(CharacterStats.CurrentHealth);

            if (BattleManager != null)
                BattleManager.OnAllyTurnStarted += CharacterStats.TriggerAllStatus;
        }

        // ─────────────────────────
        // 플레이어 사망
        // ─────────────────────────
        protected override void OnDeath()
        {
            base.OnDeath();
            if (BattleManager != null)
            {
                BattleManager.OnAllyTurnStarted -= CharacterStats.TriggerAllStatus;
                BattleManager.OnAllyDeath(this);
            }

            Destroy(gameObject);
        }
    }

    // ━━━━━━━━━━━━━━━━━━━━━━━━━━
    // 아군 체력 데이터
    // ━━━━━━━━━━━━━━━━━━━━━━━━━━
    [Serializable]
    public class AllyHealthData
    {
        [SerializeField] private string characterId;
        [SerializeField] private int maxHealth;
        [SerializeField] private int currentHealth;

        public int MaxHealth
        {
            get => maxHealth;
            set => maxHealth = value;
        }

        public int CurrentHealth
        {
            get => currentHealth;
            set => currentHealth = value;
        }

        public string CharacterId
        {
            get => characterId;
            set => characterId = value;
        }
    }
}
