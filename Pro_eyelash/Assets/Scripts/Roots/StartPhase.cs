using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// ����������������������������������������������������
// ���� ���� Ŭ����
// �̰����� �����ؼ� �ҷ���
// ����������������������������������������������������

[DefaultExecutionOrder(-11)]
public class StartPhase : MonoBehaviour
{
    void Awake()
    {
        try
        {
            //
            if (!CoreManager.Instance)
            {
                SceneManager.LoadScene("Phase", LoadSceneMode.Additive);
            }

            // �ش� ��ü �ı�
            Destroy(gameObject);
        }
        catch (Exception exception)
        {
            Debug.LogError($"�հ� �߸������� : {exception}");
            throw;
        }
    }
}
