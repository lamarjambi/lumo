using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private static GameController instance;

    public static GameController Instance
    {
        get 
        { 
            return instance; 
        }
        private set 
        { 
            instance = value; 
        }
    }
    
    [Header("UI References")]
    [SerializeField] private Transform handArea;
    [SerializeField] private Transform discardPileArea;
    [SerializeField] private CardVisual cardPrefab;
    
    [Header("Game Settings")]
    [Range(2, 10)]
    [SerializeField] private int numberOfPlayers = 4;
    
    private Deck drawPile;
    private List<Card> discardPile;
    private List<List<Card>> playerHands;
    private List<CardVisual> playerHandVisuals; // visual representation
    private CardVisual topCardVisual;
    private int currentPlayerIndex;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        if (numberOfPlayers < 2)
        {
            Debug.LogError("Cannot start game with less than 2 players!");
            numberOfPlayers = 2;
        }
        
        InitializeGame();
    }
    
    private void InitializeGame()
    {
        // 1- with each new game, make a new deck and shuffle 
        drawPile = new Deck();
        drawPile.Initialize();
        drawPile.Shuffle();
        
        // 2- start a discard list (center) and put a card there
        discardPile = new List<Card>();
        Card firstCard = drawPile.Draw();
        discardPile.Add(firstCard);
        
        // show representation
        SpawnTopCard(firstCard);
        
        // 3- start a player cards' list
        playerHands = new List<List<Card>>();
        for (int i = 0; i < numberOfPlayers; i++)
        {
            playerHands.Add(new List<Card>());
        }
        
        // 4- give 7 cards for each player
        for (int i = 0; i < numberOfPlayers; i++)
        {
            for (int j = 0; j < 7; j++)
            {
                Card dealtCard = drawPile.Draw();
                playerHands[i].Add(dealtCard);
            }
        }
        
        // 5- start with player 0
        currentPlayerIndex = 0;
        playerHandVisuals = new List<CardVisual>();
        
        // 6- only display the current player's cards
        DisplayCurrentPlayerHand();
    
    }
    
    private void SpawnTopCard(Card card)
    {
        if (topCardVisual != null)
        {
            Destroy(topCardVisual.gameObject);
        }
        
        topCardVisual = Instantiate(cardPrefab, discardPileArea);
        topCardVisual.SetCard(card);
        topCardVisual.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        
        // disable dragging on discard pile card
        topCardVisual.enabled = false;
    }
    
    private void DisplayCurrentPlayerHand()
    {
        // clear old visuals
        foreach (CardVisual visual in playerHandVisuals)
        {
            Destroy(visual.gameObject);
        }
        playerHandVisuals.Clear();
        
        // create new visuals
        List<Card> currentHand = playerHands[currentPlayerIndex];
        float cardWidth = 120f;
        float startX = -(currentHand.Count - 1) * cardWidth * 0.5f;
        
        for (int i = 0; i < currentHand.Count; i++)
        {
            CardVisual cardVisual = Instantiate(cardPrefab, handArea);
            cardVisual.SetCard(currentHand[i]);
            
            Vector2 position = new Vector2(startX + (i * cardWidth), 0);
            cardVisual.SetPosition(position);
            
            playerHandVisuals.Add(cardVisual);
        }
    }
    
    public Card GetTopCard()
    {
        return discardPile[discardPile.Count - 1];
    }
    
    public void TryPlayCard(CardVisual cardVisual)
    {
        Card card = cardVisual.GetCardData();
        
        if (!card.CanPlayOn(GetTopCard()))
        {
            Debug.Log("Invalid play! Card doesn't match.");
            return;
        }
        
        playerHands[currentPlayerIndex].Remove(card);
        
        discardPile.Add(card);
        
        SpawnTopCard(card);
        
        Debug.Log("Player " + currentPlayerIndex + " played: " + card.Color + " " + card.Type);
        
        if (playerHands[currentPlayerIndex].Count == 0)
        {
            Debug.Log("Player " + currentPlayerIndex + " wins!");
            return;
        }
        
        NextTurn();
        
        DisplayCurrentPlayerHand();
    }
    
    public void DrawCardFromDeck()
    {
        Card drawnCard = drawPile.Draw();
        
        if (drawnCard == null)
        {
            Debug.Log("Deck is empty!");
            return;
        }
        
        playerHands[currentPlayerIndex].Add(drawnCard);
        Debug.Log("Player " + currentPlayerIndex + " drew a card");
        
        NextTurn();
        DisplayCurrentPlayerHand();
    }
    
    private void NextTurn()
    {
        currentPlayerIndex = (currentPlayerIndex + 1) % numberOfPlayers;
        Debug.Log("Now player " + currentPlayerIndex + "'s turn");
    }
    
    public List<Card> GetCurrentPlayerHand()
    {
        return playerHands[currentPlayerIndex];
    }
}