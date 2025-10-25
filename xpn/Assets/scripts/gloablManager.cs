using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
public class gloablManager : MonoBehaviour
{
    public int gameIndex;
    public bool isDie;
    public static gloablManager instance;
    public player player;
    public bossControl boss;
    [SerializeField] private AudioSource audioPlayer;
    [SerializeField] private AudioClip dieClip;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        isDie = false;
    }
    private void OnDestroy()
    {
        instance = null;
    }
    private void Update()
    {
        if (Input.GetMouseButtonUp(0) && EventSystem.current.IsPointerOverGameObject())
            audioPlayer.Play();
        if (Input.GetKeyUp(KeyCode.R))
            if (isDie)
                SceneManager.LoadScene(gameIndex);
    }
    public void playDieClip()
    {
        audioPlayer.clip = dieClip;
        audioPlayer.Play();
    }
}
