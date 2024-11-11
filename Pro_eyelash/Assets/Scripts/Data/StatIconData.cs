using chataan.Scripts.Enums;
using chataan.Scripts.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace chataan.Scripts.Data.Containers
{
    // ����������������������������������������������������
    // ���� �̻� ������
    // ����������������������������������������������������
    [CreateAssetMenu(fileName = "StatusIcons", menuName = "Chataan/Containers/StatusIcons", order = 2)]
    public class StatusIconsData : ScriptableObject
    {
        [SerializeField] private StatusIconBase statusIconBasePrefab;
        [SerializeField] private List<StatusIconData> statusIconList;

        public StatusIconBase StatusIconBasePrefab => statusIconBasePrefab;
        public List<StatusIconData> StatusIconList => statusIconList;
    }

    // ����������������������������������������������������
    // ���� �̻� ������ ������
    // ����������������������������������������������������
    [Serializable]
    public class StatusIconData
    {
        [SerializeField] private StatusType iconStatus;
        [SerializeField] private Sprite iconSprite;
        [SerializeField] private List<KeywordType> keywords;
        public StatusType IconStatus => iconStatus;
        public Sprite IconSprite => iconSprite;
        public List<KeywordType> KeywordType => keywords;
    }
}