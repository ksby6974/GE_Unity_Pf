using chataan.Scripts.Card;
using chataan.Scripts.Chara;
using chataan.Scripts.Enums;
using chataan.Scripts.Interface;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

namespace chataan.Scripts.Managers
{
    // 收收收收收收收收收收收收收收收收收收收收收收收收收收
    // Ы溯檜橫 ぬ褻濛 贗楚蝶
    // 收收收收收收收收收收收收收收收收收收收收收收收收收收
    public class HandManager : MonoBehaviour
    {
        // 蘋萄
        [Header("Settings")]
        [SerializeField] private bool cardUprightWhenSelected = true;
        [SerializeField] private bool cardTilt = true;

        // ぬ
        [Header("Hand")]
        [SerializeField][Range(0, 5)] private float selectionSpacing = 1;
        [SerializeField] private Vector3 curveStart = new Vector3(2f, -0.7f, 0);
        [SerializeField] private Vector3 curveEnd = new Vector3(-2f, -0.7f, 0);
        [SerializeField] private Vector2 handOffset = new Vector2(0, -0.3f);
        [SerializeField] private Vector2 handSize = new Vector2(9, 1.7f);

        // 霤褻
        [Header("References")]
        public Transform discardTransform;
        public Transform exhaustTransform;
        public Transform drawTransform;
        public LayerMask selectableLayer;
        public LayerMask targetLayer;
        public Camera cam = null;
        [HideInInspector] public List<CardBase> hand; // ⑷營 蘋萄蒂 錨堅 氈朝 ぬ

        protected FxManager FxManager => FxManager.Instance;
        protected SoundManager SoundManager => SoundManager.Instance;
        protected CoreManager CoreManager => CoreManager.Instance;
        protected BattleManager BattleManager => BattleManager.Instance;
        protected PlayerManager PlayerManager => PlayerManager.Instance;
        protected UIManager UIManager => UIManager.Instance;

        private Plane _plane; // world XY plane, used for mouse position raycasts
        private Vector3 _a, _b, _c; // 蘋萄蒂 萄楚斜 撻 萄照й 陽 痕檜朝 陝紫 3寞щ

        private int _selected = -1; // Card index that is nearest to mouse
        private int _dragged = -1; // Card index that is held by mouse (inside of hand)
        private CardBase _heldCard; // Card that is held by mouse (when outside of hand)
        private Vector3 _heldCardOffset;
        private Vector2 _heldCardTilt;
        private Vector2 _force;
        private Vector3 _mouseWorldPos;
        private Vector2 _prevMousePos;
        private Vector2 _mousePosDelta;

        private Rect _handBounds;
        private bool _mouseInsideHand;

        private bool updateHierarchyOrder = false;
        private bool showDebugGizmos = true;

        private Camera _mainCam;

        public bool IsDraggingActive { get; private set; } = true;

        // 式式式式式式式式式式式式式式式式式式式式式式式式式
        // Awake
        // 式式式式式式式式式式式式式式式式式式式式式式式式式
        private void Awake()
        {
            // 蘋詭塭蒂 檜夠戲煎
            _mainCam = Camera.main;
        }

        // 式式式式式式式式式式式式式式式式式式式式式式式式式
        // с萄 蟾晦��
        // 式式式式式式式式式式式式式式式式式式式式式式式式式
        private void Start()
        {
            InitHand();
        }

        // 式式式式式式式式式式式式式式式式式式式式式式式式式
        // Update
        // 式式式式式式式式式式式式式式式式式式式式式式式式式
        private void Update()
        {
            // --------------------------------------------------------
            // HANDLE MOUSE & RAYCAST POSITION
            // 萄楚斜 撻 萄照 婁薑
            // --------------------------------------------------------

            if (!IsDraggingActive)
            {
                return;
            }

            var mousePos = HandleMouseInput(out var count, out var sqrDistance, out var mouseButton);

            // --------------------------------------------------------
            // HANDLE CARDS IN HAND
            // ぬ縑 氈朝 蘋萄 褻濛
            // --------------------------------------------------------

            HandleCardsInHand(count, mouseButton, sqrDistance);

            // --------------------------------------------------------
            // HANDLE DRAGGED CARD
            // (Card held by mouse, inside hand)
            // 蘋萄 萄楚斜 褻濛
            // --------------------------------------------------------

            HandleDraggedCardInsideHand(mouseButton, count);

            // --------------------------------------------------------
            // HANDLE HELD CARD
            // (Card held by mouse, outside of the hand)
            // 蘋萄 �朴� 褻濛
            // --------------------------------------------------------

            HandleDraggedCardOutsideHand(mouseButton, mousePos);
        }

