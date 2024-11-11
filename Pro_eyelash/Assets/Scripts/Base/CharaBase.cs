using chataan.Scripts.Battle;
using chataan.Scripts.Enums;
using chataan.Scripts.Interface;
using chataan.Scripts.Managers;
using UnityEngine;

namespace chataan.Scripts.Chara
{
    // ����������������������������������������������������
    // ��ü Chara �⺻ �߻� Ŭ����
    // ����������������������������������������������������
    public abstract class CharaBase : MonoBehaviour, IChara
    {
        [Header("Settings")]
        [SerializeField] private CharaType charaType;
        [SerializeField] private Transform textSpawnRoot;

        public CharacterStats CharacterStats { get; protected set; }
        public CharaType CharaType => charaType;
        public Transform TextSpawnRoot => textSpawnRoot;
        protected FxManager FxManager => FxManager.Instance;
        protected SoundManager SoundManager => SoundManager.Instance;
        protected CoreManager CoreManager => CoreManager.Instance;
        protected BattleManager BattleManager => BattleManager.Instance;
        protected PlayerManager PlayerManager => PlayerManager.Instance;
        protected UIManager UIManager => UIManager.Instance;

        // ��������������������������������������������������
        // ���� ������� ó��
        // ��������������������������������������������������
        public virtual void Awake()
        {
        }

        public virtual void BuildCharacter()
        {

        }

        protected virtual void OnDeath()
        {

        }

        public CharaBase GetCharacterBase()
        {
            return this;
        }

        public CharaType GetCharacterType()
        {
            return CharaType;
        }
    }
}
