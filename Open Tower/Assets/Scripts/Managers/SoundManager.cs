using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {
    private static SoundManager _instance;

    [SerializeField]
    private AudioSource loop;

    [SerializeField]
    private GameObject oneshots;

    public static SoundManager Instance {
        get {
            if (_instance == null) {
                _instance = FindObjectOfType<SoundManager>();
            }
            return _instance;
        }
    }

    public void Loop(AudioClip clip) {
        loop.clip = clip;
    }

    public void Play(AudioClip clip) {
        StartCoroutine(PlayThenDestroy(clip));
    }

    private IEnumerator PlayThenDestroy(AudioClip clip) {
        GameObject go = new GameObject();
        AudioSource source = go.AddComponent<AudioSource>();
        source.PlayOneShot(clip);
        source.transform.SetParent(oneshots.transform);
        yield return new WaitWhile(() => source.isPlaying);
        Destroy(go);
    }
}