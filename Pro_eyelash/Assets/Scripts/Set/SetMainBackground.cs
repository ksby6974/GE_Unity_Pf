using UnityEngine;
using chataan.Scripts.Enums;

namespace chataan.Scripts.Utils.Background
{
    // ����������������������������������������������������
    // ���� ���
    // ����������������������������������������������������
    public class BackgroundRoot : MonoBehaviour
    {
        [SerializeField] private BackgroundTypes backgroundType;

        public BackgroundTypes BackgroundType => backgroundType;
    }
}