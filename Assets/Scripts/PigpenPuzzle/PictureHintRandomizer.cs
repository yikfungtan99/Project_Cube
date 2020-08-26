using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PictureHintRandomizer : MonoBehaviour
{
    void Start()
    {
        int i = UnityEngine.Random.Range(0, gameObject.transform.childCount);
        transform.GetChild(i).gameObject.SetActive(true);
    }
}
