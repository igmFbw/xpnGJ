using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class playerSoundControl : MonoBehaviour
{
    [SerializeField] private AudioSource auSource;
    [SerializeField] private AudioClip eatBucketClip;
    public void playEatBucketClip()
    {
        auSource.clip = eatBucketClip;
        auSource.Play();
    }
}
