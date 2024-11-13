using System.Collections;
using chataan.Scripts.Card;
using chataan.Scripts.Data.Card;
using UnityEngine;

namespace chataan.Scripts.Card
{
    public class Card3D : CardBase
    {
        [Header("3D Settings")]
        [SerializeField] private Canvas canvas;

        public override void SetCard(CardData targetProfile, bool isPlayable)
        {
            base.SetCard(targetProfile, isPlayable);

            if (canvas)
                canvas.worldCamera = PlayerManager.HandManager.cam;
        }

        public override void SetInactiveMaterialState(bool isInactive)
        {
            base.SetInactiveMaterialState(isInactive);
        }
    }
}