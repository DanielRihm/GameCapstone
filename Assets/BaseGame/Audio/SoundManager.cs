using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Assertions;
using UnityEngine.Audio;
using LCPS.SlipForge;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public AudioMixer AudioMixer;
    public List<Sound> MusicSounds, SFXSounds;
    public AudioSource MusicSource, SFXSource, RightWeaponSource, RightWeaponChargeSource, RightWeaponLoopSource, LeftWeaponSource,
        LeftWeaponChargeSource, LeftWeaponLoopSource, MagicSource, MagicChargeSource, MagicLoopSource;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(gameObject);
        }

        Assert.IsTrue(MusicSource != null && SFXSource != null && RightWeaponSource != null && RightWeaponChargeSource != null && LeftWeaponSource != null &&
            LeftWeaponChargeSource != null && MagicSource != null && MagicChargeSource != null && RightWeaponLoopSource != null && LeftWeaponLoopSource != null &&
            MagicLoopSource != null && AudioMixer != null);
    }

    private void Start()
    {
        PlayMusic("title_music");
    }

    public void Update()
    {
        if (DataTracker.Instance.IsMenuOpen())
        {
            PauseSounds();
        }
        else
        {
            PlaySouns();
        }
    }

    private void PauseSounds()
    {
        SFXSource.Pause();
        RightWeaponSource.Pause();
        RightWeaponChargeSource.Pause();
        RightWeaponLoopSource.Pause();
        LeftWeaponSource.Pause();
        LeftWeaponChargeSource.Pause();
        LeftWeaponLoopSource.Pause();
        MagicSource.Pause();
        MagicChargeSource.Pause();
        MagicLoopSource.Pause();
    }

    private void PlaySouns()
    {
        SFXSource.UnPause();
        RightWeaponSource.UnPause();
        RightWeaponChargeSource.UnPause();
        RightWeaponLoopSource.UnPause();
        LeftWeaponSource.UnPause();
        LeftWeaponChargeSource.UnPause();
        LeftWeaponLoopSource.UnPause();
        MagicSource.UnPause();
        MagicChargeSource.UnPause();
        MagicLoopSource.UnPause();
    }

    public int RegisterSFX(string name, AudioClip clip)
    {
        Sound _newSound = new Sound();
        _newSound.SoundName = name;
        _newSound.Clip = clip;

        foreach (Sound _sfx in SFXSounds)
        {
            if (_sfx.SoundName == _newSound.SoundName)
            {
                return -1;
            }
        }

        SFXSounds.Add(_newSound);

        return 1;
    }

    public void PlayMusic(string name)
    {
        Sound song = MusicSounds.Find(s => s.SoundName == name);

        if(song != null)
        {
            MusicSource.clip = song.Clip;
            MusicSource.Play();
        }
    }

    public void StopMusic() { MusicSource.Stop(); }

    public void PlayLeftWeapon(string name) 
    {
        Sound noise = SFXSounds.Find(s => s.SoundName == name);

        if (noise != null)
        {
            LeftWeaponSource.clip = noise.Clip;
            LeftWeaponSource.Play();
        }
    }

    public void StopLeftWeapon() { LeftWeaponSource.Stop(); }

    public void PlayRightWeapon(string name)
    {
        Sound noise = SFXSounds.Find(s => s.SoundName == name);

        if (noise != null)
        {
            RightWeaponSource.clip = noise.Clip;
            RightWeaponSource.Play();
        }
    }

    public void StopRightWeapon() {  RightWeaponSource.Stop(); }

    public void PlayLeftWeaponCharging(string name)
    {
        Sound noise = SFXSounds.Find(s => s.SoundName == name);

        if (noise != null)
        {
            LeftWeaponChargeSource.clip = noise.Clip;
            LeftWeaponChargeSource.Play();
        }
    }

    public void StopLeftWeaponCharging() {  LeftWeaponChargeSource.Stop(); }

    public void PlayRightWeaponCharging(string name)
    {
        Sound noise = SFXSounds.Find(s => s.SoundName == name);

        if (noise != null)
        {
            RightWeaponChargeSource.clip = noise.Clip;
            RightWeaponChargeSource.Play();
        }
    }

    public void StopRightWeaponCharging() {  RightWeaponChargeSource.Stop(); }

    public void PlayMagic(string name)
    {
        Sound noise = SFXSounds.Find(s => s.SoundName == name);

        if (noise != null)
        {
            MagicSource.clip = noise.Clip;
            MagicSource.Play();
        }
    }

    public void StopMagic() { MagicSource.Stop(); }

    public void PlayMagicCharging(string name)
    {
        Sound noise = SFXSounds.Find(s => s.SoundName == name);

        if (noise != null)
        {
            MagicChargeSource.clip = noise.Clip;
            MagicChargeSource.Play();
        }
    }

    public void StopMagicCharging() { MagicChargeSource.Stop(); }

    public void PlayLeftWeaponLoop(string name)
    {
        Sound noise = SFXSounds.Find(s => s.SoundName == name);

        if (noise != null)
        {
            LeftWeaponLoopSource.clip = noise.Clip;
            LeftWeaponLoopSource.Play();
        }
    }

    public void StopLeftWeaponLoop() { LeftWeaponLoopSource.Stop(); }

    public void PlayRightWeaponLoop(string name)
    {
        Sound noise = SFXSounds.Find(s => s.SoundName == name);

        if (noise != null)
        {
            RightWeaponLoopSource.clip = noise.Clip;
            RightWeaponLoopSource.Play();
        }
    }

    public void StopRightWeaponLoop() { RightWeaponLoopSource.Stop(); }

    public void PlayMagicLoop(string name)
    {
        Sound noise = SFXSounds.Find(s => s.SoundName == name);

        if (noise != null)
        {
            MagicLoopSource.clip = noise.Clip;
            MagicLoopSource.Play();
        }
    }

    public void StopMagicLoop() 
    { 
        MagicLoopSource.Stop(); 
    }

    public void PlaySFX(string name)
    {
        Sound noise = SFXSounds.Find(s => s.SoundName == name);

        if (noise != null)
        {
            SFXSource.PlayOneShot(noise.Clip);
        }
    }

    public void StopSfx()
    {
        if(SFXSource != null)
            SFXSource.Stop();
    }

    public void ToggleMusic()
    {
        MusicSource.mute = !MusicSource.mute;
    }

    public void ToggleSFX()
    {
        SFXSource.mute = !SFXSource.mute;
    }

    public void SetMasterVolume(float volume)
    {
        AudioMixer.SetFloat("MasterVolume", volume * 80 - 80);
    }

    public void SetMusicVolume(float volume)
    {
        AudioMixer.SetFloat("MusicVolume", volume * 80 - 80);
    }

    public void SetSFXVolume(float volume)
    {
        AudioMixer.SetFloat("SFXVolume", volume * 80 - 80);
    }
}
