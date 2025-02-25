using DG.Tweening;
using System.Collections;
using UnityEngine;

public sealed class DissolveSceneChanger : MonoBehaviour
{
    [SerializeField]
    private Shader _dissolveShader;
    [SerializeField]
    private Texture2D _dissolveTex;
    [SerializeField]
    private float _dissolveDuration;

    private static readonly int _mainTexPropertyID = Shader.PropertyToID("_MainTex");
    private static readonly int _dissolveAmountPropertyID = Shader.PropertyToID("_DissolveAmount");
    private static readonly int _dissolveTexPropertyID = Shader.PropertyToID("_DissolveTex");

    private Texture2D _screenShotTex;
    private Material _material;


    private void Awake()
    {
        var camera = Camera.main;
        _screenShotTex = new Texture2D(camera.pixelWidth, camera.pixelHeight, TextureFormat.RGB24, false);
        _material = new Material(_dissolveShader);
    }

    public void TakeScreenShot()
    {
        _screenShotTex.ReadPixels(new Rect(0, 0, _screenShotTex.width, _screenShotTex.height), 0, 0);
        _screenShotTex.Apply();
    }

    public void SetTexture()
    {
        _material.SetTexture(_mainTexPropertyID, _screenShotTex);
        _material.SetTexture(_dissolveTexPropertyID, _dissolveTex);
    }

    public IEnumerator DissolveAnimation()
    {
        _material.SetFloat(_dissolveAmountPropertyID, 0F);

        yield return DOTween.To(
            () => 0F,
            value => _material.SetFloat(_dissolveAmountPropertyID, value),
            1F,
            _dissolveDuration
            ).SetLink(gameObject);

        _material.SetFloat(_dissolveAmountPropertyID, 1F);
    }
}
