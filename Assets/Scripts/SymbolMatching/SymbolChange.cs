using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SymbolChange : MonoBehaviour
{
    [SerializeField] GameObject quadBox;
    [SerializeField] SymbolBoxData symbolData;
    //! Symbole Index is Position Index of this symbol
    public int symbolIndex;
    Material m;
    
    // Start is called before the first frame update
    void Start()
    {
        m = quadBox.gameObject.GetComponent<MeshRenderer>().material;
        UpdateTexture(symbolIndex);
    }

    
    public void UpdateTexture(int textureIndex)
    {
        m.mainTexture = SymbolManager.symbolInstance.textureList[SymbolManager.symbolInstance.textureIndexList[symbolIndex]];
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    //Change material(symbol) when press
    private void OnMouseDown()
    {
        //! Call other's UpdateOtherAnswer
        //SymbolManager.symbolInstance.UpdateOtherAnswer(symbolIndex);
        int value = SymbolManager.symbolInstance.UpdatePlayerAnswer(symbolIndex);
        UpdateTexture(value);
        SymbolManager.symbolInstance.CheckAnswer();
    }
}
