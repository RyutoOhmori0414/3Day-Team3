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

    private int minutes;
    private int seconds;

    void Start()
    {
        tex_EnemyCount.text = GameManager.I.DefeatEnemyCount.ToString();
        
        //分秒
        minutes = (int)GameManager.I.CurrentGameElapsedSeconds / 60;
        seconds = (int)GameManager.I.CurrentGameElapsedSeconds % 60;
        tex_SurviveTime.text = /*minutes.ToString("00") + ":" +*/ seconds.ToString("00");

        tex_Score.text = GameManager.I.Score.ToString();

        var tex_EnCo = tex_EnemyCount.GetComponent<RectTransform>();

        {
            var target = tex_EnemyCount.transform;
            target.position += new Vector3(1500F, 0F, 0F);
            target.transform.DOLocalMoveX(-1500F + target.localPosition.x, time)
                .SetEase(Ease.InOutQuad);
        }
        
        {
            var target = tex_SurviveTime.transform;
            target.position += new Vector3(1500F, 0F, 0F);
            target.transform.DOLocalMoveX(-1500F + target.localPosition.x, time)
                .SetEase(Ease.InOutQuad);
        }

        //{
        //    var target = tex_Count.transform;
        //    target.position += new Vector3(1500F, 0F, 0F);
        //    target.transform.DOLocalMoveX(-1500F + target.localPosition.x, time)
        //        .SetEase(Ease.InOutQuad);
        //}

        {
            var target = tex_Time.transform;
            target.position += new Vector3(1500F, 0F, 0F);
            target.transform.DOLocalMoveX(-1500F + target.localPosition.x, time)
                .SetEase(Ease.InOutQuad);
        }
    }
}
