using UnityEngine;

namespace chataan.Scripts.Data.Reward
{

    // ━━━━━━━━━━━━━━━━━━━━━━━━━━
    // 보상 데이터 기본 클래스
    // ━━━━━━━━━━━━━━━━━━━━━━━━━━
    public class RewardDataBase : ScriptableObject
    {
        [SerializeField] private Sprite rewardSprite;
        [TextArea][SerializeField] private string rewardDescription;
        public Sprite RewardSprite => rewardSprite;
        public string RewardDescription => rewardDescription;
    }
}