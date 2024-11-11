namespace chataan.Scripts.Enums
{
    // ━━━━━━━━━━━━━━━━━━━━━━━━━━
    // 전투 페이즈 순서
    // ━━━━━━━━━━━━━━━━━━━━━━━━━━
    public enum BattlePhaseType
    {
        Pre,        // 전투 전
        MyTurn,     // 플레이어 차례
        EnemyTurn,  // 적 차례
        End,        // 차례 종료
    }
}