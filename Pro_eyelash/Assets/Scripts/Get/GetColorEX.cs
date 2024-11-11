using System.Text;
using UnityEngine;

namespace chataan.Scripts.Gets.ColorEx
{
    // ━━━━━━━━━━━━━━━━━━━━━━━━━━
    // 텍스트에 색깔 입히기
    // ━━━━━━━━━━━━━━━━━━━━━━━━━━
    public static class GetColorEX
    {
        public static string ColorString(string text, Color color)
        {
            // 성능 저하 방지
            var str = new StringBuilder();

            // 해당 텍스트에 색깔을 입힌다
            str.Append("<color=#").Append(ColorUtility.ToHtmlStringRGBA(color)).Append(">").Append(text).Append("</color>");

            // 반환
            return str.ToString();
        }
    }
}