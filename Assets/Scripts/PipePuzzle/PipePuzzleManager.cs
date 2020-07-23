using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Modified version of PipePuzzleManager
public class PipePuzzleManager : PuzzleManager
{
    [System.Serializable]
    public class PipePuzzle
    {
        public int winValue;
        public int currentValue;

        public int width;
        public int height;

        public PipeReactor[,] pipes;
    }
    public PipePuzzle pipePuzzle;
    public bool isReactor = false;

    public override void PuzzleStart()
    {
        base.PuzzleStart();
        if (!isReactor) return;
        InitializePipes();
        Shuffle();
        pipePuzzle.winValue = GetWinValue();
        pipePuzzle.currentValue = Sweep();
    }

    // public override void Start()
    // {
    //     base.Start();
    //     InitializePipes();
    //     Shuffle();
    //     pipePuzzle.winValue = GetWinValue();
    //     pipePuzzle.currentValue = Sweep();
    // }
    
    void Shuffle() // rotate pipes randomly
    {
        foreach (PipeReactor pipe in pipePuzzle.pipes)
        {
            int k = UnityEngine.Random.Range(0, 4);

            for (int i = 0; i < k; i++)
            {
                pipe.Rotate();
            }
        }
    }

    void InitializePipes()
    {
        //int dimensions = (int)Mathf.Sqrt(transform.childCount);
        int dimensions = 4;

        pipePuzzle.width = dimensions;
        pipePuzzle.height = dimensions;

        int indexCounter = 0;

        pipePuzzle.pipes = new PipeReactor[pipePuzzle.width, pipePuzzle.height];

        for (int h = 0; h < dimensions; h++)
        {
            for (int w = 0; w < dimensions; w++)
            {
                //pipePuzzle.pipes[w, h] = transform.GetChild(indexCounter).GetComponent<PipeReactor>();

                if (transform.GetChild(indexCounter).GetComponent<PipeReactor>())
                {
                    pipePuzzle.pipes[w, h] = transform.GetChild(indexCounter).GetComponent<PipeReactor>();
                }
                
                indexCounter++;
            }
        }
    }
    
    int GetWinValue() // set required connections to solve puzzle
    {
        int winValue = 0;
        foreach (PipeReactor pipe in pipePuzzle.pipes)
        {
            foreach (int i in pipe.values)
            {
                winValue += i;
            }
        }

        winValue /= 2;

        return winValue;
    }

    public int Sweep() // check every pipes for connections
    {
        int value = 0;

        for (int h = 0; h < pipePuzzle.height; h++)
        {
            for (int w = 0; w < pipePuzzle.width; w++)
            {
                if (h != pipePuzzle.height - 1) //compare top
                {
                    if (pipePuzzle.pipes[w, h].values[0] == 1 && pipePuzzle.pipes[w, h + 1].values[2] == 1)
                    {
                        value++;
                        
                    }
                }
                if (w != pipePuzzle.width - 1) //compare right
                {
                    if (pipePuzzle.pipes[w, h].values[1] == 1 && pipePuzzle.pipes[w + 1, h].values[3] == 1)
                    {
                        value++;
                    }
                }
            }
        }

        return value;
    }

    public void CheckForWin()
    {
        if (pipePuzzle.currentValue == pipePuzzle.winValue)
        {
            _puzzleModule.ModuleComplete();
        }
    }
}


