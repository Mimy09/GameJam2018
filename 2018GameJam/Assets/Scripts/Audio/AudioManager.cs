using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private class FadeInformation
    {
        // :: variables
        public float speed = 0.1f;
        public AudioSource source = null;
        public static float maximum = 1.0f;
        public static float minimum = 0.0f;
        // :: functions
        public void Update()
        {
            // update fade values
            float amount = source.volume + (speed * Time.deltaTime);
            source.volume = Mathf.Clamp(amount, minimum, maximum);
        }
        public bool IsFadeComplete()
        {
            // check if fade finished
            return source.volume == maximum || source.volume == minimum;
        }
    }
    // :: variables
    [Range(0, 1)] public float volume = 0.5f;
    public List<AudioClip> musicClips = new List<AudioClip>();
    public List<AudioClip> soundClips = new List<AudioClip>();
    private Dictionary<AudioClip, AudioSource> sourceTable = new Dictionary<AudioClip, AudioSource>();
    private Dictionary<AudioClip, FadeInformation> fadeTable = new Dictionary<AudioClip, FadeInformation>();
    // :: functions
    void Awake()
    {
        foreach (AudioClip clip in musicClips)
        {
            AddClip(clip, 1);
        }
        PlayMusic(0, true);
    }
    void Update()
    {
        FadeInformation.maximum = volume;
        foreach (AudioClip key in fadeTable.Keys)
        {
            fadeTable[key].Update();
            if (fadeTable[key].IsFadeComplete())
            {
                Debug.Log("removing fade");
                fadeTable.Remove(key);
            }
        }
    }
    void AddClip(AudioClip clip, float volume)
    {
        if (sourceTable.ContainsKey(clip)) return;
        GameObject obj = Instantiate(new GameObject("manager-audio"), transform);
        AudioSource source = obj.AddComponent<AudioSource>();
        source.playOnAwake = false;
        source.volume = volume;
        source.clip = clip;
        source.Stop();
        sourceTable.Add(clip, source);
    }
    void StopClip(AudioClip clip)
    {
        if (sourceTable.ContainsKey(clip))
        {
            sourceTable[clip].Stop();
        }
    }
    void PlayClip(AudioClip clip, bool repeat)
    {
        if (sourceTable.ContainsKey(clip))
        {
            if (repeat)
            {
                sourceTable[clip].Play();
                sourceTable[clip].loop = true;
            }
            else
            {
                sourceTable[clip].PlayOneShot(clip);
            }
        }
    }
    void FadeClip(AudioSource source, AudioClip clip, float speed)
    {
        Debug.Log(speed < 0 ? "[Fade] adding fadeOut" : "[Fade] adding fadeIn");
        if (!musicClips.Contains(clip) && !soundClips.Contains(clip)) return;
        FadeInformation info = new FadeInformation();
        info.source = source;
        info.speed = 1 / speed;
        if (fadeTable.ContainsKey(clip))
        {
            fadeTable[clip] = info;
        }
        else
        {
            fadeTable.Add(clip, info);
        }
        info.Update();
    }
    public void AddMusic(AudioClip clip) { if (!musicClips.Contains(clip)) { AddClip(clip, 0); musicClips.Add(clip); } }
    public void AddSound(AudioClip clip) { if (!soundClips.Contains(clip)) { AddClip(clip, 1); soundClips.Add(clip); } }
    public void StopMusic(int index) { StopClip(musicClips[index]); }
    public void StopSound(int index) { try { StopClip(soundClips[index]); } catch { } }
    public void StopMusic(AudioClip clip) { StopClip(clip); }
    public void StopSound(AudioClip clip) { StopClip(clip); }
    public void PlayMusic(int index, bool repeat) { PlayClip(musicClips[index], repeat); }
    public void PlaySound(int index, bool repeat) { PlayClip(soundClips[index], repeat); }
    public void PlayMusic(AudioClip clip, bool repeat) { PlayClip(clip, repeat); }
    public void PlaySound(AudioClip clip, bool repeat) { PlayClip(clip, repeat); }
    public void FadeInMusic(int index, float speed) { FadeInMusic(musicClips[index], speed); }
    public void FadeInMusic(AudioClip clip, float speed) { FadeClip(sourceTable[clip], clip, speed); }
    public void FadeOutMusic(int index, float speed) { FadeOutMusic(musicClips[index], speed); }
    public void FadeOutMusic(AudioClip clip, float speed) { FadeClip(sourceTable[clip], clip, -speed); }
}