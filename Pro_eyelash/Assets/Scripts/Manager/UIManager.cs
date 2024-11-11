using chataan.Scripts.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace chataan.Scripts.Managers
{
    // ����������������������������������������������������
    // UI �Ѱ� Ŭ����
    // ����������������������������������������������������
    [DefaultExecutionOrder(-4)]
    public class UIManager : Singleton<UIManager>
    {
        protected MouseManager MouseManager => MouseManager.Instance;

        // ���� ǥ�ÿ�
        [Header("Canvas")]
        [SerializeField] private BattleCanvas battleCanvas;
        [SerializeField] private InfoCanvas infoCanvas;
        [SerializeField] private RewardCanvas rewardCanvas;
        [SerializeField] private InvenCanvas invenCanvas;

        // ���̵��
        [Header("Fader")]
        [SerializeField] private CanvasGroup fader;
        [SerializeField] private float fadeSpeed = 1f;

        public BattleCanvas BattleCanvas => battleCanvas;
        public InfoCanvas InfoCanvas => infoCanvas;
        public RewardCanvas RewardCanvas => rewardCanvas;
        public InvenCanvas InvenCanvas => invenCanvas;

        // ��������������������������������������������������
        // ��� ��ȯ
        // ��������������������������������������������������
        public void ChangeScene(int iIndex)
        {
            Debug.Log($"{iIndex}��° ��� ChangeScene");
            StartCoroutine(ChangeSceneRoutine(iIndex));
        }

        // ��������������������������������������������������
        // Scene �ҷ�����
        // ��������������������������������������������������
        private IEnumerator ChangeSceneRoutine(int iIndex)
        {
            SceneManager.LoadScene(iIndex);
            yield return StartCoroutine(FadeInOut(false));
        }

        // ��������������������������������������������������
        // ���̵� �ξƿ�
        // true = ��
        // false = �ƿ�
        // ��������������������������������������������������
        public IEnumerator FadeInOut(bool bOn)
        {
            var LimitFrame = new WaitForEndOfFrame();
            float fTime = bOn ? 0f : 1f;

            while (true)
            {
                fTime += Time.deltaTime * (bOn ? fadeSpeed * 1 : fadeSpeed * -1);
                fader.alpha = fTime;

                if (fTime >= 1f)
                {
                    break;
                }

                yield return LimitFrame;
            }
        }
    }

}