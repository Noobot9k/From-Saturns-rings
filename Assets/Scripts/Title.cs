using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Title : MonoBehaviour {

    public Button NewGame;
    public Button Continue;
    public Text HighScoreText;

    SaveData data;

    void Start() {
        data = SaveDataManager.LoadData();
        Continue.interactable = data != null;
        if (data != null) {
            HighScoreText.text = data.HighScore.ToString();
        }
    }
    void Update() {

    }
    public void Action_NewGame() {
        print("Starting new game...");
        SaveDataManager.DeleteData();
        SceneManager.LoadScene("Intro");
    }
    public void Action_ContinueGame() {
        print("Continuing previous game...");
        if(data.StageOfPlay >= 4)
            Action_NewGame();
        else
            SceneManager.LoadScene("Stage" + data.StageOfPlay.ToString());

    }

}