        // 式式式式式式式式式式式式式式式式式式式式式式式式式
        // ぬ 蟾晦��
        // 式式式式式式式式式式式式式式式式式式式式式式式式式
        private void InitHand()
        {
            _a = transform.TransformPoint(curveStart);
            _b = transform.position;
            _c = transform.TransformPoint(curveEnd);
            _handBounds = new Rect((handOffset - handSize / 2), handSize);
            _plane = new Plane(-Vector3.forward, transform.position);
            _prevMousePos = Input.mousePosition;
        }

        // 式式式式式式式式式式式式式式式式式式式式式式式式式
        // 蘋萄 萄楚斜 褻濛 陛棟 罹睡
        // 式式式式式式式式式式式式式式式式式式式式式式式式式
        public void EnableDragging() => IsDraggingActive = true;
        public void DisableDragging() => IsDraggingActive = false;

        // 式式式式式式式式式式式式式式式式式式式式式式式式式
        // 葆辦蝶 殮溘 褻濛
        // 式式式式式式式式式式式式式式式式式式式式式式式式式
        private Vector2 HandleMouseInput(out int count, out float sqrDistance, out bool mouseButton)
        {
            Vector2 mousePos = Input.mousePosition;

            // Allows mouse to go outside game window but keeps cards within window
            // If mouse doesn't need to go outside, could use "Cursor.lockState = CursorLockMode.Confined;" instead
            mousePos.x = Mathf.Clamp(mousePos.x, 0, Screen.width);
            mousePos.y = Mathf.Clamp(mousePos.y, 0, Screen.height);

            // Mouse movement velocity
            if (cardTilt) TiltCard(mousePos);

            // Get world position from mouse
            GetMouseWorldPosition(mousePos);

            // Get distance to current selected card (for comparing against other cards later, to find closest)
            GetDistanceToCurrentSelectedCard(out count, out sqrDistance);

            // Check if mouse is inside hand bounds
            CheckMouseInsideHandBounds(out mouseButton);
            return mousePos;
        }

        // 式式式式式式式式式式式式式式式式式式式式式式式式式
        // ぬ縑 氈朝 蘋萄 褻濛
        // 式式式式式式式式式式式式式式式式式式式式式式式式式
        private void HandleCardsInHand(int count, bool mouseButton, float sqrDistance)
        {
            for (var i = 0; i < count; i++)
            {
                var card = hand[i];
                var cardTransform = card.transform;

                // Set to inactive material if not enough mana required to use card
                card.SetInactiveMaterialState(CoreManager.SavePlayData.CurrentMana < card.CardData.Cost);

                var noCardHeld = _heldCard == null; // Whether a card is "held" (outside of hand)
                var onSelectedCard = noCardHeld && _selected == i;
                var onDraggedCard = noCardHeld && _dragged == i;

                // Get Position along Curve (for card positioning)
                float selectOffset = 0;
                if (noCardHeld)
                    selectOffset = 0.02f *
                                   Mathf.Clamp01(1 - Mathf.Abs(Mathf.Abs(i - _selected) - 1) / (float)count * 3) *
                                   Mathf.Sign(i - _selected);

                var t = (i + 0.5f) / count + selectOffset * selectionSpacing;
                var p = GetCurvePoint(_a, _b, _c, t);

                var d = (p - _mouseWorldPos).sqrMagnitude;
                var mouseCloseToCard = d < 0.5f;
                var mouseHoveringOnSelected =
                    onSelectedCard && mouseCloseToCard && _mouseInsideHand; //  && mouseInsideHand

                // Handle Card Position & Rotation
                //Vector3 cardUp = p - (transform.position + Vector3.down * 7);
                var cardUp = GetCurveNormal(_a, _b, _c, t);
                var cardPos = p + (mouseHoveringOnSelected ? cardTransform.up * 0.3f : Vector3.zero);
                var cardForward = Vector3.forward;

                /* Card Tilt is disabled when in hand as they can clip through eachother :(
                if (cardTilt && onSelectedCard && mouseButton) {
                    cardForward -= new Vector3(heldCardOffset.x, heldCardOffset.y, 0);
                }*/

                // Sorting Order
                if (mouseHoveringOnSelected || onDraggedCard)
                {
                    // When selected bring card to front
                    if (cardUprightWhenSelected) cardUp = Vector3.up;
                    cardPos.z = transform.position.z - 0.2f;
                }
                else
                {
                    cardPos.z = transform.position.z + t * 0.5f;
                }

                // Rotation
                cardTransform.rotation = Quaternion.RotateTowards(cardTransform.rotation,
                    Quaternion.LookRotation(cardForward, cardUp), 80f * Time.deltaTime);

                // Handle Start Dragging
                if (mouseHoveringOnSelected)
                {
                    var mouseButtonDown = Input.GetMouseButtonDown(0);
                    if (mouseButtonDown)
                    {
                        _dragged = i;
                        _heldCardOffset = cardTransform.position - _mouseWorldPos;
                        _heldCardOffset.z = -0.1f;
                    }
                }

                // Handle Card Position
                if (onDraggedCard && mouseButton)
                {
                    // Held by mouse / dragging
                    cardPos = _mouseWorldPos + _heldCardOffset;
                    cardTransform.position = cardPos;
                }
                else
                {
                    cardPos = Vector3.MoveTowards(cardTransform.position, cardPos, 16f * Time.deltaTime);
                    cardTransform.position = cardPos;
                }

                // Get Selected Card
                if (CoreManager.SavePlayData.CanSelectCards)
                {
                    //float d = (p - mouseWorldPos).sqrMagnitude;
                    if (d < sqrDistance)
                    {
                        sqrDistance = d;
                        _selected = i;
                    }
                }
                else
                {
                    _selected = -1;
                    _dragged = -1;
                }

                // Debug Gizmos
                if (showDebugGizmos)
                {
                    var c = new Color(0, 0, 0, 0.2f);
                    if (i == _selected)
                    {
                        c = Color.red;
                        if (sqrDistance > 2) c = Color.blue;
                    }

                    Debug.DrawLine(p, _mouseWorldPos, c);
                }
            }
        }

