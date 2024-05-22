using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour {

    public static GameControl Singleton;
    SaveData PlayerSaveData = null;

    public Player PlayerObj;
    GravitationalObject playerGravObj;
    bool playerWasPresentLastFrame = true;

    public int StageOfPlay = 1;

    void Start() {
        Singleton = this;
        playerGravObj = PlayerObj.GetComponent<GravitationalObject>();

        PlayerSaveData = SaveDataManager.LoadData();
        if (PlayerSaveData != null) {
            DialogHandler.Singleton.UsedDialog = PlayerSaveData.UsedDialog;
            
        }

        //StartCoroutine(DialogHandler.Singleton.RunDialog("StageOfPlayBegin_" + StageOfPlay.ToString()));
        StartCoroutine(DialogHandler.Singleton.RunDialog(DialogHandler.Singleton.GetDialog(StageOfPlay, playerGravObj.GetMass(), false)));
    }
    void Update() {
        if(Time.timeScale <= 0) return;

        if(PlayerObj == null && playerWasPresentLastFrame == true) {
            //Invoke("Die", 3);
            StartCoroutine(Die());
            playerWasPresentLastFrame = false;
        }
        //GravitationalObject.GravityMultiplier = playerGravObj.GetMass() / 1600;

        if(playerGravObj.GetMass() > 150000 && StageOfPlay == 1) {
            StartCoroutine(Win());
        } else if(playerGravObj.GetMass() > 350000 && StageOfPlay == 2) {
            StartCoroutine(Win());
        } else if (playerGravObj.GetMass() > 35000 && StageOfPlay == 3) {
            StartCoroutine(Win());
        }
    }

    public IEnumerator Die(bool pause = false, bool NoDialog = false) {
        float PlayerEndMass = playerGravObj.GetMass();
        print(PlayerEndMass);

        if(pause) Time.timeScale = 0;

        if(!NoDialog) {
            yield return new WaitForSecondsRealtime(3);
            yield return DialogHandler.Singleton.RunDialog(DialogHandler.Singleton.GetDialog(StageOfPlay, PlayerEndMass, true), false);
        }
        
        Save(PlayerEndMass);

        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public IEnumerator Win() {
        float PlayerEndMass = playerGravObj.GetMass();
        print(PlayerEndMass);

        Time.timeScale = 0;
        //yield return new WaitForSecondsRealtime(1);
        yield return DialogHandler.Singleton.RunDialog(DialogHandler.Singleton.DialogTable["WinStage" + StageOfPlay.ToString()], true);

        StageOfPlay ++;
        Save(PlayerEndMass);

        if(StageOfPlay <= 3)
            SceneManager.LoadScene("Stage"+StageOfPlay.ToString());
        else
            SceneManager.LoadScene("Title");
    }
    void Save(float PlayerEndMass) {
        if(PlayerSaveData == null) PlayerSaveData = new SaveData(PlayerEndMass);
        if(PlayerEndMass > PlayerSaveData.HighScore) PlayerSaveData.HighScore = PlayerEndMass;
        PlayerSaveData.StageOfPlay = StageOfPlay;
        PlayerSaveData.UsedDialog = DialogHandler.Singleton.UsedDialog;
        SaveDataManager.SaveData(PlayerSaveData);
    }
    [ContextMenu("Delete existing save data")]
    void DeleteSaveData() {
        SaveDataManager.DeleteData();
    }
}
