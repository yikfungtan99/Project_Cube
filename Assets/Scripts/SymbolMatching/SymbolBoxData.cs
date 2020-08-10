using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Presets
{
    public int[] p1Preset;
    public int[] p2Preset;
}

public class SymbolBoxData : ScriptableObject
{
    public List<Texture> textures1;
    public List<Texture> textures2;
    public List<Texture> textures3;

    public List<Presets> presetList;
}
