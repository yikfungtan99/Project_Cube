using UnityEngine;

public enum PuzzleTypes
{
    Maze,
    Pigpen,
    Cipher,
}

//This class solve the problem of Monobehaviour not having a constructor;
public class PuzzleModuleData
{
    public PuzzleTypes PuzzleType;
    public int PuzzleVariation;
    public int PuzzleRole;

    public PuzzleModuleData(PuzzleTypes type, int variation, int role)
    {
        PuzzleType = type;
        PuzzleVariation = variation;
        PuzzleRole = role;
    }
}
