using System;

namespace AlgorithmLab
{
    /// <summary>One line of the world leaderboard (like Reef Rush's ScoreEntry).</summary>
    [Serializable]
    public class LeaderboardEntry
    {
        public string initials;
        public int score;

        public LeaderboardEntry(string initials, int score)
        {
            this.initials = initials;
            this.score = score;
        }
    }
}
