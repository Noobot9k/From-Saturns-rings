using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogHandler : MonoBehaviour {

    public static DialogHandler Singleton;

    public CanvasGroup Main;
    public Text Dialog;
    public Text Continue;

    public Dictionary<string, string> DialogTable = new Dictionary<string, string>();
    public Dictionary<string, bool> UsedDialog = null;

    float DefaultMass = 100;

    void Awake() {
        Singleton = this;

        DialogTable["StageOfPlayBegin_1"] =     "A lesser god, hoping to be forged in the rings of Saturn and one day become a great guardian planet...";
        DialogTable["StageOfPlayBegin_2"] =     "A planet of life and wonder. So beautiful... and fragile...";
        DialogTable["StageOfPlayBegin_3"] =     "A guardian may grow stronger with each hit they take, but some grow weaker...";
        DialogTable["WinStage1"] =              "Becoming a planet requires one to take a great many hits for themselves, but to become a guardian is to take hits for others.";
        DialogTable["WinStage2"] =              "As one protects they become relied appon, so much so that leaving would bring catastrophe.";
        DialogTable["WinStage3"] =              "If one truly becomes a guardian, few know. They are hardly celebrated. They have a purpose, and they serve in solitude knowing what they do is right. \n (Thanks for playing!)";
        DialogTable["Stage1_MyscDeath1"] =      "Many have this dream. Few ever return.";
        DialogTable["Stage2_MyscDeath1"] =      "To throw everything away for someone else, and not survive to be thanked.";
        DialogTable["Stage3_MyscDeath1"] =      "When a guardian fails, there is not always someone else to take their place, and one does not simply walk away claiming they did their best.";
        DialogTable["DeathSizeSub3000_1"] =     "...To endlessly evade bodies of greater power than them, and consume those lesser.";
        DialogTable["DeathSize10000-20000_1"] = "To grow in size has cost in loss of one's agility. To progress, one must flow with - and around - the forces of nature. Not against them.";
        DialogTable["KilledEarth"] =            "Some hope to become guardians for the glory, but grow vicious - even towards their charge - when they find none.";

        if(UsedDialog == null) {
            UsedDialog = new Dictionary<string, bool>();
            foreach(string key in DialogTable.Keys) {
                UsedDialog[key] = false;
            }
        }
    }
    void Update() {
    }

    public string GetDialog(int Stage, float Mass, bool Died) {
        if (Died) {
            if(Stage <= 2 &&  Mass <= 10000 && !UsedDialog["DeathSizeSub3000_1"]) {
                UsedDialog["DeathSizeSub3000_1"] = true;
                return DialogTable["DeathSizeSub3000_1"];
            } else if(Stage <= 2 && Mass > 10000 && Mass <= 150000 && !UsedDialog["DeathSize10000-20000_1"]) {
                UsedDialog["DeathSize10000-20000_1"] = true;
                return DialogTable["DeathSize10000-20000_1"];
            } else {
                if(!UsedDialog["Stage" + Stage.ToString() + "_MyscDeath1"]) {
                    UsedDialog["Stage" + Stage.ToString() + "_MyscDeath1"] = true;
                    return DialogTable["Stage" + Stage.ToString() + "_MyscDeath1"];
                } else return null;
                //if(Stage == 1) {
                //    if(!UsedDialog["Stage1_MyscDeath1"]) {
                //        UsedDialog["Stage1_MyscDeath1"] = true;
                //        return DialogTable["Stage1_MyscDeath1"];
                //    } else return null;
                //} else if(Stage == 2) {
                //    if(!UsedDialog["Stage2_MyscDeath1"]) {
                //        UsedDialog["Stage2_MyscDeath1"] = true;
                //        return DialogTable["Stage2_MyscDeath1"];
                //    } else return null;
                //} else return null;
            }
        } else {
            if(Mass <= DefaultMass && !UsedDialog["StageOfPlayBegin_" + Stage.ToString()]) {
                UsedDialog["StageOfPlayBegin_" + Stage.ToString()] = true;
                return DialogTable["StageOfPlayBegin_" + Stage.ToString()];
            } else return null;
        }
    }

    public IEnumerator RunDialog(string text, bool pause = true) {
        if(text == null) { print("ooga booga"); yield break; }

        if(pause) Time.timeScale = 0;

        //if(!DialogTable.ContainsKey(TableEntry)) { Debug.LogError(TableEntry + " is not a valid key/has no corresponding value in DialogTable."); yield return null; }
        //string text = DialogTable[TableEntry];
        Dialog.text = text;
        Continue.gameObject.SetActive(true);
        Main.gameObject.SetActive(true);
        Main.alpha = 1;

        yield return new WaitUntil(() => Input.GetButtonDown("Jump"));

        Main.gameObject.SetActive(false);
        Time.timeScale = 1;
    }
}
