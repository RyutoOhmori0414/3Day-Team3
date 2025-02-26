using System;
using UnityEngine;

public sealed class BGMManager : MonoBehaviour
{
    [SerializeField] private AudioClip _title, _ingame, _result;
    [SerializeField, Range(0F, 1F)] private float _titleVolume = 1, _ingameVolume = 1, _resultVolume = 1; 
    [SerializeField] private AudioSource _source;

    private void Awake()
    {
        Play(_title, _titleVolume);
    }

    private void Play(AudioClip clip, float volume)
    {
        _source.Stop();
        _source.volume = volume;
        _source.clip = clip;
        _source.Play();
    }
    
    public void Play(string sceneName)
    {
        switch (sceneName)
        {
            case "Title":
                Play(_title, _titleVolume);
                break;
            case "GameScene":
                Play(_ingame, _ingameVolume);
                break;
            default:
                Play(_result, _resultVolume);
                break;
        }
    }
}
