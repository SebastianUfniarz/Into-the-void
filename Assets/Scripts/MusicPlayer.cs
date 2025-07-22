using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public AudioSource introSource;
    public AudioSource loopSource;
    private double goalTime;

    private void Start()
    {
        goalTime = AudioSettings.dspTime + 0.5;
        introSource.PlayScheduled(goalTime);

        double introDuration = (double)introSource.clip.samples / introSource.clip.frequency;
        loopSource.PlayScheduled(goalTime + introDuration);

        loopSource.loop = true;
    }
}
