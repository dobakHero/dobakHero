public class Card
{
    public Card(int n, EMark m)
    {
        Number = n;
        Mark = m;
    }
    public int Number;
    public EMark Mark;
}

public enum EMark
{
    Spade,
    Heart,
    Clover,
    Diamond,
    
}