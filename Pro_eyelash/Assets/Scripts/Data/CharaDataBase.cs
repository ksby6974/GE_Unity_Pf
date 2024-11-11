using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace chataan.Scripts.Data.Chara
{
    // ����������������������������������������������������
    // ĳ���� ��ü ������ ���̽�
    // ����������������������������������������������������
    public class CharaDataBase : MonoBehaviour
    {
        [Header("Info")]
        [SerializeField] protected string characterID;      // �ε���
        [SerializeField] protected string characterName;    // �̸�
        [SerializeField][TextArea] protected string characterDescription;   // ����
        [SerializeField] protected int maxHealth;   // �ִ� ü��

        public string CharacterID => characterID;

        public string CharacterName => characterName;

        public string CharacterDescription => characterDescription;

        public int MaxHealth => maxHealth;
    }
}
