using chataan.Scripts.Enums;
using UnityEngine;

namespace chataan.Scripts.Data.Containers
{
    // ����������������������������������������������������
    // ���� �ǵ� ������
    // �ǵ� : ���� ���� �ൿ�� �����ϰ�
    // ����������������������������������������������������
    [CreateAssetMenu(fileName = "EnemyIntention", menuName = "Chataan/Containers/EnemyIntention", order = 0)]
    public class EnemyIntentionData : ScriptableObject
    {
        [SerializeField] private EnemyIntentionType enemyIntentionType;
        [SerializeField] private Sprite intentionSprite;

        public EnemyIntentionType EnemyIntentionType => enemyIntentionType;

        public Sprite IntentionSprite => intentionSprite;
    }
}