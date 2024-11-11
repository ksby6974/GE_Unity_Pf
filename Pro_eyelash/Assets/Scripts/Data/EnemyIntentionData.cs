using chataan.Scripts.Enums;
using UnityEngine;

namespace chataan.Scripts.Data.Containers
{
    // ━━━━━━━━━━━━━━━━━━━━━━━━━━
    // 적의 의도 데이터
    // 의도 : 적의 다음 행동을 예측하게
    // ━━━━━━━━━━━━━━━━━━━━━━━━━━
    [CreateAssetMenu(fileName = "EnemyIntention", menuName = "Chataan/Containers/EnemyIntention", order = 0)]
    public class EnemyIntentionData : ScriptableObject
    {
        [SerializeField] private EnemyIntentionType enemyIntentionType;
        [SerializeField] private Sprite intentionSprite;

        public EnemyIntentionType EnemyIntentionType => enemyIntentionType;

        public Sprite IntentionSprite => intentionSprite;
    }
}