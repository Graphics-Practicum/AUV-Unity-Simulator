using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolGenerator : MonoBehaviour
{
    public class Cell
    {
        public bool visited = false;
        public bool[] status = new bool[4];
    }

    private Vector2 size = new Vector2(43,43);
    private int startPos = 0;
    public GameObject pool;
    private Vector2 offset  = new Vector2(43,43);
    public Vector2 totalSize;

    List<Cell> board;
    // Start is called before the first frame update
    void Start()
    {
        MazeGenerator();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GeneratePool()
    {
        for(int i= 0; i<totalSize.x; i++)
        {
            for(int j= 0; j <totalSize.y; j++)
            {
                var newPool = Instantiate(pool, new Vector3(i*offset.x, 0, -j*offset.y), Quaternion.identity, transform).GetComponent<PoolBehavior>();
                newPool.UpdatePool(board[Mathf.FloorToInt(i+j*size.x)].status);
            }
        }
    }

    void MazeGenerator()
    {
        board = new List<Cell>();

        for(int i = 0; i<size.x; i++)
        {
            for(int j = 0;j < size.y; j++)
            {
                board.Add(new Cell());
            }
        }
        int currentCell = startPos;

        Stack<int> path= new Stack<int>();

        int k = 0;

        while(k<10)
        {
            k++;

            board[currentCell].visited = true;
            List<int> neighbors = CheckNeighbors(currentCell);

            if(neighbors.Count == 0)
            {
                if(path.Count == 0)
                {
                    break;
                }
                else
                {
                    currentCell = path.Pop();
                }
            }
            else
            {
                path.Push(currentCell);

                int newCell = neighbors[Random.Range(0,neighbors.Count)];

                if(newCell > currentCell)
                {
                    if(newCell - 1 == currentCell)
                    {
                        board[currentCell].status[1] = true;
                        currentCell = newCell;
                        board[currentCell].status[3] = true;
                    }
                    else
                    {
                        board[currentCell].status[2] = true;
                        currentCell = newCell;
                        board[currentCell].status[0] = true;
                    }
                }
                else
                {
                    if(newCell - 1 == currentCell)
                    {
                        board[currentCell].status[3] = true;
                        currentCell = newCell;
                        board[currentCell].status[1] = true;
                    }
                    else
                    {
                        board[currentCell].status[0] = true;
                        currentCell = newCell;
                        board[currentCell].status[2] = true;
                    }
                }
            }

        }
        GeneratePool();
    }

    List<int> CheckNeighbors(int cell)
    {
        List<int> neighbors = new List<int>();
        if (cell - size.x >= 0 && !board[Mathf.FloorToInt(cell - size.x)].visited)
        {
            neighbors.Add(Mathf.FloorToInt(cell - size.x));
        }

        if (cell + size.x < board.Count && !board[Mathf.FloorToInt(cell + size.x)].visited)
        {
            neighbors.Add(Mathf.FloorToInt(cell + size.x));
        }

        if ((cell+1) % size.x !=0 && !board[Mathf.FloorToInt(cell + 1)].visited)
        {
            neighbors.Add(Mathf.FloorToInt(cell + 1));
        }

        if (cell % size.x !=0 && !board[Mathf.FloorToInt(cell - 1)].visited)
        {
            neighbors.Add(Mathf.FloorToInt(cell -1));
        }
        return neighbors;
    }
}
