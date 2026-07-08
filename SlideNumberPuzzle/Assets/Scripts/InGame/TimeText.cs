using UnityEngine;
using TMPro;

public class TimeText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timeText;

    /// <summary>
    /// 時間更新メソッド
    /// </summary>
    public void UpdateTime()
    {
        timeText.text = DataHolder.Instance.time.ToString("F1") + "[s]";
    }
}
