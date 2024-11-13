using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace chataan.Scripts.Chara
{
    // 收收收收收收收收收收收收收收收收收收收收收收收收收收
    // Player
    // 收收收收收收收收收收收收收收收收收收收收收收收收收收
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
