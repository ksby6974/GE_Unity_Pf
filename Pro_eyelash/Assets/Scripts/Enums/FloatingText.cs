using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace chataan.Scripts.Utils
{
    public class FloatingText : MonoBehaviour
    {
        [SerializeField] private float duration = 1;
        [SerializeField] private AnimationCurve scaleCurve;
        [SerializeField] private AnimationCurve yForceCurve;
        [SerializeField] private AnimationCurve xForceCurve;
        [SerializeField] private TextMeshProUGUI textField;

        public void ShowText(string text, int xDir, int yDir = -1)
        {
            textField.text = text;
            StartCoroutine(Text(xDir, yDir));
        }

        private IEnumerator Text(int xDir, int yDir)
        {
            var waitFrame = new WaitForEndOfFrame();
            var timer = 0f;

            var initalScale = transform.localScale;

            while (timer <= duration)
            {
                timer += Time.deltaTime;
                transform.localScale = scaleCurve.Evaluate(timer / duration) * initalScale;
                var pos = transform.position;
                pos.x += xForceCurve.Evaluate(timer / duration) * xDir * Time.deltaTime;
                pos.y += yForceCurve.Evaluate(timer / duration) * yDir * Time.deltaTime;
                transform.position = pos;
                yield return waitFrame;
            }
            Destroy(gameObject);
        }
    }
}