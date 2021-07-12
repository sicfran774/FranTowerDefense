using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StatsData
{
    public float sfxVolume;
    public float musicVolume;
    public int currency;
    public bool sandboxMode;
    public StatsData(Stats stats)
    {
        sfxVolume = stats.sfxVolume;
        musicVolume = stats.musicVolume;
        currency = stats.currency;
        sandboxMode = stats.sandboxMode;
    }
}

