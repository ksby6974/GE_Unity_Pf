using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace chataan.Scripts.Chara
{
    // ����������������������������������������������������
    // Player
    // ����������������������������������������������������
    public class Player : MyBase
    {
        public override void BuildCharacter()
        {
            base.BuildCharacter();
            if (UIManager != null)
            {
                CharacterStats.OnHealthChanged += UIManager.InfoCanvas.SetHealthText;
            }
            CharacterStats.SetCurrentHealth(CharacterStats.CurrentHealth);
        }
    }
}
