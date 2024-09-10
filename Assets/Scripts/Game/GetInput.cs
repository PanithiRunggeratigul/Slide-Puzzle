using UnityEngine;

namespace SlidePuzzle
{
    using PuzzleUtils;
    public class GetInput : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;
        [SerializeField] private string puzzlePieceTag = "PuzzlePiece";
        [SerializeField] private MainGame game;

        private Vector2Int selectedPiecePos = new Vector2Int(-1, -1);
        private GameObject selectedPieceObject = null;
        private PuzzleMaker puzzleMaker;
        private GameObject currentlyHoveredPiece = null;

        private void Start()
        {
            puzzleMaker = FindObjectOfType<PuzzleMaker>();
        }

        void Update()
        {
            if (PuzzleUtilities.win)
            {
                game.WinGame();
                return;
            }

            if (game.isPause)
            {
                return;
            }

            HandleMouseHover();

            if (Input.GetMouseButtonDown(0))
            {
                HandleMouseClick();
            }
        }

        private void HandleMouseHover()
        {
            Vector3 mousePos = Input.mousePosition;
            Ray myRay = mainCamera.ScreenPointToRay(mousePos);
            RaycastHit raycastHit;

            if (Physics.Raycast(myRay, out raycastHit, Mathf.Infinity))
            {
                if (raycastHit.transform.CompareTag(puzzlePieceTag))
                {
                    GameObject hoveredPiece = raycastHit.transform.gameObject;

                    if (hoveredPiece != currentlyHoveredPiece)
                    {
                        if (currentlyHoveredPiece != null)
                        {
                            EndHoverOnPiece(currentlyHoveredPiece);
                        }
                        currentlyHoveredPiece = hoveredPiece;
                        StartHoverOnPiece(hoveredPiece);
                    }
                }
                else
                {
                    if (currentlyHoveredPiece != null)
                    {
                        EndHoverOnPiece(currentlyHoveredPiece);
                        currentlyHoveredPiece = null;
                    }
                }
            }
            else
            {
                if (currentlyHoveredPiece != null)
                {
                    EndHoverOnPiece(currentlyHoveredPiece);
                    currentlyHoveredPiece = null;
                }
            }
        }

        private void StartHoverOnPiece(GameObject piece)
        {
            PuzzlePieceProperties puzzlePiece = piece.GetComponent<PuzzlePieceProperties>();
            if (puzzlePiece != null)
            {
                puzzlePiece.OnHover();
            }
        }

        private void EndHoverOnPiece(GameObject piece)
        {
            PuzzlePieceProperties puzzlePiece = piece.GetComponent<PuzzlePieceProperties>();
            if (puzzlePiece != null)
            {
                puzzlePiece.OnEndHover();
            }
        }

        private void HandleMouseClick()
        {
            Vector3 mousePos = Input.mousePosition;
            Ray myRay = mainCamera.ScreenPointToRay(mousePos);
            RaycastHit raycastHit;

            if (Physics.Raycast(myRay, out raycastHit, Mathf.Infinity))
            {
                if (raycastHit.transform.CompareTag(puzzlePieceTag))
                {
                    PuzzlePieceProperties puzzlePiece = raycastHit.transform.GetComponent<PuzzlePieceProperties>();
                    Vector2Int clickedPiecePos = FindPiecePosition(raycastHit.transform.gameObject);
                    Vector2Int emptyPiecePos = FindEmptyPiecePosition();

                    if (puzzlePiece != null)
                    {
                        if (selectedPiecePos == new Vector2Int(-1, -1))
                        {
                            SelectPiece(clickedPiecePos, raycastHit.transform.gameObject);
                        }
                        else if (clickedPiecePos == selectedPiecePos)
                        {
                            if (selectedPieceObject.GetComponent<PuzzlePiece>() != null)
                            {
                                UnselectPiece();
                            }
                        }
                        else if (raycastHit.transform.GetComponent<EmptyPuzzlePiece>() != null)
                        {
                            if (PuzzleUtilities.CheckMoveable(puzzleMaker.puzzleGrid, selectedPiecePos, emptyPiecePos))
                            {
                                PuzzleUtilities.SwapPieces(puzzleMaker.puzzleGrid, selectedPiecePos, emptyPiecePos, true);
                                UnselectPiece();
                            }
                            else
                            {
                                UnselectPiece();
                            }
                        }
                        else
                        {
                            UnselectPiece();
                            SelectPiece(clickedPiecePos, raycastHit.transform.gameObject);
                        }
                    }
                }
                else
                {
                    if (selectedPiecePos != new Vector2Int(-1, -1))
                    {
                        UnselectPiece();
                    }
                }
            }
            else
            {
                if (selectedPiecePos != new Vector2Int(-1, -1))
                {
                    UnselectPiece();
                }
            }
        }

        private void SelectPiece(Vector2Int piecePos, GameObject pieceObject)
        {
            selectedPiecePos = piecePos;
            selectedPieceObject = pieceObject;
            PuzzlePieceProperties puzzlePiece = pieceObject.GetComponent<PuzzlePieceProperties>();
            if (puzzlePiece != null)
            {
                puzzlePiece.OnClick();
            }
        }

        private void UnselectPiece()
        {
            if (selectedPieceObject != null)
            {
                PuzzlePieceProperties puzzlePiece = selectedPieceObject.GetComponent<PuzzlePieceProperties>();
                if (puzzlePiece != null)
                {
                    puzzlePiece.UnClick();
                }
                selectedPiecePos = new Vector2Int(-1, -1);
                selectedPieceObject = null;
            }
        }

        private Vector2Int FindPiecePosition(GameObject piece)
        {
            for (int y = 0; y < puzzleMaker.panelLength; y++)
            {
                for (int x = 0; x < puzzleMaker.panelWidth; x++)
                {
                    if (puzzleMaker.puzzleGrid[x, y] == piece)
                    {
                        return new Vector2Int(x, y);
                    }
                }
            }
            return new Vector2Int(-1, -1);
        }

        private Vector2Int FindEmptyPiecePosition()
        {
            for (int y = 0; y < puzzleMaker.panelLength; y++)
            {
                for (int x = 0; x < puzzleMaker.panelWidth; x++)
                {
                    if (puzzleMaker.puzzleGrid[x, y].GetComponent<EmptyPuzzlePiece>() != null)
                    {
                        return new Vector2Int(x, y);
                    }
                }
            }
            return new Vector2Int(-1, -1);
        }
    }
}