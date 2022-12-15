using System.Collections;
using System.Collections.Generic;
using Assets;
using UnityEngine;



public class MazeGenerator
{
    public int Width = 23;
    public int Height = 15;

    public Maze GenerateMaze() //создаем лабиринт
    {
        MazeGeneratorCell[,] cells = new MazeGeneratorCell[Width, Height];

        for (int x = 0; x<cells.GetLength(0); x++)
        {
            for (int y = 0; y<cells.GetLength(1); y++)
            {
                cells[x, y] = new MazeGeneratorCell { X = x, Y = y }; //заполнение массива нулями
            }
        }

        //убираем лишние стены сверху и справа
        for (int x = 0; x < cells.GetLength(0); x++)
        {
            cells[x, Height - 1].WallLeft = false;
        }

        for (int y = 0; y < cells.GetLength(1); y++)
        {
            cells[Width - 1, y].WallBottom = false;
        }

        RemoveWallsWithBacktracker(cells);

        Maze maze = new Maze();

        maze.cells = cells;
        maze.finishPosition = PlaceMazeExit(cells);

        return maze;
    }

    private void RemoveWallsWithBacktracker(MazeGeneratorCell[,] maze) //удаление стен из лабиринта
    {
        MazeGeneratorCell current = maze[0, 0];
        current.Visited = true;
        current.DistanceFromStart = 0;

        Stack<MazeGeneratorCell> stack = new Stack<MazeGeneratorCell>(); //список ячеек, через который мы прошли в ходе случайного блуждания
        do
        {
            List<MazeGeneratorCell> unvisitedNeighbours = new List<MazeGeneratorCell>(); //список всех ячеек, в которых мы еще не были

            int x = current.X;
            int y = current.Y;

            //проверка посещенности - если не посещена, вносим в список (при этом ячейка не должна быть крайней)
            if (x > 0 && !maze[x - 1, y].Visited) unvisitedNeighbours.Add(maze[x-1, y]);
            if (y > 0 && !maze[x, y - 1].Visited) unvisitedNeighbours.Add(maze[x, y - 1]);
            if (x < Width - 2 && !maze[x + 1, y].Visited) unvisitedNeighbours.Add(maze[x + 1, y]);
            if (y < Height - 2 && !maze[x, y + 1].Visited) unvisitedNeighbours.Add(maze[x, y + 1]);

            if (unvisitedNeighbours.Count > 0) //выбираем случайного соседа, куда пойдем
            {
                MazeGeneratorCell chosen = unvisitedNeighbours[UnityEngine.Random.Range(0, unvisitedNeighbours.Count)];
                RemoveWall(current, chosen); //сносим стену между текущей и выбранной ячейкой

                chosen.Visited = true; //отмечаем ячейку, чтобы еще раз через неё не пойти
                stack.Push(chosen); //добавляем её в список посещенных ячеек
                chosen.DistanceFromStart = current.DistanceFromStart + 1; 
                current = chosen; //и делаем активной
            }
            else
            {
                current = stack.Pop(); //при обратном трэкинге возвращаемся назад и удаляем ячейку из списка
            }

        } while (stack.Count > 0); //значение 0 означает, что мы вернулись в начало, тем самым построили лабиринт
    }

    private void RemoveWall(MazeGeneratorCell a, MazeGeneratorCell b)
    {
        if(a.X == b.X)
        {
            if (a.Y > b.Y) a.WallBottom = false;
            else b.WallBottom = false;
        }
        else
        {
            if (a.X > b.X) a.WallLeft = false;
            else b.WallLeft = false;
        }
    }

    private Vector2Int PlaceMazeExit(MazeGeneratorCell[,] maze)
    {
        MazeGeneratorCell furthest = maze[0,0];

        //ищем самую дальнюю ячейку на краю лабиринта
        for (int x = 0; x < maze.GetLength(0); x++)
        {
            if (maze[x, Height - 2].DistanceFromStart > furthest.DistanceFromStart) furthest = maze[x, Height - 2];
            if (maze[x, 0].DistanceFromStart > furthest.DistanceFromStart) furthest = maze[x, 0];
        }

        for (int y = 0; y < maze.GetLength(1); y++)
        {
            if (maze[Width - 2, y].DistanceFromStart > furthest.DistanceFromStart) furthest = maze[Width - 2, y];
            if (maze[0, y].DistanceFromStart > furthest.DistanceFromStart) furthest = maze[0, y];
        }

        //удаляем стену ячейки для создания выхода
        if (furthest.X == 0) furthest.WallLeft = false;
        else if (furthest.Y == 0) furthest.WallBottom = false;
        else if (furthest.X == Width - 2) maze[furthest.X + 1, furthest.Y].WallLeft = false;
        else if (furthest.Y == Height - 2) maze[furthest.X, furthest.Y+1].WallBottom = false;

        return new Vector2Int(furthest.X, furthest.Y);
    }
}
