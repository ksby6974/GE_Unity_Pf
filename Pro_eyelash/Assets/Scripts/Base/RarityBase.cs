using chataan.Scripts.Enums;
using UnityEngine;

namespace chataan.Scripts.Utils
{
    // ����������������������������������������������������
    // ī�� ��͵� �⺻ Ŭ����
    // ����������������������������������������������������
    public class RarityBase : MonoBehaviour
    {
        [SerializeField] private RarityType rarity;

        public RarityType Rarity => rarity;
    }
}