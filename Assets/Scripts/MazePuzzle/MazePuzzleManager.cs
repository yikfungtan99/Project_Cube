using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class MazePuzzleManager : PuzzleManager
{
    [System.Serializable]
    public class MazePuzzle
    {
        public MazeBall mazeBall; // for ball initialization
        public int gridSize = 8; // don't change this lol it's always 8
        public MazeCell[,] mazeCells; // for cell initialization
        public HorizontalWalls[,] horizontalWalls; // for wall initialization
        public VerticalWalls[,] verticalWalls; // for wall initialization
        public int xPos = 4 - 1; // set starting ball x position
        public int yPos = 8 - 1; // set starting ball y position
        public int winXPos = 5 - 1; // set winning x position
        public int winYPos = 1 - 1; // set winning y position
    }
    public MazePuzzle mazePuzzle;
    MazeCell[] tempCells;
    HorizontalWalls[] tempWallHorizontal;
    VerticalWalls[] tempWallVertical;
    SolveButton solveButton;

    [SerializeField] private bool isInteractor;

    //-START AND UPDATE------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    public override void Start()
    {
        InitializeSolveButton();
        if (isInteractor) return;
        InitializeCells();
        InitializeWalls();
        ClearWalls();
        InitializeBall();
    }

    private void Update()
    {
        
    }
    //-Buttonn Presses---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    public void MazeButtonPress(int i) // 0 = up, 1 = right, 2 = down, 3 = left
    {
        if (isInteractor) return;
        if(i == 0) // up
        {
            UpButtonPressed();
        }
        if (i == 1) // right
        {
            RightButtonPressed();
        }
        if (i == 2) // down
        {
            DownButtonPressed();
        }
        if (i == 3) //3 left
        {
            LeftButtonPressed();
        }
    }
    #region ButtonPresses
    void UpButtonPressed()
    {
        if(mazePuzzle.yPos < mazePuzzle.gridSize-1)
        {
            if(mazePuzzle.mazeCells[mazePuzzle.xPos, mazePuzzle.yPos].values[0] == 0)
            {
                mazePuzzle.yPos += 1;
                mazePuzzle.mazeBall.SetTarget(mazePuzzle.mazeCells[mazePuzzle.xPos, mazePuzzle.yPos].gameObject);
            }
        }
    }
    void DownButtonPressed()
    {
        if (mazePuzzle.yPos > 0)
        {
            if (mazePuzzle.mazeCells[mazePuzzle.xPos, mazePuzzle.yPos - 1].values[0] == 0)
            {
                mazePuzzle.yPos -= 1;
                mazePuzzle.mazeBall.SetTarget(mazePuzzle.mazeCells[mazePuzzle.xPos, mazePuzzle.yPos].gameObject);
            }
        }
    }
    void LeftButtonPressed()
    {
        if (mazePuzzle.xPos > 0)
        {
            if (mazePuzzle.mazeCells[mazePuzzle.xPos - 1, mazePuzzle.yPos].values[1] == 0)
            {
                mazePuzzle.xPos -= 1;
                mazePuzzle.mazeBall.SetTarget(mazePuzzle.mazeCells[mazePuzzle.xPos, mazePuzzle.yPos].gameObject);
            }
        }
    }
    void RightButtonPressed()
    {
        if (mazePuzzle.xPos < mazePuzzle.gridSize-1)
        {
            if (mazePuzzle.mazeCells[mazePuzzle.xPos, mazePuzzle.yPos].values[1] == 0)
            {
                mazePuzzle.xPos += 1;
                mazePuzzle.mazeBall.SetTarget(mazePuzzle.mazeCells[mazePuzzle.xPos, mazePuzzle.yPos].gameObject);
            }
        }
    }
    #endregion
    //-Functions---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
    void InitializeSolveButton()
    {
        solveButton = GetComponentInChildren<SolveButton>();
        if(solveButton != null)
            solveButton.SetManager(this);
    }
    public override void CheckWin()
    {
        if (mazePuzzle.xPos == mazePuzzle.winXPos && mazePuzzle.yPos == mazePuzzle.winYPos)
        {
            _puzzleModule.ModuleComplete();
        }
    }

    void InitializeBall()
    {
        mazePuzzle.mazeBall = GetComponentInChildren<MazeBall>();
    }
    
    void InitializeCells()
    {
        int indexCounter = 0;
        mazePuzzle.mazeCells = new MazeCell[mazePuzzle.gridSize, mazePuzzle.gridSize];
        tempCells = GetComponentsInChildren<MazeCell>();
        for (int i = 0; i < mazePuzzle.gridSize; i++)
        {
            for (int j = 0; j < mazePuzzle.gridSize; j++)
            {
                mazePuzzle.mazeCells[j, i] = tempCells[indexCounter];
                indexCounter++;
            }
        }
    }

    void InitializeWalls()
    {
        int indexCounter = 0;
        mazePuzzle.horizontalWalls = new HorizontalWalls[mazePuzzle.gridSize, mazePuzzle.gridSize];
        tempWallHorizontal = GetComponentsInChildren<HorizontalWalls>();
        for (int i = 0; i < mazePuzzle.gridSize; i++)
        {
            for (int j = 0; j < mazePuzzle.gridSize; j++)
            {
                mazePuzzle.horizontalWalls[j, i] = tempWallHorizontal[indexCounter];
                indexCounter++;
            }
        }
        indexCounter = 0;
        mazePuzzle.verticalWalls = new VerticalWalls[mazePuzzle.gridSize, mazePuzzle.gridSize];
        tempWallVertical = GetComponentsInChildren<VerticalWalls>();
        for (int i = 0; i < mazePuzzle.gridSize; i++)
        {
            for (int j = 0; j < mazePuzzle.gridSize; j++)
            {
                mazePuzzle.verticalWalls[j, i] = tempWallVertical[indexCounter];
                indexCounter++;
            }
        }
    }

    void ClearWalls()
    {
        for (int i = 0; i < mazePuzzle.gridSize; i++)
        {
            for (int j = 0; j < mazePuzzle.gridSize; j++)
            {
                if (mazePuzzle.mazeCells[j, i].values[0] == 0) // if up has wall;
                {
                    mazePuzzle.horizontalWalls[j, i].gameObject.SetActive(false);
                }
                if (mazePuzzle.mazeCells[j, i].values[1] == 0) // if right has wall;
                {
                    mazePuzzle.verticalWalls[j, i].gameObject.SetActive(false);
                }
            }
        }
    }
}
