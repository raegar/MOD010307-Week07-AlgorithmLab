using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace AlgorithmLab
{
    /// <summary>
    /// Friday's algorithm exercises. Add this component to any GameObject, press Play, then run each drill
    /// from the component's ⋮ menu. For each one: PREDICT what the Console will show, RUN it, EXPLAIN what
    /// happened, then FIX it. Drills 1 to 4 have a bug on purpose.
    /// </summary>
    public sealed class AlgorithmDrills : MonoBehaviour
    {
        [ContextMenu("1 Where is the key?")]
        private void WhereIsTheKey()
        {
            string[] backpack = { "Map", "Rope", "Lamp", "Coin" };
            int index = IndexOf(backpack, "Key");
            Debug.Log($"The key is at index {index}: {backpack[index]}");
        }

        private static int IndexOf(string[] items, string wanted)
        {
            int found = 0;
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i] == wanted) found = i;
            }
            return found;
        }

        [ContextMenu("2 Is 42 in the list?")]
        private void IsFortyTwoThere()
        {
            int[] numbers = { 8, 42, 15, 4, 23, 16 };
            int index = BinarySearch(numbers, 42);
            Debug.Log(index >= 0 ? $"42 is at index {index}" : "42 is not in the list");
        }

        private static int BinarySearch(int[] list, int target)
        {
            int low = 0;
            int high = list.Length;
            while (low < high)
            {
                int mid = (low + high) / 2;
                if (list[mid] < target) low = mid + 1;
                else high = mid;
            }
            return low < list.Length && list[low] == target ? low : -1;
        }

        [ContextMenu("3 Find the top score")]
        private void FindTopScore()
        {
            int[] scores = { 10, 20, 30, 40 };   // sorted, smallest first
            int target = 40;
            int low = 0;
            int high = scores.Length - 1;
            int steps = 0;
            while (low < high)
            {
                int mid = (low + high) / 2;
                Debug.Log($"low = {low}, mid = {mid}, high = {high}");
                if (scores[mid] < target) low = mid;
                else high = mid;

                // A safety net, so a loop that never ends can't freeze Unity.
                if (++steps > 10)
                {
                    Debug.LogError("Stopped after 10 steps: this loop was never going to end.");
                    return;
                }
            }
            Debug.Log($"{target} is at index {low}");
        }

        [ContextMenu("4 Sort the fish sizes")]
        private void SortFishSizes()
        {
            int[] sizes = { 5, 3, 9, 1 };
            for (int i = 0; i < sizes.Length - 1; i++)
            {
                int smallest = i;
                for (int j = i + 1; j < sizes.Length - 1; j++)
                {
                    if (sizes[j] < sizes[smallest]) smallest = j;
                }
                (sizes[i], sizes[smallest]) = (sizes[smallest], sizes[i]);
            }
            Debug.Log("Sorted: " + string.Join(", ", sizes));
        }

        [ContextMenu("5 How many comparisons?")]
        private void HowManyComparisons()
        {
            // Predict all three numbers before you run this.
            foreach (int n in new[] { 10, 20, 40 })
            {
                int comparisons = 0;
                for (int i = 0; i < n - 1; i++)
                {
                    for (int j = i + 1; j < n; j++)
                    {
                        comparisons++;
                    }
                }
                Debug.Log($"n = {n}: {comparisons} comparisons");
            }
        }

        [ContextMenu("6 Remove duplicate names (List)")]
        private void RemoveDuplicatesWithList()
        {
            List<string> names = MakeNames();
            Stopwatch stopwatch = Stopwatch.StartNew();

            List<string> unique = new List<string>();
            foreach (string name in names)
            {
                if (!unique.Contains(name)) unique.Add(name);
            }

            stopwatch.Stop();
            Debug.Log($"List: {unique.Count:N0} different names out of {names.Count:N0}, in {stopwatch.Elapsed.TotalMilliseconds:0.0} ms");
        }

        [ContextMenu("7 Remove duplicate names (HashSet: write this one)")]
        private void RemoveDuplicatesWithHashSet()
        {
            List<string> names = MakeNames();
            Stopwatch stopwatch = Stopwatch.StartNew();

            List<string> unique = new List<string>();
            // TODO: do the same job as drill 6, but keep a HashSet<string> of the names you've already seen,
            // and check that instead of unique.Contains.

            stopwatch.Stop();
            Debug.Log($"HashSet: {unique.Count:N0} different names out of {names.Count:N0}, in {stopwatch.Elapsed.TotalMilliseconds:0.0} ms");
        }

        // 10,000 player names, with lots of repeats. A fixed seed makes it the same list every time.
        private static List<string> MakeNames()
        {
            System.Random random = new System.Random(7);
            List<string> names = new List<string>();
            for (int i = 0; i < 10000; i++) names.Add("Player" + random.Next(4000));
            return names;
        }
    }
}
