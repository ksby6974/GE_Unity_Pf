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
    // ¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬
    // ÇÃ·¹ÀÌ¾î ÆÐÁ¶ÀÛ Å¬·¡½º
    // ¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬¦¬
    public class HandManager : MonoBehaviour
    {
        // Ä«µå
        [Header("Settings")]
        [SerializeField] private bool cardUprightWhenSelected = true;
        [SerializeField] private bool cardTilt = true;

        // ÆÐ
        [Header("Hand")]
        [SerializeField][Range(0, 5)] private float selectionSpacing = 1;
        [SerializeField] private Vector3 curveStart = new Vector3(2f, -0.7f, 0);
        [SerializeField] private Vector3 curveEnd = new Vector3(-2f, -0.7f, 0);
        [SerializeField] private Vector2 handOffset = new Vector2(0, -0.3f);
        [SerializeField] private Vector2 handSize = new Vector2(9, 1.7f);

        // ÂüÁ¶
        [Header("References")]
        public Transform discardTransform;
        public Transform exhaustTransform;
        public Transform drawTransform;
        public LayerMask selectableLayer;
        public LayerMask targetLayer;
        public Camera cam = null;
        [HideInInspector] public List<CardBase> hand; // ÇöÀç Ä«µå¸¦ Áã°í ÀÖ´Â ÆÐ

        protected FxManager FxManager => FxManager.Instance;
        protected SoundManager SoundManager => SoundManager.Instance;
        protected CoreManager CoreManager => CoreManager.Instance;
        protected BattleManager BattleManager => BattleManager.Instance;
        protected PlayerManager PlayerManager => PlayerManager.Instance;
        protected UIManager UIManager => UIManager.Instance;

        private Plane _plane; // world XY plane, used for mouse position raycasts
        private Vector3 _a, _b, _c; // Ä«µå¸¦ µå·¡±× ¾Ø µå·ÓÇÒ ¶§ ²ªÀÌ´Â °¢µµ 3¹æÇâ

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

        // ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
        // Awake
        // ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
        private void Awake()
        {
            // Ä«¸Þ¶ó¸¦ ÀÌ°÷À¸·Î
            _mainCam = Camera.main;
        }

        // ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
        // ÇÚµå ÃÊ±âÈ­
        // ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
        private void Start()
        {
            InitHand();
        }

        // ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
        // Update
        // ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
        private void Update()
        {
            // --------------------------------------------------------
            // HANDLE MOUSE & RAYCAST POSITION
            // µå·¡±× ¾Ø µå·Ó °úÁ¤
            // --------------------------------------------------------

            if (!IsDraggingActive)
            {
                return;
            }

            var mousePos = HandleMouseInput(out var count, out var sqrDistance, out var mouseButton);

            // --------------------------------------------------------
            // HANDLE CARDS IN HAND
            // ÆÐ¿¡ ÀÖ´Â Ä«µå Á¶ÀÛ
            // --------------------------------------------------------

            HandleCardsInHand(count, mouseButton, sqrDistance);

            // --------------------------------------------------------
            // HANDLE DRAGGED CARD
            // (Card held by mouse, inside hand)
            // Ä«µå µå·¡±× Á¶ÀÛ
            // --------------------------------------------------------

            HandleDraggedCardInsideHand(mouseButton, count);

            // --------------------------------------------------------
            // HANDLE HELD CARD
            // (Card held by mouse, outside of the hand)
            // Ä«µå È¦µå Á¶ÀÛ
            // --------------------------------------------------------

            HandleDraggedCardOutsideHand(mouseButton, mousePos);
        }

        // ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
        // ÆÐ ÃÊ±âÈ­
        // ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
        private void InitHand()
        {
            _a = transform.TransformPoint(curveStart);
            _b = transform.position;
            _c = transform.TransformPoint(curveEnd);
            _handBounds = new Rect((handOffset - handSize / 2), handSize);
            _plane = new Plane(-Vector3.forward, transform.position);
            _prevMousePos = Input.mousePosition;
        }

        // ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
        // Ä«µå µå·¡±× Á¶ÀÛ °¡´É ¿©ºÎ
        // ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
        public void EnableDragging() => IsDraggingActive = true;
        public void DisableDragging() => IsDraggingActive = false;

        // ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
        // ¸¶¿ì½º ÀÔ·Â Á¶ÀÛ
        // ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
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

        // ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
        // ÆÐ¿¡ ÀÖ´Â Ä«µå Á¶ÀÛ
        // ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
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

        // ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
        // ÆÐ¿¡ ÀÖ´Â Ä«µå Á¶ÀÛ »©³»±â
        // ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
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

        // ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
        // µå·ÓÀ¸·Î Ä«µå »ç¿ë
        // ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
        private void PlayCard(Vector2 mousePos)
        {
            // µå·ÓÀ¸·Î Ä«µå »ç¿ë
            var mouseButtonUp = Input.GetMouseButtonUp(0);
            if (!mouseButtonUp)
            {
                return;
            }

            // °­Á¶ Á¦°Å
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

        // ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
        // Ä³¸¯ÅÍ°¡ Ä«µå »ç¿ë °¡´ÉÇÑ°¡?
        // ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
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

        // ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
        // »ç¿ëÇÏÁö ¾ÊÀº Ä«µå´Â ´Ù½Ã ÆÐ¿¡ ¾ÈÀ¸·Î
        // ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
        private void HandleDraggedCardInsideHand(bool mouseButton, int count)
        {
            if (!mouseButton)
            {
                // µå·¡±× Á¤Áö
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
                // µå·¡±×µÈ Ä«µå ÀÌµ¿
                MoveCardToIndex(_dragged, _selected);
                _dragged = _selected;
            }
        }

        // ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
        // ÆÐ¿¡ µé¾î°¡´Â Ä«µå Ç¥Çö
        // ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
        private void CheckMouseInsideHandBounds(out bool mouseButton)
        {
            var point = transform.InverseTransformPoint(_mouseWorldPos);
            _mouseInsideHand = _handBounds.Contains(point);

            mouseButton = Input.GetMouseButton(0);
        }

        // ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
        // ÈÖ¾îÁö´Â È­»ìÇ¥ Ç¥ÇöÀ» À§ÇØ ÇöÀç Ä«µå¿ÍÀÇ °Å¸® °è»ê
        // ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
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

        // ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
        // ÇöÀç È­¸é¿¡¼­ ¸¶¿ì½º À§Ä¡ °è»ê
        // ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
        private void GetMouseWorldPosition(Vector2 mousePos)
        {
            var ray = cam.ScreenPointToRay(mousePos);
            if (_plane.Raycast(ray, out var enter)) _mouseWorldPos = ray.GetPoint(enter);
        }

        // ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
        // ÆÐ¿¡ ÀÖ´Â Ä«µå ±â¿ï¾îÁø °¢µµ Ç¥Çö
        // ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
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

        // ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
        // È­»ìÇ¥ ÈÖ¾îÁö´Â ºÎºÐ °è»ê (3¹æÇâ)
        // ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
        /// <summary>
        /// Obtains a point along a curve based on 3 points. Equal to Lerp(Lerp(a, b, t), Lerp(b, c, t), t).
        /// </summary>
        public static Vector3 GetCurvePoint(Vector3 a, Vector3 b, Vector3 c, float t)
        {
            t = Mathf.Clamp01(t);
            float oneMinusT = 1f - t;
            return (oneMinusT * oneMinusT * a) + (2f * oneMinusT * t * b) + (t * t * c);
        }

        // ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
        // È­»ìÇ¥ ÈÖ¾îÁö´Â ºÎºÐ °è»ê : ¹ÌºÐÀ¸·Î
        /// Obtains the derivative of the curve (tangent)
        // ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
        public static Vector3 GetCurveTangent(Vector3 a, Vector3 b, Vector3 c, float t)
        {
            return 2f * (1f - t) * (b - a) + 2f * t * (c - b);
        }

        // ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
        // È­»ìÇ¥ ÈÖ¾îÁö´Â ºÎºÐ °è»ê : Tan
        /// Obtains a direction perpendicular to the tangent of the curve
        // ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
        public static Vector3 GetCurveNormal(Vector3 a, Vector3 b, Vector3 c, float t)
        {
            Vector3 tangent = GetCurveTangent(a, b, c, t);
            return Vector3.Cross(tangent, Vector3.forward);
        }

        // ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
        /// Moves the card in hand from the currentIndex to the toIndex. If you want to move a card that isn't in hand, use AddCardToHand
        // ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
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

        // ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
        // ÆÐ¿¡ Ä«µå Ãß°¡
        // ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
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

        // ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
        // ÇÚµå¿¡¼­ Æ¯Á¤ ÀÎµ¦½ºÀÇ Ä«µå Á¦°Å
        // ¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡¦¡
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