using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-4)]
public class UIManager : Singleton<UIManager>
{
    protected MouseManager MouseManager => MouseManager.Instance;

    //[Header("Canvases")]

    // ���̵��
    [Header("Fader")]
    [SerializeField] private CanvasGroup fader;
    [SerializeField] private float fadeSpeed = 1f;

    public void ChangeScene(int iIndex)
    {
        Debug.Log($"{iIndex}��° ��� ChangeScene");
        StartCoroutine(ChangeSceneRoutine(iIndex));
    }

    private IEnumerator ChangeSceneRoutine(int iIndex)
    {
        SceneManager.LoadScene(iIndex);
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
