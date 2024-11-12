using chataan.Scripts.Data.Card;
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
        // â �����ֱ�
        // ��������������������������������������������������
        public void OpenInventory(List<CardData> cardList, string title)
        {
            SetCanvas(InvenCanvas, true, true);
            InvenCanvas.ChangeTitle(title);
            InvenCanvas.SetCards(cardList);
        }

        // ��������������������������������������������������
        // ĵ���� ����
        // ��������������������������������������������������
        public void SetCanvas(CanvasBase targetCanvas, bool open, bool reset = false)
        {
            if (reset)
                targetCanvas.ResetCanvas();

            if (open)
                targetCanvas.OpenCanvas();
            else
                targetCanvas.CloseCanvas();
        }

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