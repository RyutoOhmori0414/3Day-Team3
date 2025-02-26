using UnityEngine;

public class GameStart : MonoBehaviour, ISceneLoadEndReciever
{
    public void SceneLoadEnd()
    {
        GameManager.I.PlayReady();
         
    }
}