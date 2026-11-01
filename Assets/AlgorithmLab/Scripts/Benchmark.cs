using UnityEngine;
using UnityEngine.UI;

namespace AlgorithmLab
{
    /// <summary>Times each algorithm on bigger and bigger lists. Step A5 replaces this whole script.</summary>
    public sealed class Benchmark : MonoBehaviour
    {
        [SerializeField] private Text statusText;
        [SerializeField] private Text[] columns;

        public void Run()
        {
            statusText.text = "The benchmark comes in step A5.";
        }
    }
}
