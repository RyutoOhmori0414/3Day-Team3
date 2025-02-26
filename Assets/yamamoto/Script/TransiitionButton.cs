using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

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
