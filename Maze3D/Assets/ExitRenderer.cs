using System.Collections;
using System.Collections.Generic;
using Assets;
using UnityEngine;

public class ExitRenderer : MonoBehaviour
{
    public MazeSpawner MazeSpawner;

    private LineRenderer componentLineRenderer;

    private void Start()
    {
        componentLineRenderer = GetComponent<LineRenderer>();
    }

    public void DrawExit()
    {
        Maze maze = MazeSpawner.maze;
        Vector2Int currentPosition = maze.finishPosition; //получаем позицию финиша в лабиринте
        List<Vector3> positions = new List<Vector3>(); //список позиций для генерации линии

        while (currentPosition != Vector2Int.zero && positions.Count < 10000) //идем к старту и создаем линию (от финиша к старту)
        {
            int x = currentPosition.x;
            int y = currentPosition.y;
            positions.Add(new Vector3(x * MazeSpawner.CellSize.x, y * MazeSpawner.CellSize.y, y * MazeSpawner.CellSize.z));

            MazeGeneratorCell currentCell = maze.cells[currentPosition.x, currentPosition.y];

            //ищем соседей с размерностью дистанции меньшей на один, пока не придем в финиш
            if (currentPosition.x > 0 && //переход влево
                !currentCell.WallLeft &&
                maze.cells[currentPosition.x - 1, currentPosition.y].DistanceFromStart == currentCell.DistanceFromStart - 1)
            {
                currentPosition.x--;
            }
            else if (currentPosition.y > 0 && //переход вниз
                !currentCell.WallBottom &&
                maze.cells[currentPosition.x, currentPosition.y - 1].DistanceFromStart == currentCell.DistanceFromStart - 1)
            {
                currentPosition.y--;
            }
            else if (currentPosition.x < maze.cells.GetLength(0) - 1 && //переход вправо
                !maze.cells[currentPosition.x + 1, currentPosition.y].WallLeft &&
                maze.cells[currentPosition.x + 1, currentPosition.y].DistanceFromStart == currentCell.DistanceFromStart - 1)
            {
                currentPosition.x++;
            }
            else if (currentPosition.y < maze.cells.GetLength(1) - 1 && //переход вверх
                !maze.cells[currentPosition.x, currentPosition.y + 1].WallBottom &&
                maze.cells[currentPosition.x, currentPosition.y + 1].DistanceFromStart == currentCell.DistanceFromStart - 1)
            {
                currentPosition.y++;
            }

        }

        positions.Add(Vector3.zero);
        componentLineRenderer.positionCount = positions.Count;
        componentLineRenderer.SetPositions(positions.ToArray());
    }
}