        // 式式式式式式式式式式式式式式式式式式式式式式式式式
        // ぬ縑 氈朝 蘋萄 褻濛 貍頂晦
        // 式式式式式式式式式式式式式式式式式式式式式式式式式
        private void HandleDraggedCardOutsideHand(bool mouseButton, Vector2 mousePos)
        {
            if (_heldCard != null)
            {
                var cardTransform = _heldCard.transform;
                var cardUp = Vector3.up;
                var cardPos = _mouseWorldPos + _heldCardOffset;
                var cardForward = Vector3.forward;
                if (cardTilt && mouseButton) cardForward -= new Vector3(_heldCardTilt.x, _heldCardTilt.y, 0);

                // Bring card to front
                cardPos.z = transform.position.z - 0.2f;

                // Handle Position & Rotation
                cardTransform.rotation = Quaternion.RotateTowards(cardTransform.rotation,
                    Quaternion.LookRotation(cardForward, cardUp), 80f * Time.deltaTime);
                cardTransform.position = cardPos;

                BattleManager.HighlightCardTarget(_heldCard.CardData.CardActionDataList[0].TargetType);

                //if (!canSelectCards || cardTransform.position.y <= transform.position.y + 0.5f) {
                if (!CoreManager.SavePlayData.CanSelectCards || _mouseInsideHand)
                {
                    //  || sqrDistance <= 2
                    // Card has gone back into hand
                    AddCardToHand(_heldCard, _selected);
                    _dragged = _selected;
                    _selected = -1;
                    _heldCard = null;

                    BattleManager.DeactivateCardHighlights();

                    return;
                }

                PlayCard(mousePos);
            }
        }

        // 式式式式式式式式式式式式式式式式式式式式式式式式式
        // 萄照戲煎 蘋萄 餌辨
        // 式式式式式式式式式式式式式式式式式式式式式式式式式
        private void PlayCard(Vector2 mousePos)
        {
            // 萄照戲煎 蘋萄 餌辨
            var mouseButtonUp = Input.GetMouseButtonUp(0);
            if (!mouseButtonUp)
            {
                return;
            }

            // 鬼褻 薯剪
            BattleManager.DeactivateCardHighlights();
            bool backToHand = true;

            if (CoreManager.SavePlayData.CanUseCards && CoreManager.SavePlayData.CurrentMana >= _heldCard.CardData.Cost)
            {
                //RaycastHit hit;
                var mainRay = _mainCam.ScreenPointToRay(mousePos);
                var _canUse = false;
                CharaBase selfCharacter = BattleManager.CurrentMainAlly;
                CharaBase targetCharacter = null;

                _canUse = _heldCard.CardData.UsableWithoutTarget || CheckPlayOnCharacter(mainRay, _canUse, ref selfCharacter, ref targetCharacter);

                if (_canUse)
                {
                    backToHand = false;
                    _heldCard.Use(selfCharacter, targetCharacter, BattleManager.CurrentEnemiesList, BattleManager.CurrentAlliesList);
                }
            }

            if (backToHand) // Cannot use card / Not enough mana! Return card to hand!
                AddCardToHand(_heldCard, _selected);

            _heldCard = null;
        }

