namespace AfricanNationsLeague.Domain.Common
{
    public class ValueObjects
    {

        public record Scoreline(int Home, int Away);


        public record Goal(string PlayerName, int Minute);

    }
}
