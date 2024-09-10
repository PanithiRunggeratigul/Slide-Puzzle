using System.Collections.Generic;
using UnityEngine;

namespace SlidePuzzle
{
    using PuzzleUtils;

    public class PuzzleMaker : MonoBehaviour
    {
        public int panelLength { get; }
        public int panelWidth { get; }

        private int idCounter;

        [SerializeField] private GameObject puzzlePiece;
        [SerializeField] private GameObject emptyPuzzlePiece;

        PuzzleMaker()
        {
            panelLength = 4;
            panelWidth = 4;
        }

        public GameObject[,] puzzleGrid { get; private set; }

        private GameObject[,] initialPuzzleGrid;
        private Dictionary<Vector2Int, int> initialPieceIDs = new Dictionary<Vector2Int, int>();

        public void CreatePuzzle()
        {
            puzzleGrid = new GameObject[panelLength, panelWidth];

            idCounter = 0;

            for (int y = 0; y < panelLength; y++)
            {
                for (int x = 0; x < panelWidth; x++)
                {
                    if (x == panelWidth - 1 && y == panelLength - 1)
                    {
                        puzzleGrid[x, y] = Instantiate(emptyPuzzlePiece, new Vector3(x * PuzzleUtilities.PieceLength, 0, -(y * PuzzleUtilities.PieceLength)), Quaternion.identity);
                    }
                    else
                    {
                        puzzleGrid[x, y] = Instantiate(puzzlePiece, new Vector3(x * PuzzleUtilities.PieceLength, 0, -(y * PuzzleUtilities.PieceLength)), Quaternion.identity);
                    }
                    puzzleGrid[x, y].transform.parent = transform;
                    puzzleGrid[x, y].GetComponent<PuzzlePieceProperties>().pieceID = idCounter;
                    // Debug.Log(idCounter);
                    idCounter++;
                }
            }

            ShufflePuzzle();
            initialPuzzleGrid = MemorizeInitialBoard(puzzleGrid);
        }

        public void ShufflePuzzle()
        {
            // Number of shuffle
            int shuffleMoves = Random.Range(50, 100);

            Vector2Int emptyPosition = new Vector2Int(panelWidth - 1, panelLength - 1);

            for (int i = 0; i < shuffleMoves; i++)
            {
                List<Vector2Int> possibleMoves = new List<Vector2Int>();

                if (emptyPosition.x > 0) // Can move left
                    possibleMoves.Add(new Vector2Int(emptyPosition.x - 1, emptyPosition.y));
                if (emptyPosition.x < panelWidth - 1) // Can move right
                    possibleMoves.Add(new Vector2Int(emptyPosition.x + 1, emptyPosition.y));
                if (emptyPosition.y > 0) // Can move up
                    possibleMoves.Add(new Vector2Int(emptyPosition.x, emptyPosition.y - 1));
                if (emptyPosition.y < panelLength - 1) // Can move down
                    possibleMoves.Add(new Vector2Int(emptyPosition.x, emptyPosition.y + 1));

                Vector2Int chosenMove = possibleMoves[Random.Range(0, possibleMoves.Count)];

                PuzzleUtilities.SwapPieces(puzzleGrid, emptyPosition, chosenMove, false);

                emptyPosition = chosenMove;
            }

            PuzzleUtilities.win = false;
        }

        public void RemovePuzzle()
        {
            if (puzzleGrid != null)
            {
                for (int y = 0; y < panelLength; y++)
                {
                    for (int x = 0; x < panelWidth; x++)
                    {
                        if (puzzleGrid[x, y] != null)
                        {
                            Destroy(puzzleGrid[x, y]);
                        }
                    }
                }
                puzzleGrid = new GameObject[panelLength, panelWidth];
            }
        }

        public void RestartPuzzle()
        {
            ResetBoardToInit();
        }

        public GameObject[,] MemorizeInitialBoard(GameObject[,] puzzleGrid)
        {
            initialPuzzleGrid = new GameObject[panelWidth, panelLength];

            for (int y = 0; y < panelLength; y++)
            {
                for (int x = 0; x < panelWidth; x++)
                {
                    if (puzzleGrid[x, y].GetComponent<PuzzlePiece>() != null)
                    {
                        initialPuzzleGrid[x, y] = puzzlePiece;
                    }
                    else if (puzzleGrid[x, y].GetComponent<EmptyPuzzlePiece>() != null)
                    {
                        initialPuzzleGrid[x, y] = emptyPuzzlePiece;
                    }

                    // Store ID
                    if (puzzleGrid[x, y].TryGetComponent(out PuzzlePieceProperties pieceProperties))
                    {
                        initialPieceIDs[new Vector2Int(x, y)] = pieceProperties.pieceID;
                    }
                }
            }
            return initialPuzzleGrid;
        }


        public void ResetBoardToInit()
        {
            RemovePuzzle();

            PuzzleUtilities.win = false;

            for (int y = 0; y < panelLength; y++)
            {
                for (int x = 0; x < panelWidth; x++)
                {
                    GameObject piece = Instantiate(initialPuzzleGrid[x, y], new Vector3(x * PuzzleUtilities.PieceLength, 0, -(y * PuzzleUtilities.PieceLength)), Quaternion.identity);
                    piece.transform.parent = transform;

                    if (piece.TryGetComponent(out PuzzlePieceProperties pieceProperties))
                    {
                        if (initialPieceIDs.TryGetValue(new Vector2Int(x, y), out int id))
                        {
                            pieceProperties.pieceID = id;
                        }
                    }
                    puzzleGrid[x, y] = piece;
                }
            }
        }

    }
}
