using System;
using System.Collections;
using System.Collections.Generic;
using chataan.Scripts.Enums;
using UnityEngine;


namespace chataan.Scripts.Data.Keyword
{
    [CreateAssetMenu(fileName = "KeywordData", menuName = "Chataan/Containers/KeywordData", order = 0)]
    public class KeywordData : MonoBehaviour
    {
        [SerializeField] private List<KeywordBase> keywordBaseList;
        public List<KeywordBase> KeywordBaseList => keywordBaseList;
    }

    [Serializable]
    public class KeywordBase
    {
        [SerializeField] private KeywordType keyword;
        [SerializeField][TextArea] private string contentText;

        public KeywordType Keyword => keyword;


        public string GetHeader(string overrideKeywordHeader = "")
        {
            return string.IsNullOrEmpty(overrideKeywordHeader) ? keyword.ToString() : overrideKeywordHeader;
        }

        public string GetContent(string overrideContent = "")
        {
            return string.IsNullOrEmpty(overrideContent) ? contentText : overrideContent;
        }
    }
}
