using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public sealed class CursorController : MonoBehaviour
{
    [SerializeField] private RectTransform _cursorImage;
    [SerializeField] private Outline _outline;
    [SerializeField] private float _angle;

    private Coroutine _rotateCursor = null;
    
    private void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void OnDestroy()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void Update()
    {
        _cursorImage.anchoredPosition = Input.mousePosition;
    }

    private IEnumerator RotateCoroutine()
    {
        while (true)
        {
            _cursorImage.Rotate(0, 0, -_angle * Time.deltaTime);
            yield return null;
        }
    }

    [ContextMenu("Start")]
    public void StartRotate()
    {
        if (_rotateCursor is not null) return;

        _rotateCursor = StartCoroutine(RotateCoroutine());
    }

    [ContextMenu("Stop")]
    public void StopRotate()
    {
        if (_rotateCursor is null) return;
        
        StopCoroutine(_rotateCursor);
        _rotateCursor = null;
    }

    public void SetColor(Color outlineColor)
    {
        _outline.effectColor = outlineColor;
    }
}
