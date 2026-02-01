public enum CardColor
{
    Red,
    Blue,
    Green,
    Yellow,
    Wild
}

public enum CardType
{
    Number,
    Skip,
    Reverse,
    DrawTwo,
    Wild,
    WildDrawFour
}

public class Card
{
    private CardColor color;
    private CardType type;
    private int number;

    public CardColor Color 
    {
        get { return color; }
        private set { color = value; }
    }

    public CardType Type 
    {
        get { return type; }
        private set { type = value; }
    }
    
    public int Number 
    {
        get { return number; }
        private set { number = value; }
    }

    public Card(CardColor color, CardType type, int number = -1)
    {
        this.color = color;
        this.type = type;
        this.number = number;
    }

    public bool CanPlayOn(Card topCard) 
    {
        if (Color == CardColor.Wild) return true;
        if (Color == topCard.Color) return true;
        if (Type == CardType.Number && Number == topCard.Number) return true;
        if (Type == topCard.Type && Type != CardType.Number) return true;
        return false;
    }
}
