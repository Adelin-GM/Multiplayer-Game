using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualEffectsManager : MonoBehaviour
{
    public static VisualEffectsManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    public void Play(GameObject VFXObject, Vector3 position) 
    {
        if (VFXObject == null)
            return;

        var vfxGameObject = Instantiate(VFXObject, position, Quaternion.identity);
        var particleSystems = vfxGameObject.GetComponentsInChildren<ParticleSystem>();

        var maxLength = 0f;
        foreach (var particleSystem in particleSystems)
        {
            var currentLength = particleSystem.main.duration + particleSystem.main.startLifetime.constantMax;
            if (currentLength > maxLength)
                maxLength = currentLength;
        }

        Destroy(vfxGameObject, maxLength);
    }
}
