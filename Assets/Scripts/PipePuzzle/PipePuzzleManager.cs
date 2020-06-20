using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipePuzzleManager : MonoBehaviour
{
    [System.Serializable]
    public class Puzzle
    {
        public int winValue;
        public int currentValue;

        public int width;
        public int height;

        public PipeScript[,] pipes;
        public PipeButtonScript[,] buttons;
    }
    public Puzzle puzzle;

    private class AnchorTransform
    {
        public Vector3 p;
        public Quaternion q;
        public Vector3 s;
    }
    GameObject p1;
    GameObject p2;
    List<GameObject> Anchors = new List<GameObject>();
    List<AnchorTransform> AnchorT = new List<AnchorTransform>();
    
    public bool IsSolved = false;

    //-START AND UPDATE------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    void Start()
    {
        ResetTransform(); // stores transform values and reset transform values for initialization purposes

        InitializePipes();

        OriginalTransform(); // restore original transform values after initialization
        
        puzzle.winValue = GetWinValue(); // set required connections to solve puzzle
        Shuffle(); // rotate pipes randomly
        puzzle.currentValue = Sweep(); // count current connections
    }

    void Update()
    {
        
    }

    //-SETTERS AND GETTERS------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    Vector2 CheckDimensions() // ONLY WORKS ON A 1 BY 1 TRANSFORM SCALE
    {
        Vector2 aux = Vector2.zero;

        GameObject[] pipes = GameObject.FindGameObjectsWithTag("Pipes");

        foreach (var p in pipes)
        {
            if (p.transform.position.x > aux.x)
                aux.x = p.transform.position.x;

            if (p.transform.position.y > aux.y)
                aux.y = p.transform.position.y;
        }

        aux.x++;
        aux.y++;

        return aux;
    }

    int GetWinValue() // set required connections to solve puzzle
    {
        int winValue = 0;
        foreach (PipeScript pipe in puzzle.pipes)
        {
            foreach(int i in pipe.values)
            {
                winValue += i;
            }
        }

        winValue /= 2;

        return winValue;
    }

    public void SetCurrentValue() // add to current connections from a quicksweep
    {
        puzzle.currentValue = Sweep();
    }

    public void CheckWin() // use this to change state to puzzle is completed
    {
        if(puzzle.currentValue == puzzle.winValue)
        {
            IsSolved = true;
        }
    }

    public bool GetState() // use this when checking if puzzle is completed
    {
        return IsSolved;
    }

    //-FUNCTIONS------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    void Shuffle() // rotate pipes randomly
    {
        foreach (var pipe in puzzle.pipes)
        {
            int k = Random.Range(0, 4);
            
            for(int i = 0; i < k; i++)
            {
                pipe.Rotate90Degrees();
            }
        }
    }

    public int Sweep() // check every pipes for connections
    {
        int value = 0;

        for(int h = 0; h < puzzle.height; h++)
        {
            for(int w = 0; w < puzzle.width; w++)
            {
                if(h != puzzle.height - 1) //compare top
                {
                    if (puzzle.pipes[w, h].values[0] == 1 && puzzle.pipes[w, h + 1].values[2] == 1) 
                    {
                        value++;
                    }
                }
                if (w != puzzle.width - 1) //compare right
                {
                    if (puzzle.pipes[w, h].values[1] == 1 && puzzle.pipes[w + 1, h].values[3] == 1)
                    {
                        value++;
                    }
                }
            }
        }

        return value;
    }

    public void ButtonPress(int x, int y) // When button is pressed rotate the equivalent pipe and set current value
    {
        puzzle.pipes[x, y].Rotate90Degrees();
        SetCurrentValue();
    }

    void InitializePipes()
    {
        // set dimensions 
        Vector2 dimensions = CheckDimensions();

        puzzle.width = (int)dimensions.x;
        puzzle.height = (int)dimensions.y;
        
        // set pipes
        puzzle.pipes = new PipeScript[puzzle.width , puzzle.height];
        
        foreach (GameObject pipe in GameObject.FindGameObjectsWithTag("Pipes"))
        {
            puzzle.pipes[(int)pipe.transform.localPosition.x, (int)pipe.transform.localPosition.y] = pipe.GetComponent<PipeScript>();
        }

        // set buttons
        puzzle.buttons = new PipeButtonScript[puzzle.width , puzzle.height];

        foreach (GameObject button in GameObject.FindGameObjectsWithTag("PipeButtons"))
        {
            puzzle.buttons[(int)button.transform.localPosition.x, (int)button.transform.localPosition.y] = button.GetComponent<PipeButtonScript>();
        }
    }

    void ResetTransform() // Store transform values then resets it
    {
        p1 = GameObject.FindGameObjectWithTag("PipesAnchor");
        p2 = GameObject.FindGameObjectWithTag("PipeButtonsAnchor");
        Anchors.Add(p1);
        Anchors.Add(p2);
        Anchors.Add(this.gameObject);
        for (int i = 0; i < Anchors.Count; i++)
        {
            AnchorT.Add(new AnchorTransform());
        }

        for (int i = 0; i < Anchors.Count; i++)
        {
            AnchorT[i].p = Anchors[i].transform.localPosition;
            AnchorT[i].q = Anchors[i].transform.localRotation;
            AnchorT[i].s = Anchors[i].transform.localScale;

            Anchors[i].transform.localPosition = new Vector3(0, 0, 0);
            Anchors[i].transform.localRotation = new Quaternion(0, 0, 0, 0);
            Anchors[i].transform.localScale = new Vector3(1, 1, 1);
        }
    }

    void OriginalTransform() // Restore transform values to original values
    {
        for (int i = 0; i < AnchorT.Count; i++)
        {
            Anchors[i].transform.localPosition = AnchorT[i].p;
            Anchors[i].transform.localRotation = AnchorT[i].q;
            Anchors[i].transform.localScale = AnchorT[i].s;
        }
    }

    public int QuickSweep(int w, int h) // check ONLY 1 piece // ****NOT IN USE****
    {
        int value = 0;

        if (h != puzzle.height - 1) //compare top
        {
            if (puzzle.pipes[w, h].values[0] == 1 && puzzle.pipes[w, h + 1].values[2] == 1)
            {
                value++;
            }
        }
        if (w != puzzle.width - 1) //compare right
        {
            if (puzzle.pipes[w, h].values[1] == 1 && puzzle.pipes[w + 1, h].values[3] == 1)
            {
                value++;
            }
        }
        if (w != 0) // compare bottom
        {
            if (puzzle.pipes[w, h].values[2] == 1 && puzzle.pipes[w, h - 1].values[0] == 1)
            {
                value++;
            }
        }
        if (h != 0) // compare left
        {
            if (puzzle.pipes[w, h].values[3] == 1 && puzzle.pipes[w - 1, h].values[1] == 1)
            {
                value++;
            }
        }

        return value;
    }

    void DebugPipe() //shows pipes in console
    {
        foreach (var item in puzzle.pipes)
        {
            Debug.Log(item.gameObject.name);
        }
    }
}
