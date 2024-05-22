using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData {

    public int StageOfPlay;
    public float HighScore;
    public Dictionary<string, bool> UsedDialog;

    public SaveData(int stageOfPlay, float highScore, Dictionary<string, bool> usedDialog) {
        StageOfPlay = stageOfPlay;
        HighScore = highScore;
        UsedDialog = usedDialog;
    }
    public SaveData(float highScore) {
        HighScore = highScore;
    }
}
