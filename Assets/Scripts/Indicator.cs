using UnityEngine;

public enum IndicatorTypes
{
    SelfIndicator,
    OtherIndicator,
    CompleteIndicator
}

public class Indicator : MonoBehaviour
{
    [SerializeField] private GameObject[] indicator;
    public IndicatorTypes indicatorType;

    public bool isActive = false;
    
    public void ActiveIndicator()
    {
        isActive = true;
        foreach (var i in indicator)
        {
            i.SetActive(true);
        }
    }

    public void DeActivateIndicator()
    {
        isActive = false;
        foreach (var i in indicator)
        {
            i.SetActive(false);
        }
    }
}
