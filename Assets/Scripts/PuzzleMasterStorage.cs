using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Puzzle/PuzzleModuleStorage")]
public class PuzzleMasterStorage : ScriptableObject
{
    private static PuzzleMasterStorage _instance = null;

    public static PuzzleMasterStorage Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.Load("PuzzleMasterStorage") as PuzzleMasterStorage;
            }
            return _instance;
        }
    }
    
    public PuzzleTypeStorage[] puzzleTypes;
}
