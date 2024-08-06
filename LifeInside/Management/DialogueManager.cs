using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Newtonsoft.Json;
using System.IO;
using System.Linq;
using System.Data;
using System;

public class DialogueManager : MonoBehaviour
{
    private TextAsset        diaData;
    private DialogueSequence diaSequence;
    private List<Dialogue>   diaSorted;
    
    private TextAsset             evtDiaData;
    private EventDialogueSequence evtDiaSequence;



    private PlayerController pController;
    private UIController uIController;

    public  List<string> diaCompleted;
    bool playing = false;


    void Start()
    {
        LoadSrc(); 

        diaCompleted = new List<string>();

        pController  = FindAnyObjectByType<PlayerController>();
        uIController = FindAnyObjectByType<UIController>();

    }


    private void LoadSrc()
    {
        string filePath = Path.Combine("Dialogues", "dialogFile");
        diaData     = Resources.Load<TextAsset>(filePath);
        diaSequence = JsonConvert.DeserializeObject<DialogueSequence>(diaData.text);
    }

    private void LoadSrcFile(string jsonFileName)
    {
        try
        {
            string removedFilename = RemoveInvalidChars(jsonFileName);
            string filePath = Path.Combine("Dialogues", removedFilename);

            evtDiaData      = Resources.Load<TextAsset>(filePath);
            evtDiaSequence  = JsonConvert.DeserializeObject<EventDialogueSequence>(evtDiaData.text);
        }
        catch(Exception e)
        {
            Debug.LogError($"Failed to get Json File: [{jsonFileName}] \n Error: {e}");
            return;
        }
    }

    public string RemoveInvalidChars(string filename)
    {
        return string.Concat(filename.Split(Path.GetInvalidFileNameChars()));
    }


    public void PlayEvtDialogueFile(string jsonFileName)
    {
        LoadSrcFile(jsonFileName);

        uIController.EnableWindow();

        StartCoroutine( PlayEvtDialogueCo(evtDiaSequence.dialogues) );
    }

    IEnumerator PlayEvtDialogueCo(List<EventDialogue> dias )
    {
        GameManager.I.eventManager.isCurDialogEnd = false;

        playing = true;
        Time.timeScale = 0;

        if(dias == null){ Debug.Log("dias is null"); yield return null; }

        foreach (EventDialogue dialogue in dias)
        {
            
            Debug.Log(dialogue.characterName + " : " + dialogue.dialogueText);

            uIController.talkerText.text  = dialogue.characterName;
            uIController.contentText.text = dialogue.dialogueText;

            yield return new WaitUntil(() => Input.GetKeyDown( KeyCode.Return ));
            yield return new WaitForEndOfFrame(); // 키 중복입력 방지
            yield return new WaitForEndOfFrame(); // 키 중복입력 방지
        
        }

        playing = false;
        Time.timeScale = 1;
        GameManager.I.inputManager.UnLockInput();

        GameManager.I.eventManager.isCurDialogEnd = true;
        uIController.DisableWindow();
        uIController.uIEffect.EffectEnd();
    }

    public bool CheckComplete(string eventPoint_)
    {
        foreach(string eventPoint in diaCompleted)
        {
            if( eventPoint == eventPoint_){
                Debug.Log("Complete : " + eventPoint_);
                return true;
            }
        }
        return false;
    }

}


[System.Serializable]
public class Dialogue
{
    public string eventPoint;
    public string characterName;
    public string dialogueText;
    public float  lineTimeout;
}

[System.Serializable]
public class DialogueSequence
{
    public List<Dialogue> dialogues;
}

[System.Serializable]
public class EventDialogue
{
    public string characterName;
    public string dialogueText;
}

[System.Serializable]
public class EventDialogueSequence
{
    public List<EventDialogue> dialogues;
}