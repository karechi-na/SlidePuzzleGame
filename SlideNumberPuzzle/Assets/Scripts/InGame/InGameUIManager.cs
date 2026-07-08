using UnityEngine;

public class InGameUIManager : SingletonMonobehaviour<InGameUIManager>
{
    [SerializeField] private TimeText timeText;

    public void UpdateTime()
    {
        timeText.UpdateTime();
    }
}
