using System.Collections;
using TMPro;
using UnityEngine;

namespace SlidePuzzle
{
    public class PuzzlePiece : PuzzlePieceProperties
    {
        private bool isSelected = false;

        [SerializeField] private TextMeshProUGUI pieceText;
        // [SerializeField] private Image img;
        [SerializeField] private GameObject border;
        [SerializeField] private GameObject hoverBorder;

        private void Start()
        {
            pieceText.text = pieceID.ToString();
            border.SetActive(false);
            hoverBorder.SetActive(false);
        }

        public override void OnClick()
        {
            if (isSelected)
            {
                UnClick();
            }
            else
            {
                Debug.Log("Selected piece ID: " + pieceID);
                border.SetActive(true);
                isSelected = true;
            }
        }

        public override void UnClick()
        {
            Debug.Log("Unselected piece ID: " + pieceID);
            border.SetActive(false);
            isSelected = false;
        }

        public void SlideAnim(Vector3 newPos)
        {
            StartCoroutine(SlideCoroutine(newPos));
        }

        private IEnumerator SlideCoroutine(Vector3 newPos)
        {
            float duration = 0.5f;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / duration;

                this.transform.position = Vector3.Lerp(this.transform.position, newPos, t);

                yield return null;
            }

            this.transform.position = newPos;
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
