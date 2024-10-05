using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SE : MonoBehaviour
{

    private AudioSource audioSource;
// Use this for initialization
void Start()
{
    audioSource = GetComponent<AudioSource>();
    audioSource.loop = true;  // 音楽をループさせる
}

    // Update is called once per frame
void Update()
{
    if (Input.GetMouseButton(0))  // 左マウスボタンが押されている間
    {
        if (!audioSource.isPlaying)
        {
            audioSource.Play();  // 音楽を再生する
        }
    }
    else
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();  // 音楽を停止する
        }
    }
}
}