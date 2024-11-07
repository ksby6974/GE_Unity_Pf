using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace chataan.Scripts.Utils
{
    public class TooltipText : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI headerText;
        [SerializeField] private TextMeshProUGUI contentText;
        [SerializeField] private Image backgroundImage;
        [SerializeField] private LayoutElement layoutElement;
        [SerializeField] private int characterWrapLimit = 50;

        public void SetText(string content = "", string header = "")
        {
            if (string.IsNullOrEmpty(header))
            {
                headerText.gameObject.SetActive(false);
            }
            else
            {
                headerText.gameObject.SetActive(true);
                headerText.text = header;
            }

            if (string.IsNullOrEmpty(content))
            {
                contentText.gameObject.SetActive(false);
            }
            else
            {
                contentText.gameObject.SetActive(true);
                contentText.text = content;
            }

            if (contentText.gameObject.activeSelf || headerText.gameObject.activeSelf)
            {
                backgroundImage.gameObject.SetActive(true);
            }
            else
            {
                backgroundImage.gameObject.SetActive(false);
            }

            PrepareLayout();
        }

        // 레이아웃 대기
        private void PrepareLayout()
        {
            var longestTextLength = GetLongestTextLength();
            layoutElement.enabled = (longestTextLength > characterWrapLimit);
        }

        // 텍스트 길이
        private int GetLongestTextLength()
        {
            if (headerText.text.Length > contentText.text.Length)
            {
                return headerText.text.Length;
            }

            if (headerText.text.Length <= contentText.text.Length)
            {
                return contentText.text.Length;
            }

            return 0;
        }
    }
}
