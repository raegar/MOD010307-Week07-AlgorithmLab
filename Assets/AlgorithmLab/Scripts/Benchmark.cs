using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

namespace AlgorithmLab
{
    /// <summary>
    /// Times each algorithm on bigger and bigger lists of numbers, to see how the time grows with N.
    /// The algorithms are the same ones you wrote for the fish and the leaderboard, working on ints.
    /// </summary>
    public sealed class Benchmark : MonoBehaviour
    {
        [SerializeField] private Text statusText;

        [Tooltip("One Text per column: N, linear search, binary search, selection sort, insertion sort, List.Sort.")]
        [SerializeField] private Text[] columns;

        [Tooltip("The list sizes to try.")]
        [SerializeField] private int[] sizes = { 1000, 10000, 100000 };

        [Tooltip("Selection and insertion sort are skipped above this size: 100,000 items would take minutes.")]
        [SerializeField] private int slowSortLimit = 10000;

        [Tooltip("How many searches to time at each size. One search is too quick to measure on its own.")]
        [SerializeField] private int searches = 1000;

        private static readonly string[] Headings = { "N\n(items)", "Linear\nsearch", "Binary\nsearch", "Selection\nsort", "Insertion\nsort", "List\n.Sort" };

        private bool running;

        public void Run()
        {
            if (!running) StartCoroutine(RunAll());
        }

        private IEnumerator RunAll()
        {
            running = true;
            for (int c = 0; c < columns.Length; c++) columns[c].text = Headings[c] + "\n";

            // Warm up: the first run of any method is slow while it is compiled, so run each one once on a tiny list.
            TimeRow(100, false);

            foreach (int n in sizes)
            {
                statusText.text = $"Timing N = {n:N0}...";
                yield return null;   // let the screen update before the slow part

                string[] row = TimeRow(n, true);
                for (int c = 0; c < columns.Length; c++) columns[c].text += row[c] + "\n";
                Debug.Log(string.Join("  |  ", row));
                yield return null;
            }

            statusText.text = $"Done. Each search column is {searches:N0} searches; each sort is one sort.";
            running = false;
        }

        private string[] TimeRow(int n, bool show)
        {
            List<int> numbers = RandomNumbers(n);
            List<int> sorted = new List<int>(numbers);
            sorted.Sort();
            List<int> targets = new List<int>();
            for (int i = 0; i < searches; i++) targets.Add(numbers[Random.Range(0, n)]);

            string linear = Measure(() => { foreach (int t in targets) LinearSearch(sorted, t); });
            string binary = Measure(() => { foreach (int t in targets) BinarySearch(sorted, t); });
            string selection = n <= slowSortLimit ? Measure(() => SelectionSort(new List<int>(numbers))) : "skipped";
            string insertion = n <= slowSortLimit ? Measure(() => InsertionSort(new List<int>(numbers))) : "skipped";
            string listSort = Measure(() => new List<int>(numbers).Sort());
            return new[] { n.ToString("N0"), linear, binary, selection, insertion, listSort };
        }

        /// <summary>Run some code once and return how long it took, in milliseconds.</summary>
        private static string Measure(System.Action code)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            code();
            stopwatch.Stop();
            return $"{stopwatch.Elapsed.TotalMilliseconds:0.00} ms";
        }

        private static List<int> RandomNumbers(int n)
        {
            List<int> numbers = new List<int>(n);
            for (int i = 0; i < n; i++) numbers.Add(Random.Range(0, 1000000));
            return numbers;
        }

        // ---------------------------------------------------------------- the algorithms, on ints

        // O(n): looks at every item in the worst case.
        private static int LinearSearch(List<int> list, int target)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] == target) return i;
            }
            return -1;
        }

        // O(log n): halves what's left every time. The list must be sorted (smallest first here).
        private static int BinarySearch(List<int> list, int target)
        {
            int low = 0;
            int high = list.Count;
            while (low < high)
            {
                int mid = (low + high) / 2;
                if (list[mid] < target) low = mid + 1;
                else high = mid;
            }
            return low < list.Count && list[low] == target ? low : -1;
        }

        // O(n²): a loop inside a loop, whatever order the list starts in.
        private static void SelectionSort(List<int> list)
        {
            for (int i = 0; i < list.Count - 1; i++)
            {
                int smallest = i;
                for (int j = i + 1; j < list.Count; j++)
                {
                    if (list[j] < list[smallest]) smallest = j;
                }
                (list[i], list[smallest]) = (list[smallest], list[i]);
            }
        }

        // O(n²) on shuffled data, but close to O(n) when the list is nearly sorted already.
        private static void InsertionSort(List<int> list)
        {
            for (int i = 1; i < list.Count; i++)
            {
                int j = i;
                while (j > 0 && list[j - 1] > list[j])
                {
                    (list[j - 1], list[j]) = (list[j], list[j - 1]);
                    j--;
                }
            }
        }
    }
}
