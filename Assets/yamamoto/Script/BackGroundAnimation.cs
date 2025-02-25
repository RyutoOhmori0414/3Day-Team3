using DG.Tweening;
using TMPro;
using UnityEngine;

public class BackGroundAnimation : MonoBehaviour
{
    [Header("リザルトテキスト")]
    public TextMeshProUGUI tex_EnemyCount;
    public TextMeshProUGUI tex_SurviveTime;
    public TextMeshProUGUI tex_Count;
    public TextMeshProUGUI tex_Time;

    [Header("テキストのアニメーション秒数")]
    public float time;

    private int minutes;
    private int seconds;

    void Start()
    {
        tex_EnemyCount.text = GameManager.I.DefeatEnemyCount.ToString();
        
        //分秒
        minutes = (int)GameManager.I.CurrentReadySeconds / 60;
        seconds = (int)GameManager.I.CurrentReadySeconds % 60;
        tex_SurviveTime.text = minutes + " " +  ":" +  " " + seconds;

        var tex_EnCo = tex_EnemyCount.GetComponent<RectTransform>();
        tex_EnCo.anchoredPosition = new Vector2(0, -600);
        tex_EnCo.transform.DOMoveY(650f, time)
            .SetEase(Ease.InOutQuad);
        
        var tex_SuTi = tex_SurviveTime.GetComponent<RectTransform>();
        tex_SuTi.anchoredPosition = new Vector2(0, -715);
        tex_SuTi.transform.DOMoveY(500f, time)
            .SetEase(Ease.InOutQuad);

        var tex_Co = tex_Count.GetComponent<RectTransform>();
        tex_Co.anchoredPosition = new Vector2(500, -600);
        tex_Co.transform.DOMoveY(650f, time)
            .SetEase(Ease.InOutQuad);

        var tex_Ti = tex_Time.GetComponent<RectTransform>();
        tex_Ti.anchoredPosition = new Vector2(500, -715);
        tex_Ti.transform.DOMoveY(500f, time)
            .SetEase(Ease.InOutQuad);
    }
}
