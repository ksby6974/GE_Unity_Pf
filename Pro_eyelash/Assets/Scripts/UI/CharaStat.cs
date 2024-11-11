using chataan.Scripts.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace chataan.Scripts.Chara
{
    // ����������������������������������������������������
    // �����̻�
    // ����������������������������������������������������
    public class StatusStats
    {
        public StatusType StatusType { get; set; } // �����̻� ����
        public int StatusValue { get; set; } // ������ �����̻� ��
        public bool DecreaseOverTurn { get; set; } // ���ʰ� �带 ������ ������ �����̻� ���� ���� 
        public bool IsPermanent { get; set; } // ���� ���� ������ �����̻�
        public bool IsActive { get; set; } // Ȱ��ȭ
        public bool CanNegativeStack { get; set; } // ������ �����̻� �������� �Ҵ�
        public bool ClearAtNextTurn { get; set; } // ���� ���ʿ� ����

        public Action OnTriggerAction;

        // ������
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

    // ����������������������������������������������������
    // ĳ���� ü�� �� �����̻�
    // ����������������������������������������������������
    public class CharacterStats
    {
        public int MaxHealth { get; set; }          // �ִ� ü��
        public int CurrentHealth { get; set; }      // ���� ü��
        public bool IsStunned { get; set; }         // ����
        public bool IsDeath { get; private set; }   // ���

        // Action �븮�� �������� �Ѳ����� ���
        public Action OnDeath;
        public Action<int, int> OnHealthChanged;
        private readonly Action<StatusType, int> OnStatusChanged;
        private readonly Action<StatusType, int> OnStatusApplied;
        private readonly Action<StatusType> OnStatusCleared;
        public Action OnHealAction;
        public Action OnTakeDamageAction;
        public Action OnShieldGained;

        // ��ųʸ��� �����̻� ���� 
        public readonly Dictionary<StatusType, StatusStats> StatusDict = new Dictionary<StatusType, StatusStats>();

        // ��������������������������������������������������
        // ������
        // ��������������������������������������������������
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

        // ��������������������������������������������������
        // �ʱ� �����̻� ����
        // ��������������������������������������������������
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

        // ��������������������������������������������������
        // �����̻� ����
        // ��������������������������������������������������
        public void ApplyStatus(StatusType targetStatus, int value)
        {
            // ����
            if (StatusDict[targetStatus].IsActive)
            {
                StatusDict[targetStatus].StatusValue += value;
                OnStatusChanged?.Invoke(targetStatus, StatusDict[targetStatus].StatusValue);

            }
            // �ű� �����̻�
            else
            {
                StatusDict[targetStatus].StatusValue = value;
                StatusDict[targetStatus].IsActive = true;
                OnStatusApplied?.Invoke(targetStatus, StatusDict[targetStatus].StatusValue);
            }
        }

        // ��������������������������������������������������
        // �����̻� ȿ�� �߻�
        // ��������������������������������������������������
        public void TriggerAllStatus()
        {
            for (int i = 0; i < Enum.GetNames(typeof(StatusType)).Length; i++)
                TriggerStatus((StatusType)i);
        }

        // ��������������������������������������������������
        // ���� ü�� ����
        // ��������������������������������������������������
        public void SetCurrentHealth(int targetCurrentHealth)
        {
            CurrentHealth = targetCurrentHealth <= 0 ? 1 : targetCurrentHealth;
            OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
        }

        // ��������������������������������������������������
        // ü�� ȸ��
        // ��������������������������������������������������
        public void Heal(int value)
        {
            CurrentHealth += value;
            if (CurrentHealth > MaxHealth)
            {
                CurrentHealth = MaxHealth;
            }

            OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
        }

        // ��������������������������������������������������
        // ���� ����
        // ��������������������������������������������������
        public void Damage(int value, bool canPierceArmor = false)
        {
            // ��� �� ���� ����
            if (IsDeath)
            {
                return;
            }

            OnTakeDamageAction?.Invoke();
            var remainingDamage = value;

            // ���� ���غ��� �� �� ����
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

            // ü�� 0 ���Ϸ� ���
            if (CurrentHealth <= 0)
            {
                CurrentHealth = 0;
                OnDeath?.Invoke();
                IsDeath = true;
            }

            OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
        }

        // ��������������������������������������������������
        // �ִ�ü�� ����
        // ��������������������������������������������������
        public void IncreaseMaxHealth(int value)
        {
            MaxHealth += value;
            OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
        }

        // ��������������������������������������������������
        // �����̻� ���� ����
        // ��������������������������������������������������
        public void ClearAllStatus()
        {
            foreach (var status in StatusDict)
            {
                ClearStatus(status.Key);
            }
        }

        // ��������������������������������������������������
        // �����̻� ���� ����
        // ��������������������������������������������������
        public void ClearStatus(StatusType targetStatus)
        {
            StatusDict[targetStatus].IsActive = false;
            StatusDict[targetStatus].StatusValue = 0;
            OnStatusCleared?.Invoke(targetStatus);
        }

        // ��������������������������������������������������
        // �����̻� ȿ�� �ߵ�
        // ��������������������������������������������������
        private void TriggerStatus(StatusType targetStatus)
        {
            StatusDict[targetStatus].OnTriggerAction?.Invoke();

            // ���� ���ʿ��� ������� ���� �̻�
            if (StatusDict[targetStatus].ClearAtNextTurn)
            {
                ClearStatus(targetStatus);
                OnStatusChanged?.Invoke(targetStatus, StatusDict[targetStatus].StatusValue);
                return;
            }

            // ���� �̻� ����
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

            // ���� ���ʿ� ������ ��ġ ���ҵǴ� ���� �̻�
            if (StatusDict[targetStatus].DecreaseOverTurn)
            {
                StatusDict[targetStatus].StatusValue--;

            }

            // ������ ���� �̻� 0���� ����
            if (StatusDict[targetStatus].StatusValue == 0)
            {
                // ������ �����̻��� �ƴ϶��
                if (!StatusDict[targetStatus].IsPermanent)
                {
                    ClearStatus(targetStatus);
                }
            }

            OnStatusChanged?.Invoke(targetStatus, StatusDict[targetStatus].StatusValue);
        }

        // ��������������������������������������������������
        // ���� : ��
        // ��������������������������������������������������
        private void DamagePoison()
        {
            // ���� �̻� �� ����
            if (StatusDict[StatusType.Poison].StatusValue <= 0)
            {
                return;
            }

            // ����
            Damage(StatusDict[StatusType.Poison].StatusValue, true);
        }

        // ��������������������������������������������������
        // ���� ���� �̻�
        // ��������������������������������������������������
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
