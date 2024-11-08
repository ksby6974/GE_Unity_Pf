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
       // [SerializeField] private BackgroundContainer backgroundContainer; // ��� �̹���
        [SerializeField] private List<Transform> enemyPosList;  // �� ��ġ
        [SerializeField] private List<Transform> allyPosList;   // �Ʊ� ��ġ

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

        // �̱��� �ƴ�
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
                // ó������ ���� ���� ���������
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
                // ���� ���� ����
                case BattlePhaseType.Pre:
                    break;

                // �÷��̾� ��
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

                // ���� ���� ��

                case BattlePhaseType.End:
                    CoreManager.PersistentGameplayData.CanSelectCards = false;
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(phaseType), phaseType, null);
            }
        }

        // ���� ����
        public void EndTurn()
        {
            CurrentBattlePhaseType = BattlePhaseType.EnemyTurn;
        }
    }
}