        // 式式式式式式式式式式式式式式式式式式式式式式式式式
        // 議葛攪陛 蘋萄 餌辨 陛棟и陛?
        // 式式式式式式式式式式式式式式式式式式式式式式式式式
        private bool CheckPlayOnCharacter(Ray mainRay, bool _canUse, ref CharaBase selfCharacter,
            ref CharaBase targetCharacter)
        {
            RaycastHit hit;
            if (Physics.Raycast(mainRay, out hit, 1000, targetLayer))
            {
                var character = hit.collider.gameObject.GetComponent<IChara>();

                if (character != null)
                {
                    var checkEnemy = (_heldCard.CardData.CardActionDataList[0].TargetType == TargetType.Enemy && character.GetCharaType() == CharaType.Enemy);
                    var checkAlly = (_heldCard.CardData.CardActionDataList[0].TargetType == TargetType.Ally && character.GetCharaType() == CharaType.My);

                    if (checkEnemy || checkAlly)
                    {
                        _canUse = true;
                        selfCharacter = BattleManager.CurrentMainAlly;
                        targetCharacter = character.GetCharaBase();
                    }
                }
            }

            return _canUse;
        }

        // 式式式式式式式式式式式式式式式式式式式式式式式式式
        // 餌辨ж雖 彊擎 蘋萄朝 棻衛 ぬ縑 寰戲煎
        // 式式式式式式式式式式式式式式式式式式式式式式式式式
        private void HandleDraggedCardInsideHand(bool mouseButton, int count)
        {
            if (!mouseButton)
            {
                // 萄楚斜 薑雖
                _heldCardOffset = Vector3.zero;
                _dragged = -1;
            }

            if (_dragged != -1)
            {
                var card = hand[_dragged];
                if (mouseButton && !_mouseInsideHand)
                {
                    //  && sqrDistance > 2.1f
                    //if (cardPos.y > transform.position.y + 0.5) {
                    // Card is outside of the hand, so is considered "held" ready to be used
                    // Remove from hand, so that cards in hand fill the hole that the card left
                    _heldCard = card;
                    RemoveCardFromHand(_dragged);
                    count--;
                    _dragged = -1;
                }
            }

            if (_heldCard == null && mouseButton && _dragged != -1 && _selected != -1 && _dragged != _selected)
            {
                // 萄楚斜脹 蘋萄 檜翕
                MoveCardToIndex(_dragged, _selected);
                _dragged = _selected;
            }
        }

        // 式式式式式式式式式式式式式式式式式式式式式式式式式
        // ぬ縑 菟橫陛朝 蘋萄 ル⑷
        // 式式式式式式式式式式式式式式式式式式式式式式式式式
        private void CheckMouseInsideHandBounds(out bool mouseButton)
        {
            var point = transform.InverseTransformPoint(_mouseWorldPos);
            _mouseInsideHand = _handBounds.Contains(point);

            mouseButton = Input.GetMouseButton(0);
        }

        // 式式式式式式式式式式式式式式式式式式式式式式式式式
        // �祩轀鶼� �香嚂� ル⑷擊 嬪п ⑷營 蘋萄諦曖 剪葬 啗骯
        // 式式式式式式式式式式式式式式式式式式式式式式式式式
        private void GetDistanceToCurrentSelectedCard(out int count, out float sqrDistance)
        {
            count = hand.Count;
            sqrDistance = 1000;
            if (_selected >= 0 && _selected < count)
            {
                var t = (_selected + 0.5f) / count;
                var p = GetCurvePoint(_a, _b, _c, t);
                sqrDistance = (p - _mouseWorldPos).sqrMagnitude;
            }
        }

        // 式式式式式式式式式式式式式式式式式式式式式式式式式
        // ⑷營 �飛橦□� 葆辦蝶 嬪纂 啗骯
        // 式式式式式式式式式式式式式式式式式式式式式式式式式
        private void GetMouseWorldPosition(Vector2 mousePos)
        {
            var ray = cam.ScreenPointToRay(mousePos);
            if (_plane.Raycast(ray, out var enter)) _mouseWorldPos = ray.GetPoint(enter);
        }

