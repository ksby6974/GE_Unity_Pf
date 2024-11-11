using chataan.Scripts.Chara;
using chataan.Scripts.Enums;

namespace chataan.Scripts.Interface
{
    // ━━━━━━━━━━━━━━━━━━━━━━━━━━
    // 캐릭터 인터페이스
    // ━━━━━━━━━━━━━━━━━━━━━━━━━━
    public interface IChara
    {
        public CharaBase GetCharacterBase();
        public CharaType GetCharacterType();
    }
}