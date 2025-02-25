using UnityEngine;

[CreateAssetMenu(fileName = "SingletonMonoBehaviourCreateData", menuName = "SingletonData")]
public sealed class SingletonMonoBehaviourCreateData : ScriptableObject
{
    private const string CREATE_DATA_PATH = "SingletonMonoBehaviourCreateData";

    [RuntimeInitializeOnLoadMethod]
    private static void CreateSingletonPrefabs()
    {
        var data = Resources.Load<SingletonMonoBehaviourCreateData>(CREATE_DATA_PATH);

        if (!data) return;

        foreach (var prefab in data._singletonPrefabs)
        {
            var instance = Instantiate(prefab.gameObject);
            DontDestroyOnLoad(instance);
        }
    }

    [SerializeField]
    private SingletonMonoBehaviour[] _singletonPrefabs;
}
