using chataan.Scripts.Chara;
using chataan.Scripts.Enums;

namespace chataan.Scripts.Interface
{
    // ����������������������������������������������������
    // ĳ���� �������̽�
    // ����������������������������������������������������
    public interface IChara
    {
        public CharaBase GetCharaBase();
        public CharaType GetCharaType();
    }
}