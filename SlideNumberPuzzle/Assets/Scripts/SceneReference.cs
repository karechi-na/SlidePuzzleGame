using UnityEngine;

[CreateAssetMenu(fileName = "SceneReference", menuName = "Scriptable Objects/SceneReference")]
public class SceneReference : ScriptableObject
{
    [Header("ゲーム本編シーン名")]
    [SerializeField] private string mainSceneName;

    public string SceneName => mainSceneName;

    [Header("クリアシーン名")]
    [SerializeField] private string endSceneName;

    public string EndSceneName => endSceneName;

    [Header("タイトルシーン名")]
    [SerializeField] private string titleSceneName;

    public string TitleSceneName => titleSceneName;
}