using UnityEngine;
using System.Collections.Generic;

namespace Assets
{
    public class Maze //отдельный класс для лабиринта, чтобы вынести набор клеток и финиш (нужен для подсказки)
    {
        public MazeGeneratorCell[,] cells;
        public Vector2Int finishPosition;
        public List<Vector2Int> pills;
    }
}

public class MazeGeneratorCell //массив ячеек, который вернется в качестве лабиринта
{
    public int X;
    public int Y;

    public bool WallLeft = true; //все стены по умолчанию включены
    public bool WallBottom = true; //а затем алгоритм будет удалять стены с помощью recursive backtracking

    public bool Visited = false;
    public int DistanceFromStart;
}
