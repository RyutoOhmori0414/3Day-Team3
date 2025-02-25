using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.UI;

public sealed class DissolveSceneChanger : MonoBehaviour
{
    [SerializeField]
    private Shader _dissolveShader;
    [SerializeField]
    private Texture2D _dissolveTex;
    [SerializeField, ColorUsage(false, true)]
    private Color _dissolveColor;
    [SerializeField]
    private float _dissolveDuration;
    [SerializeField]
    private RawImage _dissolvePanel;

    private static readonly int _dissolveAmountPropertyID = Shader.PropertyToID("_DissolveAmount");
    private static readonly int _dissolveColorPropertyID = Shader.PropertyToID("_DissolveColor");
    private static readonly int _dissolveTexPropertyID = Shader.PropertyToID("_DissolveTex");

    private Texture2D _screenShotTex;
    private Material _material;


    private void Awake()
    {
        var camera = Camera.main;
        _screenShotTex = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        _material = new Material(_dissolveShader);
        
        _dissolvePanel.gameObject.SetActive(false);
    }

    private IEnumerator TakeScreenShot()
    {
        var current = RenderTexture.active;
        var renderTex = new RenderTexture(Screen.width, Screen.height, 0);
        
        RenderTexture.active = renderTex;
        Camera.main.targetTexture = renderTex;

        yield return new WaitForEndOfFrame();
        
        _screenShotTex.ReadPixels(new Rect(0, 0, _screenShotTex.width, _screenShotTex.height), 0, 0);
        _screenShotTex.Apply();
        
        RenderTexture.active = current;
        Camera.main.targetTexture = null;
    }

    private void SetTexture()
    {
        _dissolvePanel.texture = _screenShotTex;
        _dissolvePanel.material = _material;
        _material.SetColor(_dissolveColorPropertyID, _dissolveColor);
        _material.SetTexture(_dissolveTexPropertyID, _dissolveTex);
    }

    public IEnumerator BeforeDissolve()
    {
        yield return TakeScreenShot();
        SetTexture();
    }

    public IEnumerator DissolveAnimation()
    {
        Debug.Log("te");
        _dissolvePanel.gameObject.SetActive(true);
        
        _material.SetFloat(_dissolveAmountPropertyID, 0F);

        yield return DOTween.To(
            () => 0F,
            value => _material.SetFloat(_dissolveAmountPropertyID, value),
            1F,
            _dissolveDuration
            ).SetLink(gameObject).WaitForCompletion();

        _material.SetFloat(_dissolveAmountPropertyID, 1F);
        
        _dissolvePanel.gameObject.SetActive(false);
    }
}
