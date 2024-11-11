using UnityEngine;

namespace chataan.Scripts.Data.Reward
{

    // ����������������������������������������������������
    // ���� ������ �⺻ Ŭ����
    // ����������������������������������������������������
    public class RewardDataBase : ScriptableObject
    {
        [SerializeField] private Sprite rewardSprite;
        [TextArea][SerializeField] private string rewardDescription;
        public Sprite RewardSprite => rewardSprite;
        public string RewardDescription => rewardDescription;
    }
}