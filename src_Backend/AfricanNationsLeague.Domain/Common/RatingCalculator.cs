using AfricanNationsLeague.Domain.Entities;

namespace AfricanNationsLeague.Domain.Common
{
    public static class RatingCalculator
    {
        public static double TeamAverageRating(IEnumerable<Player> players)
        {
            if (players == null || !players.Any()) return 0;
            // average of each player's highest position rating could be used or average of natural position rating
            return players.Select(p => p.Ratings.Values.Average()).Average();
        }
    }
}
