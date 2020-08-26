using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Photon.Pun;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class PlayerCube : MonoBehaviourPun, IPunInstantiateMagicCallback
{
    public PlayerCube otherCube;
    [SerializeField] private PuzzleModule[] modules;
    private bool _allPuzzleGenerated = false;
    private bool _allPuzzleInitialized = false;
    private PuzzleMasterStorage _puzzleMasterStorage;

    public PuzzleModule currentModule;
    
    private bool _gameStarted = false;

    private int spawnAll = 0;

    //Using a coroutine as we need to wait until other cube is detected
    private IEnumerator Start()
    {
        //wait until other cube is detected
        while (!otherCube)
        {
            yield return null;
        }
        
        _puzzleMasterStorage = PuzzleMasterStorage.Instance;
        //Generate puzzle at Start
        GenerateRandomPuzzle();
    }

    #region  Puzzle Generation
   
    private void GenerateRandomPuzzle()
    {
        if (!photonView.IsMine) return;
        
        if (!PhotonNetwork.IsMasterClient) return;
        
        if (_allPuzzleGenerated) return;

        int seed = Random.Range(0, 999);
        
        photonView.RPC("RpcSpawnPuzzle", RpcTarget.All, seed);

        _allPuzzleGenerated = true;
    }

    [PunRPC]
    private void RpcSpawnPuzzle(int seed)
    {
        Random.InitState(seed);
        
        for (int i = 0; i < 6; i++)
        {
            int newSeed = Random.Range(0, 999);
            PuzzleModuleData moduleDataInstance = GeneratePuzzleModuleData(newSeed);
            modules[i].SpawnPuzzle((int)moduleDataInstance.PuzzleType, moduleDataInstance.PuzzleVariation, moduleDataInstance.PuzzleRole);
            otherCube.modules[i].SpawnPuzzle((int)moduleDataInstance.PuzzleType, moduleDataInstance.PuzzleVariation, -moduleDataInstance.PuzzleRole);
        }
    }

    private PuzzleModuleData GeneratePuzzleModuleData(int seed)
    {
        Random.InitState(seed);
        //PuzzleTypes puzzleGenType = (PuzzleTypes) Random.Range(0, 5);
        PuzzleTypes puzzleGenType = (PuzzleTypes) spawnAll;
        spawnAll++;
        //int puzzleGenVar = 0;
        int puzzleGenVar = Random.Range(0, _puzzleMasterStorage.puzzleTypes[(int) puzzleGenType].puzzleVariation.Length);
        int puzzleGenRole = Random.Range(0, 2);

        if (puzzleGenRole == 0)
        {
            puzzleGenRole = -1;
        }
        else
        {
            puzzleGenRole = 1;
        }
        
        PuzzleModuleData generatedPuzzleModuleData = new PuzzleModuleData(puzzleGenType, puzzleGenVar, puzzleGenRole);
        return generatedPuzzleModuleData;
    }

    #endregion

    public void GetExaminingModule()
    {

        PuzzleModule mod = null;
        
        if (currentModule)
        {
            if(!currentModule.moduleCompleted)
            {
                currentModule.ToggleSelfIndicator();
                OtherIndicator(currentModule.PuzzleId);
            }
            currentModule = null;
            return;
        }

        float curHighest = 0;
        
        foreach (var module in modules)
        {
            if (module.transform.position.z < curHighest)
            {
                curHighest = module.transform.position.z;
                currentModule = module;
                mod = module;
                // module.ToggleSelfIndicator();
                // OtherIndicator(module.PuzzleId);
            }
        }

        if (!mod) return;
        if(!mod.moduleCompleted)
        {
            mod.ToggleSelfIndicator();
            OtherIndicator(mod.PuzzleId);
        }
    }
    
    #region Info Sending
    
    public void Action(object sender, Interactor.OnInteractedEventArgs e)
    {
        if (this.photonView.IsMine)
        {
            this.photonView.RPC("RpcAction", RpcTarget.All, e.ModuleId, e.ComponentId);
        }
    }

    [PunRPC]
    void RpcCheckAllModuleInit()
    {
        if (_allPuzzleInitialized && otherCube._allPuzzleInitialized)
        {
            if (_gameStarted) return;
            GameObject.Find("LoadCanvas").transform.GetChild(0).gameObject.SetActive(false);
            print("Start");
            foreach (var t in modules)
            {
                t.puzzleManager.PuzzleStart();
            }
            _gameStarted = true;
        }
        else
        {
            print("Not started");
            //SceneManager.LoadScene(6);
        }
    }

    public void CheckAllModuleInitialized()
    {
        int count = 0;
        foreach (PuzzleModule module in modules)
        {
            if (module.moduleInitialized) count += 1;
        }
        
        if (count > 5)
        {
            print("All done");
            _allPuzzleInitialized = true;
            photonView.RPC("RpcCheckAllModuleInit", RpcTarget.All);
        }
    }
    
    public void CheckAllModuleCompleted()
    {
        int counter = 0;
        foreach (PuzzleModule module in modules)
        {
            if(module.moduleCompleted)
            {
                counter++;
            }
        }
        if(counter == 5)
        {
            SceneManager.LoadScene(4);
        }
    }

    [PunRPC]
    void RpcAction(int id, int cid)
    {
        otherCube.modules[id].reactors[cid].GetComponent<Reactor>().ReAct();
    }
    
    public void CompletedModule(int id)
    {
        this.photonView.RPC("RpcCompletion", RpcTarget.All, id);
        CheckAllModuleCompleted();
    }

    [PunRPC]
    void RpcCompletion(int id)
    {
        otherCube.modules[id].SetModuleCompleted();
        otherCube.CheckAllModuleCompleted();
    }

    public void OtherIndicator(int id)
    {
        photonView.RPC("RpcOtherIndicator", RpcTarget.All, id);
    }

    [PunRPC]
    void RpcOtherIndicator(int id)
    {
        otherCube.modules[id].ToggleOtherIndicator();
    }

    public void CipherPuzzleRpc(int id, int seed)
    {
        photonView.RPC("RpcCipherPuzzle", RpcTarget.All, id, seed);
    }

    [PunRPC]
    void RpcCipherPuzzle(int id, int seed)
    {
        CipherPuzzleManager cpm = otherCube.modules[id].puzzleManager as CipherPuzzleManager;
        if (cpm)
        {
            cpm.InitializeCipherPuzzle(seed);
        }
        else
        {
            print("cypher puzzle manager not found");
        }
    }

    public void CipherClear(int id)
    {
        photonView.RPC("RpcCipherClear", RpcTarget.All, id);
    }

    [PunRPC]
    private void RpcCipherClear(int id)
    {
        CipherPuzzleManager cpm = otherCube.modules[id].puzzleManager as CipherPuzzleManager;

        if (cpm)
        {
            cpm.ClearSlot();
        }
        else
        {
            print("cypher puzzle manager not found");
        }
    }

    public void CipherPuzzleButton(int id, string s)
    {
       photonView.RPC("RpcCipherPuzzleButton", RpcTarget.All, id , s);
    }
    
    [PunRPC]
    void RpcCipherPuzzleButton(int id, string s)
    {
        CipherPuzzleManager cpm = otherCube.modules[id].puzzleManager as CipherPuzzleManager;
        if (cpm)
        {
            cpm.KeyboardButtonPress(s);
        }
        else
        {
            print("cypher puzzle manager not found");
        }
    }

    public void MazePuzzleButton(int id, int i)
    {
        photonView.RPC("RpMazePuzzleButton", RpcTarget.All, id, i);
    }

    [PunRPC]
    void RpMazePuzzleButton(int id, int i)
    {
        MazePuzzleManager mpm = otherCube.modules[id].puzzleManager as MazePuzzleManager;
        if (mpm)
        {
            mpm.MazeButtonPress(i);
        }
        else
        {
            print("maze puzzle manager not found");
        }
    }

    public void PigpenPuzzle(int id)
    {
        photonView.RPC("RpcPigpenPuzzle", RpcTarget.All, id);
    }

    [PunRPC]
    void RpcPigpenPuzzle(int id)
    {
        PigpenManager pigpenManager = otherCube.modules[id].puzzleManager as PigpenManager;
        if (pigpenManager)
        {
            pigpenManager.CompareIndex();
        }
    }

    #endregion
    
    private void OnDestroy()
    {
        for (int i = 0; i < modules.Length; i++)
        {
            //Need to change to detect multiple interactor
            if (modules[i].GetComponent<Interactor>() != null)
            {
                modules[i].GetComponent<Interactor>().OnInteracted -= Action;
            }
        }
    }

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        SpawnSystem ss = GameObject.FindWithTag("SpawnSystem").GetComponent<SpawnSystem>();
        ss.currentCubes.Add(info.photonView.GetComponent<PlayerCube>());
        ss.AssignCube();
    }
}