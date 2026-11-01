using UnityEngine;

namespace AlgorithmLab
{
    public enum FishKind { Blue, Green, Orange, Pink, Red, Puffer }

    /// <summary>How a fish is drawn: normal, already checked by a search, the one a search found, or in its sorted place.</summary>
    public enum Mark { None, Checked, Found, Sorted }

    /// <summary>
    /// One fish in the parade: its kind, its size, and how it is drawn.
    /// Part of the starter: you don't need to change this script.
    /// </summary>
    public sealed class LabFish : MonoBehaviour
    {
        private const float MoveSpeed = 12f;
        private static readonly Color Gold = new Color32(255, 213, 79, 255);
        private static readonly Color Green = new Color32(150, 235, 150, 255);
        private static readonly Color Grey = new Color32(150, 165, 185, 255);

        [SerializeField] private SpriteRenderer body;
        [SerializeField] private TextMesh label;

        private Vector3 target;

        public FishKind Kind { get; private set; }
        public int Size { get; private set; }

        public void Setup(FishKind kind, Sprite sprite, int size)
        {
            Kind = kind;
            Size = size;
            name = $"{kind} fish ({size})";
            body.sprite = sprite;

            // Sizes run from 10 to 99, so the fish are drawn from 0.45 to 1.25 units across.
            // The body is moved right by half its width, so every fish starts at the same left edge.
            float scale = Mathf.Lerp(0.45f, 1.25f, (size - 10) / 89f);
            body.transform.localScale = new Vector3(scale, scale, 1f);
            body.transform.localPosition = new Vector3(scale * 0.5f, 0f, 0f);
            label.text = size.ToString();
            SetMark(Mark.None);
        }

        /// <summary>Glide to a new place in the parade (or jump there).</summary>
        public void MoveTo(Vector3 position, bool instantly = false)
        {
            target = position;
            if (instantly) transform.position = position;
        }

        public void SetMark(Mark mark)
        {
            body.color = mark == Mark.Checked ? new Color(1f, 1f, 1f, 0.3f) : Color.white;
            label.color = mark switch
            {
                Mark.Checked => Grey,
                Mark.Found => Gold,
                Mark.Sorted => Green,
                _ => Color.white,
            };
            label.fontStyle = mark == Mark.Found ? FontStyle.Bold : FontStyle.Normal;
        }

        private void Update()
        {
            transform.position = Vector3.MoveTowards(transform.position, target, MoveSpeed * Time.deltaTime);
        }
    }
}
