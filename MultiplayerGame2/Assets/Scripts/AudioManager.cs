using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public GameObject AudioPrefab;
    public static AudioManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    public void Play3D(AudioClip clip, Vector3 position)
    {
        if (clip == null)
            return;

        var audioGameObject = Instantiate(AudioPrefab, position, Quaternion.identity);
        var source = audioGameObject.GetComponent<AudioSource>();
        source.clip = clip;
        source.Play();
        Destroy(audioGameObject, clip.length);
    }
}
