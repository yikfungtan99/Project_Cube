using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Puzzle/PuzzleModuleStorage")]
public class PuzzleMasterStorage : ScriptableSingleton<PuzzleMasterStorage>
{
    public PuzzleTypeStorage[] puzzleTypes;
}
