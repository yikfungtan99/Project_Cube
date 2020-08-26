using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeButtonRandomizer : MonoBehaviour
{
    public MazeButton[] buttons; 
    

    // Start is called before the first frame update
    void Start()
    {
        buttons = GetComponentsInChildren<MazeButton>();
        List<int> listNumbers = new List<int>();
        int number;
        for (int i = 0; i < buttons.Length; i++)
        {
            do
            {
                number = UnityEngine.Random.Range(0, 4);
            } while (listNumbers.Contains(number));
            listNumbers.Add(number);
            buttons[i].SetDirection(number); // 0 = up, 1 = right, 2 = down, 3 = left
        }
    }
}
