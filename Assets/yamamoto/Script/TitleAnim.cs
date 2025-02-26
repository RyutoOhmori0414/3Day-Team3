using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TitleAnim : MonoBehaviour
{
    public Title title; //ディゾルブ処理のスクリプト

    [Header("アニメーション待機時間")]
    public float waitTime;

    [Header("テキストアニメーション時間")]
    public float animTime;

    [Header("表示するテキスト")]
    public Text startText;

    [Header("UIオブジェクト")]
    public GameObject uiObj;

    private void Start()
    {
        startText.enabled = false; //非表示
    }

    public void GameStart()
    {
        uiObj.SetActive(false); //UIのオブジェクトを非表示
        startText.enabled = true; //表示

        var stText = startText.GetComponent<RectTransform>();
        stText.anchoredPosition = new Vector2(0, 0);
        stText.transform.DOScale(new Vector2(2, 2), animTime);
            
        StartCoroutine(UIAnimWait()); //待機
    }

    IEnumerator UIAnimWait()
    {
        yield return new WaitForSeconds(waitTime);

        title.GoNextScene();
    }
}
