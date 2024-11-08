using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using chattan.Scripts.Enums;
using chataan.Scripts.Managers;

namespace chataan.Scripts.Battle
{
    public class BattleManager : MonoBehaviour
    {
        private BattleManager() { }

        public static BattleManager Instance { get; private set; }

        [Header("References")]
       // [SerializeField] private BackgroundContainer backgroundContainer; // 배경 이미지
        [SerializeField] private List<Transform> enemyPosList;  // 적 위치
        [SerializeField] private List<Transform> allyPosList;   // 아군 위치

        //public List<EnemyBase> CurrentEnemiesList { get; private set; } = new List<EnemyBase>();
        //public List<AllyBase> CurrentAlliesList { get; private set; } = new List<AllyBase>();

        public Action OnAllyTurnStarted;
        public Action OnEnemyTurnStarted;
        public List<Transform> EnemyPosList => enemyPosList;

        public List<Transform> AllyPosList => allyPosList;

        //public AllyBase CurrentMainAlly => CurrentAlliesList.Count > 0 ? CurrentAlliesList[0] : null;

        //public EnemyEncounter CurrentEncounter { get; private set; }

        public BattlePhaseType CurrentBattlePhaseType
        {
            get => _CurrentBattle;
            private set
            {
                ExecuteBattleState(value);
                _CurrentBattle = value;
            }
        }

        private BattlePhaseType _CurrentBattle;

        protected FxManager FxManager => FxManager.Instance;
        protected SoundManager AudioManager => SoundManager.Instance;
        protected CoreManager GameManager => CoreManager.Instance;
        protected UIManager UIManager => UIManager.Instance;

        //protected CollectionManager CollectionManager => CollectionManager.Instance;

        // 싱글톤 아님
        private void Awake()
        {
            if (Instance)
            {
                Destroy(gameObject);
                return;
            }
            else
            {
                Instance = this;
                CurrentBattlePhaseType = BattlePhaseType.Pre;
                // 처음에는 전투 시작 페이즈부터
            }
        }

        private void Start()
        {
            StartBattle();
        }

        public void StartBattle()
        {
            BuildEnemies();
            BuildAllies();
            backgroundContainer.OpenSelectedBackground();

            CollectionManager.SetGameDeck();

            UIManager.CombatCanvas.gameObject.SetActive(true);
            UIManager.InformationCanvas.gameObject.SetActive(true);
            CurrentCombatStateType = BattlePhaseType.AllyTurn;
        }

        public void ExecuteBattleState(BattlePhaseType phaseType)
        {
            switch (phaseType)
            {
                // 전투 돌입 직전
                case BattlePhaseType.Pre:
                    break;

                // 플레이어 턴
                case BattlePhaseType.MyTurn:

                    OnAllyTurnStarted?.Invoke();

                    if (CurrentMainAlly.CharacterStats.IsStunned)
                    {
                        EndTurn();
                        return;
                    }

                    GameManager.PersistentGameplayData.CurrentMana = GameManager.PersistentGameplayData.MaxMana;

                    PlayerManager.DrawCards(CoreManager.PersistentGameplayData.DrawCount);

                    GameManager.PersistentGameplayData.CanSelectCards = true;

                    break;
                case BattlePhaseType.EnemyTurn:

                    OnEnemyTurnStarted?.Invoke();

                    PlayerManager.DiscardHand();

                    StartCoroutine(nameof(EnemyTurnRoutine));

                    GameManager.PersistentGameplayData.CanSelectCards = false;

                    break;

                // 전투 종료 후

                case BattlePhaseType.End:
                    CoreManager.PersistentGameplayData.CanSelectCards = false;
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(phaseType), phaseType, null);
            }
        }

        // 차례 종료
        public void EndTurn()
        {
            CurrentBattlePhaseType = BattlePhaseType.EnemyTurn;
        }
    }
}
