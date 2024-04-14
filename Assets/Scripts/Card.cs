namespace CardHolder // Note the correct namespace here
{
    public class Card
    {
        public string rank;
        public string suit;

        public Card(string rank, string suit)
        {
            this.rank = rank;
            this.suit = suit;
        }
    }
}