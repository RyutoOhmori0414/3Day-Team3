using UnityEngine;

public abstract class SingletonMonoBehaviour<T> : SingletonMonoBehaviour where T : SingletonMonoBehaviour
{
    private static T _instance = null;

    public static T I
    {
        get
        {
            if (!_instance) Debug.LogError($"{nameof(T)}のシングルトンインスタンスがありません、Nullを返します。");

            return _instance;
        }
    }

    protected virtual void OnAwake() { }
    protected virtual void OnThisDestroy() { }

    private void Awake()
    {
        if (!_instance && this is T instance)
        {
            _instance = instance;
            OnAwake();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (this == _instance)
        {
            _instance = null;
        }
    }
}
