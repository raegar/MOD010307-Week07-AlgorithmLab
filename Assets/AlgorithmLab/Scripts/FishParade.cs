using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AlgorithmLab
{
    /// <summary>
    /// A column of fish of different sizes, and the algorithms that search and sort them.
    /// The starter does the setting up and the drawing. You write the four methods at the bottom.
    /// </summary>
    public sealed class FishParade : MonoBehaviour
    {
        // Slot 0 is at the top of the screen; each slot is a little lower than the one before.
        private const float TopY = 2.45f;
        private const float SlotGap = 0.72f;
        private const float LeftX = -0.8f;
        private static readonly string[] Arrangements = { "Shuffled", "Nearly sorted", "Reversed", "Already sorted" };

        [SerializeField] private LabFish fishPrefab;

        [Tooltip("One sprite for each FishKind, in the same order as the enum.")]
        [SerializeField] private Sprite[] kindSprites;

        [SerializeField, Range(2, 10)] private int fishCount = 10;

        [Tooltip("Seconds between the steps of a sort, so you can watch it work.")]
        [SerializeField, Range(0f, 1f)] private float stepDelay = 0.35f;

        [Header("UI")]
        [SerializeField] private Text messageText;
        [SerializeField] private Text countersText;
        [SerializeField] private Text selectionResults;
        [SerializeField] private Text insertionResults;

        // The parade, top to bottom: fish[0] is in the top slot.
        private readonly List<LabFish> fish = new List<LabFish>();

        // How much work did the algorithm do? Your code adds 1 to checks for every comparison.
        private int checks;
        private int swaps;

        private string arrangement = "Shuffled";
        private FishKind lookingFor = FishKind.Blue;
        private bool sorting;

        // The last result for each algorithm and arrangement, e.g. "Insertion Reversed" -> "45 / 45".
        private readonly Dictionary<string, string> results = new Dictionary<string, string>();

        private void Start()
        {
            // Ten different sizes between 10 and 99, so no two fish are the same size.
            List<int> sizes = new List<int>();
            while (sizes.Count < fishCount)
            {
                int size = Random.Range(10, 100);
                if (!sizes.Contains(size)) sizes.Add(size);
            }

            for (int i = 0; i < fishCount; i++)
            {
                FishKind kind = (FishKind)Random.Range(0, kindSprites.Length);
                LabFish newFish = Instantiate(fishPrefab, transform);
                newFish.Setup(kind, kindSprites[(int)kind], sizes[i]);
                fish.Add(newFish);
            }
            Place(true);
            ShowCounters();
            ShowResults();
            messageText.text = "Shuffle the parade, then search it or sort it.";
        }

        private void Update()
        {
            if (sorting) ShowCounters();
        }

        // ---------------------------------------------------------------- buttons (part of the starter)

        public void Shuffle()
        {
            if (sorting) return;
            // Fisher-Yates shuffle: swap each place with a random place at or above it.
            for (int i = fish.Count - 1; i > 0; i--)
            {
                int j = Random.Range(0, i + 1);
                (fish[i], fish[j]) = (fish[j], fish[i]);
            }
            Arrange("Shuffled");
        }

        public void NearlySorted()
        {
            if (sorting) return;
            fish.Sort((a, b) => a.Size.CompareTo(b.Size));
            // Swap two neighbouring pairs, so just a couple of fish are out of place.
            int first = Random.Range(0, fish.Count - 1);
            int second = (first + fish.Count / 2) % (fish.Count - 1);
            (fish[first], fish[first + 1]) = (fish[first + 1], fish[first]);
            (fish[second], fish[second + 1]) = (fish[second + 1], fish[second]);
            Arrange("Nearly sorted");
        }

        public void Reversed()
        {
            if (sorting) return;
            fish.Sort((a, b) => b.Size.CompareTo(a.Size));
            Arrange("Reversed");
        }

        public void Find()
        {
            if (sorting) return;
            ResetMarks();
            checks = 0;
            swaps = 0;
            int index = IndexOfKind(lookingFor);
            if (index >= 0)
            {
                fish[index].SetMark(Mark.Found);
                messageText.text = $"Looking for {lookingFor}: found at index {index} after {Count(checks, "check")}.";
            }
            else
            {
                messageText.text = $"Looking for {lookingFor}: not found (-1) after {Count(checks, "check")}.";
            }
            ShowCounters();

            // Next time, look for the next kind of fish.
            lookingFor = (FishKind)(((int)lookingFor + 1) % kindSprites.Length);
        }

        public void Biggest()
        {
            if (sorting) return;
            ResetMarks();
            checks = 0;
            swaps = 0;
            int index = IndexOfBiggest();
            if (index >= 0)
            {
                fish[index].SetMark(Mark.Found);
                messageText.text = $"The biggest fish is size {fish[index].Size}, at index {index} ({Count(checks, "check")}).";
            }
            else
            {
                messageText.text = "IndexOfBiggest isn't written yet (step A1).";
            }
            ShowCounters();
        }

        public void SortSelection()
        {
            if (!sorting) StartCoroutine(RunSort("Selection", SelectionSort()));
        }

        public void SortInsertion()
        {
            if (!sorting) StartCoroutine(RunSort("Insertion", InsertionSort()));
        }

        // ---------------------------------------------------------------- helpers (part of the starter)

        /// <summary>Swap the fish at two indexes: in the List, and on the screen.</summary>
        private void Swap(int a, int b)
        {
            (fish[a], fish[b]) = (fish[b], fish[a]);
            swaps++;
            Place(false);
        }

        private IEnumerator RunSort(string algorithm, IEnumerator sort)
        {
            sorting = true;
            ResetMarks();
            checks = 0;
            swaps = 0;
            messageText.text = $"{algorithm} sort on {arrangement.ToLower()} fish...";

            yield return StartCoroutine(sort);
            yield return new WaitForSeconds(stepDelay);

            sorting = false;
            ShowCounters();
            if (!IsSorted())
            {
                messageText.text = $"{algorithm} sort finished, but the fish aren't in order yet. Has it been written?";
                yield break;
            }

            foreach (LabFish f in fish) f.SetMark(Mark.Sorted);
            messageText.text = $"{algorithm} sort on {arrangement.ToLower()} fish: {Count(checks, "check")}, {Count(swaps, "swap")}.";
            results[algorithm + " " + arrangement] = $"{checks} / {swaps}";
            ShowResults();
            arrangement = "Already sorted";
        }

        private bool IsSorted()
        {
            for (int i = 1; i < fish.Count; i++)
            {
                if (fish[i - 1].Size > fish[i].Size) return false;
            }
            return true;
        }

        private void Arrange(string name)
        {
            arrangement = name;
            ResetMarks();
            Place(false);
            messageText.text = $"{name}. Now search it or sort it.";
        }

        /// <summary>Move every fish to the slot that matches its index in the List.</summary>
        private void Place(bool instantly)
        {
            for (int i = 0; i < fish.Count; i++)
            {
                fish[i].MoveTo(new Vector3(LeftX, TopY - i * SlotGap, 0f), instantly);
            }
        }

        private void ResetMarks()
        {
            foreach (LabFish f in fish) f.SetMark(Mark.None);
        }

        /// <summary>"1 check" or "2 checks": a number and a word, with an s unless the number is 1.</summary>
        private static string Count(int n, string word) => n == 1 ? $"{n} {word}" : $"{n} {word}s";

        private void ShowCounters()
        {
            countersText.text = $"Checks: {checks}     Swaps: {swaps}";
        }

        private void ShowResults()
        {
            string selection = "Selection\n";
            string insertion = "Insertion\n";
            foreach (string row in Arrangements)
            {
                selection += (results.TryGetValue("Selection " + row, out string s) ? s : "-") + "\n";
                insertion += (results.TryGetValue("Insertion " + row, out string n) ? n : "-") + "\n";
            }
            selectionResults.text = selection;
            insertionResults.text = insertion;
        }

        // ---------------------------------------------------------------- your algorithms

        /// <summary>Step A1: the index of the first fish of this kind, or -1 if there isn't one.</summary>
        private int IndexOfKind(FishKind kind)
        {
            // Linear search: look at each fish in turn, from index 0 to the end.
            for (int i = 0; i < fish.Count; i++)
            {
                checks++;
                fish[i].SetMark(Mark.Checked);
                if (fish[i].Kind == kind) return i;   // found it: stop looking
            }
            return -1;   // looked at every fish, and none matched
        }

        /// <summary>Step A1: the index of the biggest fish.</summary>
        private int IndexOfBiggest()
        {
            // Best so far: start by assuming the first fish is the biggest...
            int biggest = 0;
            for (int i = 1; i < fish.Count; i++)
            {
                checks++;
                // ...then compare each of the others with the best so far.
                if (fish[i].Size > fish[biggest].Size) biggest = i;
            }
            return biggest;
        }

        /// <summary>Step A2: sort the fish from smallest (index 0) to biggest, using selection sort.</summary>
        private IEnumerator SelectionSort()
        {
            for (int i = 0; i < fish.Count - 1; i++)
            {
                // Find the smallest fish from index i to the end. (The same search as IndexOfBiggest, but smallest.)
                int smallest = i;
                for (int j = i + 1; j < fish.Count; j++)
                {
                    checks++;
                    if (fish[j].Size < fish[smallest].Size) smallest = j;
                }

                // Swap it into place i. Now everything from 0 to i is sorted, and never moves again.
                if (smallest != i)
                {
                    Swap(i, smallest);
                    yield return new WaitForSeconds(stepDelay);
                }
                fish[i].SetMark(Mark.Sorted);
            }
        }

        /// <summary>Step A3: sort the fish from smallest (index 0) to biggest, using insertion sort.</summary>
        private IEnumerator InsertionSort()
        {
            yield break;   // replace this in step A3
        }
    }
}
