using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Rentire.Core;
using UnityEngine;
using Random = UnityEngine.Random;

public class AudioManager : Singleton<AudioManager>
{
    public List<SFXElement> sfxList = new List<SFXElement>();
    public float footStepTimerVal;
    public bool isFootStep;

    private AudioSource _audioSource;
    private SFXElement _footStep;
    private SFXElement _coinCollect;


    private AudioSource _footStepSource;
    private AudioSource _coinCollectSource;

    
    private float _footStepTimer;
    
    private int coinCollectCount;
    private float coinTimer;



    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _footStep = sfxList.FirstOrDefault(x => x.sfx == SFX.footStep);
        _footStepSource = NewAudioSource("Foot Step");
        _coinCollectSource = NewAudioSource("Coin Collect");
    }

    public void PlaySFX(SFX _sfx)
    {
        var element = sfxList.FirstOrDefault(x => x.sfx == _sfx);
        if (element == null)
            return;

        var randomSource = element.audio[Random.Range(0, element.audio.Length)];
        _audioSource.PlayOneShot(randomSource, element.volume);
    }

    public void PlayFootStep()
    {
        var randomSource = _footStep.audio[Random.Range(0, _footStep.audio.Length)];
        var randomPitch = Random.Range(1 / 0.5f, 0.8f);
        _footStepSource.pitch = randomPitch;
        _footStepSource.PlayOneShot(randomSource, _footStep.volume);
    }

    public void Play_CoinCollect()
    {
        if (_coinCollect == null)
            _coinCollect = sfxList.FirstOrDefault(x => x.sfx == SFX.coinCollect);

        var clip = _coinCollect.audio[0];

        
        var newPitch = Fmap(coinCollectCount, 0, 20, 0.75f, 2f);
        _coinCollectSource.pitch = newPitch;
        coinTimer = 3f;
        coinCollectCount++;
        _coinCollectSource.PlayOneShot(clip,_coinCollect.volume);
    }
    


    
    private void Update()
    {
        if (currentPlayerState == PlayerState.Running && isFootStep)
        {
            if (Time.time > _footStepTimer)
            {
                _footStepTimer = Time.time + 1 / (footStepTimerVal + Random.Range(-0.2f, 0.2f));
                PlayFootStep();
            }

            coinTimer -= Time.deltaTime;
            if (coinTimer <= 0)
            {
                coinCollectCount = 0;
            }
            
        }
    }

    AudioSource NewAudioSource( string name)
    {
        var footStepSource = new GameObject(name);
        footStepSource.transform.parent = transform;
        var source = footStepSource.AddComponent<AudioSource>();
        return source;
    }
}

[System.Serializable]
public class SFXElement
{
    public SFX sfx;
    public AudioClip[] audio;
    public float volume = 1;
}

public enum SFX
{
    success,
    fail,
    footStep,
    coinCollect,

}