        // 式式式式式式式式式式式式式式式式式式式式式式式式式
        // ぬ縑 氈朝 蘋萄 晦選橫霞 陝紫 ル⑷
        // 式式式式式式式式式式式式式式式式式式式式式式式式式
        private void TiltCard(Vector2 mousePos)
        {
            _mousePosDelta = (mousePos - _prevMousePos) * new Vector2(1600f / Screen.width, 900f / Screen.height) *
                            Time.deltaTime;
            _prevMousePos = mousePos;

            var tiltStrength = 3f;
            var tiltDrag = 3f;
            var tiltSpeed = 50f;

            _force += (_mousePosDelta * tiltStrength - _heldCardTilt) * Time.deltaTime;
            _force *= 1 - tiltDrag * Time.deltaTime;
            _heldCardTilt += _force * (Time.deltaTime * tiltSpeed);
            // these calculations probably aren't correct, but hey, they work...? :P

            if (showDebugGizmos)
            {
                Debug.DrawRay(_mouseWorldPos, _mousePosDelta, Color.red);
                Debug.DrawRay(_mouseWorldPos, _heldCardTilt, Color.cyan);
            }
        }

        // 式式式式式式式式式式式式式式式式式式式式式式式式式
        // �香嚂� �祩轀鶼� 睡碟 啗骯 (3寞щ)
        // 式式式式式式式式式式式式式式式式式式式式式式式式式
        /// <summary>
        /// Obtains a point along a curve based on 3 points. Equal to Lerp(Lerp(a, b, t), Lerp(b, c, t), t).
        /// </summary>
        public static Vector3 GetCurvePoint(Vector3 a, Vector3 b, Vector3 c, float t)
        {
            t = Mathf.Clamp01(t);
            float oneMinusT = 1f - t;
            return (oneMinusT * oneMinusT * a) + (2f * oneMinusT * t * b) + (t * t * c);
        }

        // 式式式式式式式式式式式式式式式式式式式式式式式式式
        // �香嚂� �祩轀鶼� 睡碟 啗骯 : 嘐碟戲煎
        /// Obtains the derivative of the curve (tangent)
        // 式式式式式式式式式式式式式式式式式式式式式式式式式
        public static Vector3 GetCurveTangent(Vector3 a, Vector3 b, Vector3 c, float t)
        {
            return 2f * (1f - t) * (b - a) + 2f * t * (c - b);
        }

        // 式式式式式式式式式式式式式式式式式式式式式式式式式
        // �香嚂� �祩轀鶼� 睡碟 啗骯 : Tan
        /// Obtains a direction perpendicular to the tangent of the curve
        // 式式式式式式式式式式式式式式式式式式式式式式式式式
        public static Vector3 GetCurveNormal(Vector3 a, Vector3 b, Vector3 c, float t)
        {
            Vector3 tangent = GetCurveTangent(a, b, c, t);
            return Vector3.Cross(tangent, Vector3.forward);
        }

        // 式式式式式式式式式式式式式式式式式式式式式式式式式
        /// Moves the card in hand from the currentIndex to the toIndex. If you want to move a card that isn't in hand, use AddCardToHand
        // 式式式式式式式式式式式式式式式式式式式式式式式式式
        public void MoveCardToIndex(int currentIndex, int toIndex)
        {
            if (currentIndex == toIndex) return; // Same index, do nothing
            CardBase card = hand[currentIndex];
            hand.RemoveAt(currentIndex);
            hand.Insert(toIndex, card);

            if (updateHierarchyOrder)
            {
                card.transform.SetSiblingIndex(toIndex);
            }
        }

        // 式式式式式式式式式式式式式式式式式式式式式式式式式
        // ぬ縑 蘋萄 蹺陛
        // 式式式式式式式式式式式式式式式式式式式式式式式式式
        public void AddCardToHand(CardBase card, int index = -1)
        {
            if (index < 0)
            {
                // Add to end
                hand.Add(card);
                index = hand.Count - 1;
            }
            else
            {
                // Insert at index
                hand.Insert(index, card);
            }

            if (updateHierarchyOrder)
            {
                card.transform.SetParent(transform);
                card.transform.SetSiblingIndex(index);
            }
        }

        // 式式式式式式式式式式式式式式式式式式式式式式式式式
        // с萄縑憮 か薑 檣策蝶曖 蘋萄 薯剪
        // 式式式式式式式式式式式式式式式式式式式式式式式式式
        public void RemoveCardFromHand(int index)
        {
            if (updateHierarchyOrder)
            {
                CardBase card = hand[index];
                card.transform.SetParent(transform.parent);
                card.transform.SetSiblingIndex(transform.GetSiblingIndex() + 1);
            }

            hand.RemoveAt(index);
        }
    }

}