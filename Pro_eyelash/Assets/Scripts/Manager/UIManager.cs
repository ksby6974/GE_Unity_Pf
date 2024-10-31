using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public UIManager()
    {

    }

    public static UIManager Instance
    { 
        get;
        private set;
    }

    [Header("Fader")]
    [SerializeField] private CanvasGroup fader;
    [SerializeField] private float fadeSpeed = 1f;

    private void Awake()
    {
        //�Ŀ� �̱���
    }

    private IEnumerator ChangeScene(int iIndex)
    {
        SetScene.LoadScene(iIndex);
        yield return StartCoroutine(FadeInOut(false));
    }

    // ���̵� �ξƿ�
    // true = ��
    // false = �ƿ�
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
