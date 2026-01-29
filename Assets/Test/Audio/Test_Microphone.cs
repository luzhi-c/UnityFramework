using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Microphone : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip audioClip;

    private string audioClipStr = "Built-in Microphone";
    // Start is called before the first frame update
    void Start()
    {
        var devices = Microphone.devices;
        if (devices.Length <= 0)
        {
            return;
        }
        audioClipStr = devices[0];
        // audioSource.clip = Microphone.Start(audioClipStr, true, 10, 44100);
        // audioSource.Play();
    }

    public void OnGUI()
    {
        if (GUILayout.Button("开始录音"))
        {
            audioClip = Microphone.Start(audioClipStr, true, 10, 44100);
        }
        if (GUILayout.Button("结束录音"))
        {
            if (audioClip)
            {
                Microphone.End(audioClipStr);
            }
        }

        if (GUILayout.Button("播放录音"))
        {
            if (audioClip)
            {
                audioSource.clip = audioClip;
                audioSource.Play();
            }
        }
    }
}
