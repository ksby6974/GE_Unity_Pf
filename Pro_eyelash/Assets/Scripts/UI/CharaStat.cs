using chataan.Scripts.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace chataan.Scripts.Chara
{
    // 收收收收收收收收收收收收收收收收收收收收收收收收收收
    // 鼻鷓檜鼻
    // 收收收收收收收收收收收收收收收收收收收收收收收收收收
    public class StatusStats
    {
        public StatusType StatusType { get; set; } // 鼻鷓檜鼻 謙盟
        public int StatusValue { get; set; } // 援瞳脹 鼻鷓檜鼻 高
        public bool DecreaseOverTurn { get; set; } // 離滔陛 �撣� 陽葆棻 援瞳脹 鼻鷓檜鼻 高檜 馬模 
        public bool IsPermanent { get; set; } // 瞪癱 翕寰 艙掘瞳 鼻鷓檜鼻
        public bool IsActive { get; set; } // �側瘓�
        public bool CanNegativeStack { get; set; } // 擠熱煎 鼻鷓檜鼻 援瞳高檜 й渡
        public bool ClearAtNextTurn { get; set; } // 棻擠 離滔縑 п薯

        public Action OnTriggerAction;

        // 儅撩濠
        public StatusStats(StatusType statusType, int statusValue, bool decreaseOverTurn = false, bool isPermanent = false, bool isActive = false, bool canNegativeStack = false, bool clearAtNextTurn = false)
        {
            StatusType = statusType;
            StatusValue = statusValue;
            DecreaseOverTurn = decreaseOverTurn;
            IsPermanent = isPermanent;
            IsActive = isActive;
            CanNegativeStack = canNegativeStack;
            ClearAtNextTurn = clearAtNextTurn;
        }
    }

    // 收收收收收收收收收收收收收收收收收收收收收收收收收收
    // 議葛攪 羹晝 塽 鼻鷓檜鼻
    // 收收收收收收收收收收收收收收收收收收收收收收收收收收
    public class CharacterStats
    {
        public int MaxHealth { get; set; }          // 譆渠 羹溘
        public int CurrentHealth { get; set; }      // ⑷營 羹溘
        public bool IsStunned { get; set; }         // 晦瞰
        public bool IsDeath { get; private set; }   // 餌蜂

        // Action 渠葬濠 撲薑戲煎 и疏廓縑 啗骯
        public Action OnDeath;
        public Action<int, int> OnHealthChanged;
        private readonly Action<StatusType, int> OnStatusChanged;
        private readonly Action<StatusType, int> OnStatusApplied;
        private readonly Action<StatusType> OnStatusCleared;
        public Action OnHealAction;
        public Action OnTakeDamageAction;
        public Action OnShieldGained;

        // 蛐敷傘葬煎 鼻鷓檜鼻 盪濰 
        public readonly Dictionary<StatusType, StatusStats> StatusDict = new Dictionary<StatusType, StatusStats>();

        // 式式式式式式式式式式式式式式式式式式式式式式式式式
        // 儅撩濠
        // 式式式式式式式式式式式式式式式式式式式式式式式式式
        public CharacterStats(int maxHealth, CharaCanvas characterCanvas)
        {
            MaxHealth = maxHealth;
            CurrentHealth = maxHealth;
            SetAllStatus();

            OnHealthChanged += characterCanvas.UpdateHealthText;
            OnStatusChanged += characterCanvas.UpdateStatusText;
            OnStatusApplied += characterCanvas.ApplyStatus;
            OnStatusCleared += characterCanvas.ClearStatus;
        }

        // 式式式式式式式式式式式式式式式式式式式式式式式式式
        // 蟾晦 鼻鷓檜鼻 撲薑
        // 式式式式式式式式式式式式式式式式式式式式式式式式式
        private void SetAllStatus()
        {
            for (int i = 0; i < Enum.GetNames(typeof(StatusType)).Length; i++)
                StatusDict.Add((StatusType)i, new StatusStats((StatusType)i, 0));

            StatusDict[StatusType.Poison].DecreaseOverTurn = true;
            StatusDict[StatusType.Poison].OnTriggerAction += DamagePoison;

            StatusDict[StatusType.Block].ClearAtNextTurn = true;

            StatusDict[StatusType.Strength].CanNegativeStack = true;
            StatusDict[StatusType.Dexterity].CanNegativeStack = true;

            StatusDict[StatusType.Stun].DecreaseOverTurn = true;
            StatusDict[StatusType.Stun].OnTriggerAction += CheckStunStatus;

        }

        // 式式式式式式式式式式式式式式式式式式式式式式式式式
        // 鼻鷓檜鼻 瞳辨
        // 式式式式式式式式式式式式式式式式式式式式式式式式式
        public void ApplyStatus(StatusType targetStatus, int value)
        {
            // 援瞳
            if (StatusDict[targetStatus].IsActive)
            {
                StatusDict[targetStatus].StatusValue += value;
                OnStatusChanged?.Invoke(targetStatus, StatusDict[targetStatus].StatusValue);

            }
            // 褐敘 鼻鷓檜鼻
            else
            {
                StatusDict[targetStatus].StatusValue = value;
                StatusDict[targetStatus].IsActive = true;
                OnStatusApplied?.Invoke(targetStatus, StatusDict[targetStatus].StatusValue);
            }
        }

        // 式式式式式式式式式式式式式式式式式式式式式式式式式
        // 鼻鷓檜鼻 �膩� 嫦儅
        // 式式式式式式式式式式式式式式式式式式式式式式式式式
        public void TriggerAllStatus()
        {
            for (int i = 0; i < Enum.GetNames(typeof(StatusType)).Length; i++)
                TriggerStatus((StatusType)i);
        }

        // 式式式式式式式式式式式式式式式式式式式式式式式式式
        // ⑷營 羹溘 撲薑
        // 式式式式式式式式式式式式式式式式式式式式式式式式式
        public void SetCurrentHealth(int targetCurrentHealth)
        {
            CurrentHealth = targetCurrentHealth <= 0 ? 1 : targetCurrentHealth;
            OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
        }

        // 式式式式式式式式式式式式式式式式式式式式式式式式式
        // 羹溘 �蛹�
        // 式式式式式式式式式式式式式式式式式式式式式式式式式
        public void Heal(int value)
        {
            CurrentHealth += value;
            if (CurrentHealth > MaxHealth)
            {
                CurrentHealth = MaxHealth;
            }

            OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
        }

        // 式式式式式式式式式式式式式式式式式式式式式式式式式
        // Яп 嫡擠
        // 式式式式式式式式式式式式式式式式式式式式式式式式式
        public void Damage(int value, bool canPierceArmor = false)
        {
            // 餌蜂 衛 翱轎 儅楞
            if (IsDeath)
            {
                return;
            }

            OnTakeDamageAction?.Invoke();
            var remainingDamage = value;

            // 嫡擎 Яп爾棻 寞橫陛 渦 堪擠
            if (!canPierceArmor)
            {
                if (StatusDict[StatusType.Block].IsActive)
                {
                    ApplyStatus(StatusType.Block, -value);

                    remainingDamage = 0;
                    if (StatusDict[StatusType.Block].StatusValue <= 0)
                    {
                        remainingDamage = StatusDict[StatusType.Block].StatusValue * -1;
                        ClearStatus(StatusType.Block);
                    }
                }
            }

            CurrentHealth -= remainingDamage;

            // 羹溘 0 檜ж煎 餌蜂
            if (CurrentHealth <= 0)
            {
                CurrentHealth = 0;
                OnDeath?.Invoke();
                IsDeath = true;
            }

            OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
        }

        // 式式式式式式式式式式式式式式式式式式式式式式式式式
        // 譆渠羹溘 隸陛
        // 式式式式式式式式式式式式式式式式式式式式式式式式式
        public void IncreaseMaxHealth(int value)
        {
            MaxHealth += value;
            OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
        }

        // 式式式式式式式式式式式式式式式式式式式式式式式式式
        // 鼻鷓檜鼻 瞪睡 薯剪
        // 式式式式式式式式式式式式式式式式式式式式式式式式式
        public void ClearAllStatus()
        {
            foreach (var status in StatusDict)
            {
                ClearStatus(status.Key);
            }
        }

        // 式式式式式式式式式式式式式式式式式式式式式式式式式
        // 鼻鷓檜鼻 瞪睡 薯剪
        // 式式式式式式式式式式式式式式式式式式式式式式式式式
        public void ClearStatus(StatusType targetStatus)
        {
            StatusDict[targetStatus].IsActive = false;
            StatusDict[targetStatus].StatusValue = 0;
            OnStatusCleared?.Invoke(targetStatus);
        }

        // 式式式式式式式式式式式式式式式式式式式式式式式式式
        // 鼻鷓檜鼻 �膩� 嫦翕
        // 式式式式式式式式式式式式式式式式式式式式式式式式式
        private void TriggerStatus(StatusType targetStatus)
        {
            StatusDict[targetStatus].OnTriggerAction?.Invoke();

            // 棻擠 離滔縑憮 餌塭雖朝 鼻鷓 檜鼻
            if (StatusDict[targetStatus].ClearAtNextTurn)
            {
                ClearStatus(targetStatus);
                OnStatusChanged?.Invoke(targetStatus, StatusDict[targetStatus].StatusValue);
                return;
            }

            // 鼻鷓 檜鼻 薄匐
            if (StatusDict[targetStatus].StatusValue <= 0)
            {
                if (StatusDict[targetStatus].CanNegativeStack)
                {
                    if (StatusDict[targetStatus].StatusValue == 0 && !StatusDict[targetStatus].IsPermanent)
                        ClearStatus(targetStatus);
                }
                else
                {
                    if (!StatusDict[targetStatus].IsPermanent)
                        ClearStatus(targetStatus);
                }
            }

            // 棻擠 離滔縑 援瞳脹 熱纂 馬模腎朝 鼻鷓 檜鼻
            if (StatusDict[targetStatus].DecreaseOverTurn)
            {
                StatusDict[targetStatus].StatusValue--;

            }

            // 援瞳脹 鼻鷓 檜鼻 0戲煎 п薯
            if (StatusDict[targetStatus].StatusValue == 0)
            {
                // 艙掘瞳 鼻鷓檜鼻檜 嬴棲塭賊
                if (!StatusDict[targetStatus].IsPermanent)
                {
                    ClearStatus(targetStatus);
                }
            }

            OnStatusChanged?.Invoke(targetStatus, StatusDict[targetStatus].StatusValue);
        }

        // 式式式式式式式式式式式式式式式式式式式式式式式式式
        // Яп : 絮
        // 式式式式式式式式式式式式式式式式式式式式式式式式式
        private void DamagePoison()
        {
            // 鼻鷓 檜鼻 絮 襄營
            if (StatusDict[StatusType.Poison].StatusValue <= 0)
            {
                return;
            }

            // Яп
            Damage(StatusDict[StatusType.Poison].StatusValue, true);
        }

        // 式式式式式式式式式式式式式式式式式式式式式式式式式
        // 晦瞰 鼻鷓 檜鼻
        // 式式式式式式式式式式式式式式式式式式式式式式式式式
        public void CheckStunStatus()
        {
            if (StatusDict[StatusType.Stun].StatusValue <= 0)
            {
                IsStunned = false;
                return;
            }

            IsStunned = true;
        }
    }
}
