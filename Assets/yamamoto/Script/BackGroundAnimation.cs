using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class BackGroundAnimation : MonoBehaviour
{
    [Header("リザルトテキスト")]
    public Text tex_EnemyCount;
    public Text tex_SurviveTime;
    public Text tex_Count;
    public Text tex_Time;
    public Text tex_Score;
    
    [Header("テキストのアニメーション秒数")]
    public float time;

    private int sTime; //生存時間計算

    void Start()
    {
        tex_EnemyCount.text = GameManager.I.DefeatEnemyCount.ToString();

        sTime = 60 - (int)GameManager.I.CurrentGameElapsedSeconds;
        tex_SurviveTime.text = sTime.ToString();

        tex_Score.text = GameManager.I.Score.ToString();

        var tex_EnCo = tex_EnemyCount.GetComponent<RectTransform>();
        tex_EnCo.anchoredPosition = new Vector2(0, -700);
        tex_EnCo.transform.DOMoveY(650f, time)
            .SetEase(Ease.InOutQuad);
        
        var tex_SuTi = tex_SurviveTime.GetComponent<RectTransform>();
        tex_SuTi.anchoredPosition = new Vector2(0, -815);
        tex_SuTi.transform.DOMoveY(500f, time)
            .SetEase(Ease.InOutQuad);

        var tex_Co = tex_Count.GetComponent<RectTransform>();
        tex_Co.anchoredPosition = new Vector2(500, -700);
        tex_Co.transform.DOMoveY(650f, time)
            .SetEase(Ease.InOutQuad);

        var tex_Ti = tex_Time.GetComponent<RectTransform>();
        tex_Ti.anchoredPosition = new Vector2(500, -815);
        tex_Ti.transform.DOMoveY(500f, time)
            .SetEase(Ease.InOutQuad);
    }
}
