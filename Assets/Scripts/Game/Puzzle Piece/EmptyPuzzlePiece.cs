using UnityEngine;

namespace SlidePuzzle
{
    public class EmptyPuzzlePiece : PuzzlePieceProperties
    {
        [SerializeField] private GameObject hoverBorder;

        private void Start()
        {
            hoverBorder.SetActive(false);
        }

        public override void OnClick()
        {
            Debug.Log(pieceID);
        }

        public override void UnClick()
        {

        }

        public override void OnHover()
        {
            hoverBorder.SetActive(true);
        }
        public override void OnEndHover()
        {
            hoverBorder.SetActive(false);
        }
    }
}
