using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class playerSoundControl : MonoBehaviour
{
    public AudioSource auSource;
    [SerializeField] private AudioClip eatBucketClip;
    [SerializeField] private AudioClip hurtClip;
    [SerializeField] private AudioClip jumpClip;
    [SerializeField] private AudioClip squatClip;
    public void playEatBucketClip()
    {
        auSource.clip = eatBucketClip;
        auSource.Play();
    }
    public void playHurtClip()
    {
        auSource.clip = hurtClip;
        auSource.Play();
    }
    public void playJumpClip()
    {
        auSource.clip = jumpClip;
        auSource.Play();
    }
    public void playSquatClip()
    {
        auSource.clip = squatClip;
        auSource.Play();
    }
}
