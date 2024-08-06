using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public class CutSceneManager : MonoBehaviour
{
    DialogueSequence diaSequence;
    TextAsset diaData;

    public Canvas canvas;
    
    private Coroutine diaCoroutine;

    private DialogueSequence LoadSrc(string fileName)
    {
        string filePath = Path.Combine("Dialogues", fileName);
        diaData = Resources.Load<TextAsset>(filePath);
        return JsonConvert.DeserializeObject<DialogueSequence>(diaData.text);
    }

    public void CutSceneStart(string fileName)
    {
        diaSequence = LoadSrc(fileName);
        if (diaCoroutine != null) StopCoroutine(diaCoroutine);
        diaCoroutine = StartCoroutine(DialogueCor(diaSequence));
    }

    IEnumerator DialogueCor(DialogueSequence diaSequence)
    {
        foreach (Dialogue dialogue in diaSequence.dialogues)
        {
            GameManager.I.uIController.talkerText.text = dialogue.characterName;
            GameManager.I.uIController.contentText.text = dialogue.dialogueText;
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
        }
        GameManager.I.CutSceneEnd();
    }

    [System.Serializable]
    public class Dialogue
    {
        public string characterName;
        public string dialogueText;
    }

    [System.Serializable]
    public class DialogueSequence
    {
        public List<Dialogue> dialogues;
    }
}

