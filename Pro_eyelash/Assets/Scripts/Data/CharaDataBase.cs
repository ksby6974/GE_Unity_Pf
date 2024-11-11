using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace chataan.Scripts.Data.Chara
{
    // ━━━━━━━━━━━━━━━━━━━━━━━━━━
    // 캐릭터 객체 데이터 베이스
    // ━━━━━━━━━━━━━━━━━━━━━━━━━━
    public class CharaDataBase : MonoBehaviour
    {
        [Header("Info")]
        [SerializeField] protected string characterID;      // 인덱스
        [SerializeField] protected string characterName;    // 이름
        [SerializeField][TextArea] protected string characterDescription;   // 설명
        [SerializeField] protected int maxHealth;   // 최대 체력

        public string CharacterID => characterID;

        public string CharacterName => characterName;

        public string CharacterDescription => characterDescription;

        public int MaxHealth => maxHealth;
    }
}
