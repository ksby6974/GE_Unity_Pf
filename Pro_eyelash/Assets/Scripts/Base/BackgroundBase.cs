using UnityEngine;
using chataan.Scripts.Enums;

namespace chataan.Scripts.UI
{
    // ����������������������������������������������������
    // ���� ���
    // ����������������������������������������������������
    public class BackgroundBase : MonoBehaviour
    {
        [SerializeField] private BackgroundTypes backgroundType;

        public BackgroundTypes BackgroundType => backgroundType;
    }
}