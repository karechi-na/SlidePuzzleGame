//==========================================================================================================
// 製作者 : スズキ チヒロ
//==========================================================================================================

using UnityEngine;

/// <summary>
/// Singleton化されたMonoBehaviourクラスの基底クラス。
/// </summary>
public abstract class SingletonMonobehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindFirstObjectByType<T>();
                if (instance == null)
                {
                    Debug.LogError($"No instance of {typeof(T).Name} found in the scene. Please ensure that there is an active GameObject with a {typeof(T).Name} component.");
                }
            }
            return instance;
        }
    }

    protected virtual void Awake()
    {
        CheckInstance();
    }

    protected bool CheckInstance()
    {
        if (instance == null)
        {
            instance = this as T;
            return true;
        }
        else if (Instance == this)
        {
            return true;
        }
        Destroy(this);
        return false;
    }
}
