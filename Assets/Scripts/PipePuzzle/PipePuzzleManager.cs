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

    private void Start()
    {
        InitializePipes();
        pipePuzzle.winValue = GetWinValue();
        pipePuzzle.currentValue = Sweep();
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

    void InitializePipes()
    {
        int dimensions = (int)Mathf.Sqrt(transform.childCount);

        pipePuzzle.width = dimensions;
        pipePuzzle.height = dimensions;

        int indexCounter = 0;

        for (int h = 0; h < dimensions; h++)
        {
            for (int w = 0; w < dimensions; w++)
            {
                pipePuzzle.pipes[h, w] = transform.GetChild(indexCounter).GetComponent<PipeReactor>();
                indexCounter++;
            }
        }
    }

}


