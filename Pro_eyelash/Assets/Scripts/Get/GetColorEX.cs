using System.Text;
using UnityEngine;

namespace chataan.Scripts.Gets.ColorEx
{
    // ����������������������������������������������������
    // �ؽ�Ʈ�� ���� ������
    // ����������������������������������������������������
    public static class GetColorEX
    {
        public static string ColorString(string text, Color color)
        {
            // ���� ���� ����
            var str = new StringBuilder();

            // �ش� �ؽ�Ʈ�� ������ ������
            str.Append("<color=#").Append(ColorUtility.ToHtmlStringRGBA(color)).Append(">").Append(text).Append("</color>");

            // ��ȯ
            return str.ToString();
        }
    }
}