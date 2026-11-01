using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AlgorithmLab
{
    /// <summary>
    /// A world leaderboard of 1,000 scores, highest first. Type a score to see what rank it would get,
    /// then submit it. The starter makes the board and draws it. You write LinearRank and BinaryRank.
    /// </summary>
    public sealed class Leaderboard : MonoBehaviour
    {
        private const string HighlightColour = "#FFD54F";
        private const int RowsShown = 9;

        [SerializeField, Min(10)] private int size = 1000;

        [Header("UI")]
        [SerializeField] private InputField scoreInput;
        [SerializeField] private Text messageText;
        [SerializeField] private Text ranksText;
        [SerializeField] private Text initialsText;
        [SerializeField] private Text scoresText;

        // Highest score first: entries[0] is rank 1.
        private readonly List<LeaderboardEntry> entries = new List<LeaderboardEntry>();

        // Your code adds 1 for every entry it compares the new score with.
        private int checks;

        private void Awake()
        {
            // The same "random" board every time: a fixed seed gives the same sequence of numbers.
            System.Random random = new System.Random(2026);
            const string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            for (int i = 0; i < size; i++)
            {
                string initials = $"{letters[random.Next(26)]}{letters[random.Next(26)]}{letters[random.Next(26)]}";
                entries.Add(new LeaderboardEntry(initials, random.Next(1, 10000) * 10));
            }
            entries.Sort((a, b) => b.score.CompareTo(a.score));   // highest first, as in Week 6

            messageText.text = $"{entries.Count:N0} scores, from {entries[0].score:N0} down to {entries[entries.Count - 1].score:N0}.\nType a score, then try both searches.";
            Show(0, -1);
        }

        // ---------------------------------------------------------------- buttons (part of the starter)

        public void CheckLinear() => Check("Linear", LinearRank);

        public void CheckBinary() => Check("Binary", BinaryRank);

        /// <summary>Put the new score into the board at the place binary search found, without sorting again.</summary>
        public void Submit()
        {
            if (!ReadScore(out int score)) return;
            checks = 0;
            int place = BinaryRank(score);
            if (place < 0)
            {
                messageText.text = "BinaryRank isn't written yet (step A4).";
                return;
            }
            entries.Insert(place, new LeaderboardEntry("YOU", score));
            messageText.text = $"Inserted at rank {place + 1} after {Count(checks, "check")}.\n{entries.Count:N0} scores. Still in order? {(IsInOrder() ? "Yes" : "No!")}";
            Show(place, place);
        }

        // ---------------------------------------------------------------- helpers (part of the starter)

        // findRank is the method to use: LinearRank or BinaryRank.
        private void Check(string name, System.Func<int, int> findRank)
        {
            if (!ReadScore(out int score)) return;
            checks = 0;
            int place = findRank(score);
            if (place < 0)
            {
                messageText.text = $"{name}Rank isn't written yet (step A4).";
                return;
            }
            messageText.text = $"{name} search: {score:N0} would be rank {place + 1}.\nIt took {Count(checks, "check")}.";
            Show(place, -1);
        }

        /// <summary>"1 check" or "2 checks": a number and a word, with an s unless the number is 1.</summary>
        private static string Count(int n, string word) => n == 1 ? $"{n} {word}" : $"{n} {word}s";

        private bool ReadScore(out int score)
        {
            if (int.TryParse(scoreInput.text, out score) && score >= 0) return true;
            messageText.text = "Type a score (a whole number) first.";
            return false;
        }

        private bool IsInOrder()
        {
            for (int i = 1; i < entries.Count; i++)
            {
                if (entries[i - 1].score < entries[i].score) return false;
            }
            return true;
        }

        /// <summary>Draw a few rows of the board around one place, highlighting one row (or -1 for none).</summary>
        private void Show(int around, int highlight)
        {
            int first = Mathf.Clamp(around - RowsShown / 2, 0, Mathf.Max(0, entries.Count - RowsShown));
            int last = Mathf.Min(entries.Count, first + RowsShown);
            string ranks = "";
            string initials = "";
            string scores = "";
            for (int i = first; i < last; i++)
            {
                string colour = i == highlight ? HighlightColour : "#FFFFFF";
                ranks += $"<color={colour}>{i + 1}.</color>\n";
                initials += $"<color={colour}>{entries[i].initials}</color>\n";
                scores += $"<color={colour}>{entries[i].score:N0}</color>\n";
            }
            ranksText.text = ranks;
            initialsText.text = initials;
            scoresText.text = scores;
        }

        // ---------------------------------------------------------------- your algorithms

        /// <summary>Step A4: where a new score would go (0 = the top), checking each entry from the top down.</summary>
        private int LinearRank(int score)
        {
            return -1;   // replace this in step A4
        }

        /// <summary>Step A4: where a new score would go (0 = the top), using binary search.</summary>
        private int BinaryRank(int score)
        {
            return -1;   // replace this in step A4
        }
    }
}
