using UnityEngine;

namespace SlidePuzzle
{
    namespace PuzzleUtils
    {
        public static class PuzzleUtilities
        {
            public static float PieceLength { get; } = 5f;

            public static bool win = false;

            public static bool CheckMoveable(GameObject[,] puzzleGrid, Vector2Int selectedPiecePos, Vector2Int emptyPiecePos)
            {
                int distanceX = Mathf.Abs(selectedPiecePos.x - emptyPiecePos.x);
                int distanceY = Mathf.Abs(selectedPiecePos.y - emptyPiecePos.y);

                return (distanceX + distanceY) == 1;
            }

            public static void SwapPieces(GameObject[,] puzzleGrid, Vector2Int pos1, Vector2Int pos2, bool shouldConsiderWinning)
            {
                GameObject temp = puzzleGrid[pos1.x, pos1.y];
                puzzleGrid[pos1.x, pos1.y] = puzzleGrid[pos2.x, pos2.y];
                puzzleGrid[pos2.x, pos2.y] = temp;

                Vector3 tempPosition = puzzleGrid[pos1.x, pos1.y].transform.position;

                puzzleGrid[pos1.x, pos1.y].transform.position = puzzleGrid[pos2.x, pos2.y].transform.position;

                if (shouldConsiderWinning)
                {
                    puzzleGrid[pos2.x, pos2.y].GetComponent<PuzzlePiece>().SlideAnim(tempPosition);

                    CheckWinCondition(puzzleGrid);
                }
                else
                {
                    puzzleGrid[pos2.x, pos2.y].transform.position = tempPosition;
                }
            }

            public static void CheckWinCondition(GameObject[,] puzzleGrid)
            {
                int idCounter = 0;
                for (int y = 0; y < puzzleGrid.GetLength(1); y++)
                {
                    for (int x = 0; x < puzzleGrid.GetLength(0); x++)
                    {
                        if (x == puzzleGrid.GetLength(0) - 1 && y == puzzleGrid.GetLength(1) - 1)
                        {
                            if (puzzleGrid[x, y].GetComponent<EmptyPuzzlePiece>() == null)
                            {
                                Debug.Log("Puzzle is not solved.");
                                return;
                            }
                        }
                        else
                        {
                            if (puzzleGrid[x, y].GetComponent<PuzzlePieceProperties>().pieceID != idCounter)
                            {
                                Debug.Log("Puzzle is not solved.");
                                return;
                            }
                            idCounter++;
                        }
                    }
                }
                Debug.Log("Puzzle is solved!");
                win = true;
            }
        }
    }
}