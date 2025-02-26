using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StartCountUIController : MonoBehaviour, IGameReadyReciever, IGameStartReciever
{
    [SerializeField] private Text _count;
    
    private bool _isReady = false;

    private void Update()
    {
        if(!_isReady) return;

        _count.text = GameManager.I.CurrentReadySeconds.ToString("0");
    }

    public void GameReady()
    {
        _isReady = true;
        _count.gameObject.SetActive(true);
    }

    private IEnumerator GameStartCoroutine()
    {
        _count.text = "GO!!!!";
        yield return new WaitForSeconds(0.5F);
        _count.gameObject.SetActive(false);
    }
    
    public void GameStart()
    {
        _isReady = false;
        StartCoroutine(GameStartCoroutine());
    }
}
