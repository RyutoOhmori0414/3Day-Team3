using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public sealed class PanelGameOverUIController : MonoBehaviour, IGameEndReciever
{
    [SerializeField] private GameObject _finishPanel;

    [SerializeField]
    private float _nextSceneSeconds = 1;

    [SerializeField]
    private string _resultSceneName = "Result";

    private void Awake()
    {
        _finishPanel.SetActive(false);
    }

    public void GameEnd()
    {
        Debug.Log("End");
        _finishPanel.SetActive(true);
        StartCoroutine(GameEndCoroutine());
    }

    private IEnumerator GameEndCoroutine()
    {
        _finishPanel.transform.localScale = Vector3.zero;
        yield return _finishPanel.transform.DOScale(new Vector3(50, 50, 50), _nextSceneSeconds).SetEase(Ease.InQuad).WaitForCompletion();
        
        SingletonSceneManager.I.LoadScene(_resultSceneName);
    }
}
