using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "LevelSetup/LevelData", order = 1)]
public class LevelSetUp : ScriptableObject
{
    public WaveSetup[] waveSetups;
    public List<WaveSetup> listEndlessWave = new List<WaveSetup>();
}
