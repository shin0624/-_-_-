using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;

    private Dictionary<string, string> dialogueDictionary = new Dictionary<string, string>();

    void Start()
    {
        LoadDialoguesFromJSON();
    }

    void LoadDialoguesFromJSON()
    {
        TextAsset dialogueAsset = Resources.Load<TextAsset>("Dialogue/dialogue");
        if (dialogueAsset != null)// �� üũ �߰�
        {
            string jsonString = dialogueAsset.text;
            DialogueData dialogueData = JsonUtility.FromJson<DialogueData>(jsonString);
            foreach (Dialogue dialogue in dialogueData.dialogues)
            {
                dialogueDictionary.Add(dialogue.id, dialogue.content);
            }
        }
        else
        {
            Debug.LogError("Dialogue JSON file not found!");
        }
    }

    public void DisplayDialogue(string dialogueID)
    {
        if (dialogueDictionary.ContainsKey(dialogueID))
        {
            dialogueText.text = dialogueDictionary[dialogueID];
            StartCoroutine(ClearDialogueAfterDelay(3f));// ���̾�αװ� ǥ�õǰ� 3�ʰ� ������ ������� �ڷ�ƾ ����
        }
    }

    private IEnumerator ClearDialogueAfterDelay(float delay)//�ڷ�ƾ �߰�
    {
        yield return new WaitForSeconds(delay);
        dialogueText.text = string.Empty;
    }

}

[System.Serializable]
public class DialogueData
{
    public List<Dialogue> dialogues;
}

[System.Serializable]
public class Dialogue
{
    public string id;
    public string content;
}
