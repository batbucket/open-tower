using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {
    private static SoundManager _instance;

    [SerializeField]
    private AudioSource loop;

    [SerializeField]
    private GameObject oneshots;

    [SerializeField]
    private MusicPersistence persistencePrefab;

    private MusicPersistence persistence;

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

    private void Start() {
        persistence = FindObjectOfType<MusicPersistence>();
        if (persistence != null) {
            if (persistence.loop == this.loop.clip) {
                this.loop.time = persistence.time;
            }
        } else {
            persistence = Instantiate(persistencePrefab);
            DontDestroyOnLoad(persistence);
        }
    }

    private void Update() {
        persistence.loop = this.loop.clip;
        persistence.time = this.loop.time;
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