using chataan.Scripts.Enums;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace chataan.Scripts.Card
{
    // ����������������������������������������������������
    // ī�� �̱�
    // ����������������������������������������������������
    public class SetDraw : CardActionBase
    {
        public override CardEffectType ActionType => CardEffectType.Draw;
        public override void DoAction(CardActionParameters actionParameters)
        {
            if (PlayerManager != null)
            {
                PlayerManager.DrawCards(Mathf.RoundToInt(actionParameters.Value));
            }
            else
            {
                Debug.LogError("No PlayerManager");
            }

            if (FxManager != null)
            {
                FxManager.PlayFx(actionParameters.SelfCharacter.transform, FxType.Buff);
            }

           // if (SoundManager != null)
                //SoundManager.PlayOneShot(actionParameters.CardData.AudioType);
        }
    }
}
