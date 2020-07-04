using UnityEngine;

public class Indicator : MonoBehaviour
{
    [SerializeField] private GameObject[] indicator;
    
    public void ActivateIndicator()
    {
        foreach (var i in indicator)
        {
            i.SetActive(true);
        }
    }
}
