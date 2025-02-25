using DG.Tweening;
using TMPro;
using UnityEngine;

public class BackGroundAnimation : MonoBehaviour
{
    [Header("リザルトテキスト")]
    public TextMeshProUGUI tex_EnemyCount;
    public TextMeshProUGUI tex_SurviveTime;

    void Start()
    {
        tex_EnemyCount.text = GameManager.I.DefeatEnemyCount.ToString() + " " + "tai";
        tex_SurviveTime.text = GameManager.I.Score.ToString() + " " + "jikann";

        var tex_EnCo = tex_EnemyCount.GetComponent<RectTransform>();
        tex_EnCo.anchoredPosition = new Vector2(0, -600);
        tex_EnCo.transform.DOMoveY(650f, 5f);

        var tex_SuTi = tex_SurviveTime.GetComponent<RectTransform>();
        tex_SuTi.anchoredPosition = new Vector2(0, -715);
        tex_SuTi.transform.DOMoveY(550f, 5f);
    }
}
