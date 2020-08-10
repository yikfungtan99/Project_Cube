using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SymbolManager : MonoBehaviour
{
    public static SymbolManager symbolInstance = null;
    [SerializeField] GameObject symbolPrefab1;
    [SerializeField] GameObject symbolPrefab2;
    //Already randomized buttons then put in
    [SerializeField] List<ButtonGroup> buttonGroup = new List<ButtonGroup>();
    [SerializeField] SymbolBoxData symbolData;
    //[SerializeField] SymbolPreset symbolPreset;

    //Change texture 
    public List<Texture> textureList;

    // Answer List & preset symboll list
    [HideInInspector] public int[] textureIndexList;
    [HideInInspector] public int[] reactTextureIndexList;//! player 2's textureindexList

    // Start is called before the first frame update
    void Awake()
    {
        CheckInstance();
        SpawnSymbolPuzzle();
    }

    private void Start()
    {
        textureList = symbolData.textures1;

        //TODO Set preset from symbolData
        textureIndexList = symbolData.presetList[0].p1Preset;
        reactTextureIndexList = symbolData.presetList[0].p2Preset;
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Both are player1, when sending index, updateotheranswer().
    /// </summary>
    /// <param name="index"></param>
    /// <param name="answer"></param>
    public int UpdatePlayerAnswer(int index)
    {
        textureIndexList[index]++;
        if(textureIndexList[index] >= textureList.Count)
        {
            textureIndexList[index] = 0;
        }
        return textureIndexList[index];
    }

    public void UpdateOtherAnswer(int index)
    {
        reactTextureIndexList[index]++;
        if (reactTextureIndexList[index] >= textureList.Count)
        {
            reactTextureIndexList[index] = 0;
        }
    }

    void CheckInstance()
    {
        if (symbolInstance == null)
        {
            symbolInstance = this;
        }
        else if (symbolInstance = this)
        {
            Destroy(gameObject);
        }
    }

    void SpawnSymbolPuzzle()
    {
        Instantiate(symbolPrefab1);
        Instantiate(symbolPrefab2);

        //Store in tempList and then remove element to avoid grabbing same object from scriptableobject list.
        List<GameObject> cylinderP1TempList = new List<GameObject>();
        List<GameObject> cylinderP2TempList = new List<GameObject>();

        #region Adding button to list
        for (int i=0; i<symbolPrefab1.transform.childCount; i++)
        { 
            Transform newBtn = symbolPrefab1.transform.GetChild(i);

            if (newBtn.name.Equals("P1Cylinder"))
            {
                cylinderP1TempList.Add(newBtn.gameObject);
                continue;
            } 

            if(cylinderP1TempList.Count >= 10)
            {
                return;
            }
        }

        for (int k = 0; k < symbolPrefab2.transform.childCount; k++)
        {

            Transform newBtn = symbolPrefab2.transform.GetChild(k);

            if (newBtn.name.Equals("P2Cylinder"))
            {
                cylinderP2TempList.Add(newBtn.gameObject);
                continue;
            }

            if (cylinderP1TempList.Count >= 10)
            {
                return;
            }
        }
        #endregion 

        //Randomizing button to be on or off.
        for (int j = 0; j < buttonGroup.Count; j++)
        {
            int cylinderRando = Random.Range(0, cylinderP1TempList.Count);
            buttonGroup[j].buttonP1Collider = cylinderP1TempList[cylinderRando].GetComponent<CapsuleCollider>();
            cylinderP1TempList.RemoveAt(cylinderRando);
            buttonGroup[j].button1ColliderEnabled = true;

            //cylinderRando = Random.Range(0, cylinderP2TempList.Count);
            buttonGroup[j].buttonP2Collider = cylinderP2TempList[cylinderRando].GetComponent<CapsuleCollider>();
            cylinderP2TempList.RemoveAt(cylinderRando);

            buttonGroup[j].InitialiseButtonDefaultEnabled();
            int tempNum = Random.Range(0, 2);
            if (tempNum == 0)
            {
                buttonGroup[j].ToggleGroup();
            }
        }
    }

    void RandomizeSymbol()
    {
        int tempRandom = Random.Range(0, 3);
        {
            List<Texture> tempTextureList;
            if(tempRandom == 0)
            {
                tempTextureList = new List<Texture>(symbolData.textures1);
            }
            if(tempRandom == 1)
            {
                tempTextureList = new List<Texture>(symbolData.textures2);
            }
            if(tempRandom == 2)
            {
                tempTextureList = new List<Texture>(symbolData.textures3);
            }
        }
    }

    public bool CheckAnswer()
    {
        //Check answer between player 1 textureIndexList & player 2 textureIndexList
        for (int i = 0; i < textureIndexList.Length; i++)
        {
            if(textureIndexList[i] != reactTextureIndexList[i])
            {
                return false;
            }
        }

        //! do finish game here
        return true;
    }

    private void OnMouseDown()
    {
        //! Find which button group is pressed
        int index = 1;
        buttonGroup[index].ToggleGroup();
        Debug.Log("Toggle");
    }
}

[System.Serializable]
public class ButtonGroup
{
    public Collider buttonP1Collider;
    public bool button1ColliderEnabled = true;

    public Collider buttonP2Collider;
    public bool button2ColliderEnabled = false;

    public Texture textureP1;
    public Texture textureP2;

    //! Must be called in Awake or Start unless you manually set the default states
    public void InitialiseButtonDefaultEnabled()
    {
        buttonP1Collider.enabled = button1ColliderEnabled;
        buttonP2Collider.enabled = button2ColliderEnabled;
    }

    public void ToggleGroup()
    {
        buttonP1Collider.enabled = !buttonP1Collider.enabled;
        buttonP2Collider.enabled = !buttonP2Collider.enabled;

        button1ColliderEnabled = !button1ColliderEnabled;
        button2ColliderEnabled = !button2ColliderEnabled;
    }

    public void AddQuadPictureToButton()
    {
        buttonP1Collider.GetComponent<MeshRenderer>().material.mainTexture = textureP1;
        buttonP1Collider.GetComponent<MeshRenderer>().material.mainTexture = textureP2;
    }
}
