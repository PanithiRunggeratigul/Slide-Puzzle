using UnityEngine;

namespace SlidePuzzle
{
    public abstract class PuzzlePieceProperties : MonoBehaviour
    {
        public int pieceID { get; set; }

        public abstract void OnClick();
        public abstract void UnClick();
        public abstract void OnHover();
        public abstract void OnEndHover();
    }
}
