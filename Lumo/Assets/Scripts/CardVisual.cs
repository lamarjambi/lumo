using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardVisual : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    
    [Header("UI Components")]
    [SerializeField] private Image background;
    [SerializeField] private Image cardImage;
    
    [Header("Hand-Drawn Sprites")]
    [SerializeField] private Sprite[] numberSprites; // 0-9
    [SerializeField] private Sprite skipSprite;
    [SerializeField] private Sprite reverseSprite;
    [SerializeField] private Sprite drawTwoSprite;
    [SerializeField] private Sprite wildSprite;
    [SerializeField] private Sprite wildDrawFourSprite;
    
    [Header("Hover Settings")]
    [SerializeField] private float hoverScale = 1.2f;
    [SerializeField] private float hoverYOffset = 50f;
    
    private Card cardData;
    private RectTransform rectTransform;
    private Vector2 originalPosition;
    private bool isDragging = false;
    private Canvas canvas;
    
    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }
    
    public void SetCard(Card card)
    {
        cardData = card;
        
        // card's background color
        background.color = GetCardColor(card.Color);
        
        // set number or action sprite
        if (card.Type == CardType.Number)
        {
            cardImage.sprite = numberSprites[card.Number];
        }
        else
        {
            cardImage.sprite = card.Type switch
            {
                CardType.Skip => skipSprite,
                CardType.Reverse => reverseSprite,
                CardType.DrawTwo => drawTwoSprite,
                CardType.Wild => wildSprite,
                CardType.WildDrawFour => wildDrawFourSprite,
                _ => null
            };
        }
    }
    
    private Color GetCardColor(CardColor color)
    {
        return color switch
        {
            CardColor.Red => new Color32(237, 53, 85, 255),
            CardColor.Blue => new Color32(93, 75, 219, 255),
            CardColor.Green => new Color32(91, 220, 79, 255),
            CardColor.Yellow => new Color32(237, 203, 53, 255),
            CardColor.Wild => Color.black,
            _ => Color.white
        };
    }
    
    public Card GetCardData()
    {
        return cardData;
    }
    
    public void SetPosition(Vector2 position)
    {
        originalPosition = position;
        rectTransform.anchoredPosition = position;
    }
    
    // make card bigger on hover
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isDragging)
        {
            transform.localScale = Vector3.one * hoverScale;
            rectTransform.anchoredPosition = new Vector2(originalPosition.x, originalPosition.y + hoverYOffset);
        }
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isDragging)
        {
            transform.localScale = Vector3.one;
            rectTransform.anchoredPosition = originalPosition;
        }
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;
        transform.localScale = Vector3.one * 1.1f;
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }
    
    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
        transform.localScale = Vector3.one;
        
        if (IsOverDiscardPile())
        {
            GameController.Instance.TryPlayCard(this);
        }
        else
        {
            rectTransform.anchoredPosition = originalPosition;
        }
    }
    
    private bool IsOverDiscardPile()
    {
        return rectTransform.anchoredPosition.y > 0;
    }
}