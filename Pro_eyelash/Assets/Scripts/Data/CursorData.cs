using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using chataan.Scripts.Enums;

namespace chataan.Scripts.Data.Cursor
{
    [CreateAssetMenu(fileName = "CursorData", menuName = "Chataan/Containers/CursorData", order = 0)]
    public class CursorData : ScriptableObject
    {
        [SerializeField] private List<CursorProfile> cursorProfileList;
        public CursorProfile GetCursor(CursorType targetType)
        {
            var targetCursor = cursorProfileList.FirstOrDefault(x => x.cursorType == targetType);

            return targetCursor;
        }

        [Serializable]
        public class CursorProfile
        {
            public string name;
            public CursorType cursorType;
            public bool useCursorAnimation;
            public List<CursorAnimationFrame> cursorAnimationFrameList;
            public List<CursorAnimationFrame> cursorAnimationClickedFrameList;
            public float frameRate = 0.1f;
        }

        [Serializable]
        public class CursorAnimationFrame
        {
            public Texture2D cursorTexture;
            public Vector2 cursorOffset = Vector2.zero;
        }
    }
}
