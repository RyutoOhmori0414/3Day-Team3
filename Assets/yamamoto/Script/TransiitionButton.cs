using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class TransiitionButton : MonoBehaviour
{
    public void Title()
    {
        SceneManager.LoadScene("Title");
    }

    public void GameStart()
    {
        SceneManager.LoadScene("GameScene");
    }
}
