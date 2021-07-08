using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Neet.Data;
using Neet.UI;
using System.Drawing;
using TMPro;

public class TargetTrackPeekScoreboard : MonoBehaviour
{
    private TargetTrackPeek game;

    public float pressTimeThreshold;
    private float lastReleaseTime;

    public TextMeshProUGUI hitText;


    private void Awake()
    {
        game = GetComponent<TargetTrackPeek>();
    }

    public void StartPlaying()
    {
        StartCoroutine(TrackPlayerSpeed());
    }
    public void StopPlaying()
    {
        StopCoroutine(TrackPlayerSpeed());
    }

    IEnumerator TrackPlayerSpeed()
    {
        bool waitingForRelease = false;
        while (true)
        {
            var pressing = game.playerMotor.isPressing;
            if (waitingForRelease && !pressing)
            {
                InputReleased();
                waitingForRelease = false;
            }
            else if (pressing)
            {
                waitingForRelease = true;
            }

            yield return null;
        }
    }
    private void InputReleased()
    {
        lastReleaseTime = Time.time;
    }

    public void Hit(GameObject target)
    {
        float timeToKill = Time.time - target.GetData<float>(Target.SPAWN_TIME_KEY);
        float timeToStop = Time.time - lastReleaseTime;


        string ttk = (timeToKill * 1000).ToString("f0").Colorize(GetTTKColor(timeToKill)) + "ms";
        string tts = (timeToStop * 1000).ToString("f0").Colorize(GetTTSColor(timeToStop)) + "ms";
        string line = "TTK: " + ttk + " | " + "TTS: " + tts;

        hitText.text = line;
    }

    private Color GetTTSColor(float tts)
    {
        float idealTTS = 8f / 60f;
        Color c;
        if (tts < idealTTS)
            c = Color.red;
        else
        {
            var t = (tts - idealTTS) / .5f;
            c = GameExtensions.LerpPlus(Color.green, Color.white, t);
        }
        return c;
    }
    private Color GetTTKColor(float ttk)
    {
        float idealTTK = .3f + (8f / 60f);
        Color c;
        var t = (ttk - idealTTK) / .5f;
        c = GameExtensions.LerpPlus(Color.green, Color.white, t);
        return c;
    }
}
