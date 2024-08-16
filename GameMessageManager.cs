using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class GameMessageManager : MonoBehaviour
{
    public TextMeshProUGUI dialogueText; // ��� �ؽ�Ʈ�� ǥ���� UI ���
    public TextMeshProUGUI objectiveText; // ��ǥ �ؽ�Ʈ�� ǥ���� UI ���
    public TextMeshProUGUI newObjectiveText; // ���ο� ��ǥ �ؽ�Ʈ�� ǥ���� UI ���

    private Dictionary<string, string> dialogueDictionary = new Dictionary<string, string>(); // ��� ��ųʸ�
    private Dictionary<string, string> objectiveDictionary = new Dictionary<string, string>(); // ��ǥ ��ųʸ�

    void Start()
    {
        LoadDialoguesFromJSON();
        LoadObjectivesFromJSON();
    }

    void LoadDialoguesFromJSON()
    {
        // JSON ������ �ҷ��� ��ųʸ��� ���� (���)
        TextAsset jsonTextAsset = Resources.Load<TextAsset>("Json/dialogue");
        if (jsonTextAsset != null)
        {
            string jsonString = jsonTextAsset.text;
            DialogueData dialogueData = JsonUtility.FromJson<DialogueData>(jsonString);

            foreach (Dialogue dialogue in dialogueData.dialogues)
            {
                dialogueDictionary.Add(dialogue.id, dialogue.content);
            }
        }
        else
        {
            Debug.LogWarning("Dialogue JSON ������ �ε��� �� �����ϴ�.");
        }
    }

    void LoadObjectivesFromJSON()
    {
        // JSON ������ �ҷ��� ��ųʸ��� ���� (��ǥ)
        TextAsset jsonTextAsset = Resources.Load<TextAsset>("Json/objective");
        if (jsonTextAsset != null)
        {
            string jsonString = jsonTextAsset.text;
            ObjectiveData objectiveData = JsonUtility.FromJson<ObjectiveData>(jsonString);

            foreach (Objective objective in objectiveData.objective)
            {
                objectiveDictionary.Add(objective.id, objective.content);
            }
        }
        else
        {
            Debug.LogWarning("Objective JSON ������ �ε��� �� �����ϴ�.");
        }
    }

    public void DisplayDialogue(string dialogueID)
    {
        // ��� �ؽ�Ʈ ����
        if (dialogueDictionary.ContainsKey(dialogueID))
        {
            dialogueText.text = dialogueDictionary[dialogueID];
            StartCoroutine(FadeInText(dialogueText, 1f)); // �ؽ�Ʈ ���̵� ��
            StartCoroutine(ClearTextAfterDelay(dialogueText, 5f)); // ���� �ð� �� ���̵� �ƿ�
        }
    }

    public void DisplayObjectives(string newObjectiveID, string objectiveID)
    {
        // ���ο� ��ǥ �ؽ�Ʈ ����
        if (objectiveDictionary.ContainsKey(newObjectiveID))
        {
            newObjectiveText.text = objectiveDictionary[newObjectiveID];
            StartCoroutine(FadeInText(newObjectiveText, 1f));
            StartCoroutine(ClearTextAfterDelay(newObjectiveText, 5f));
        }

        // ��ǥ �ؽ�Ʈ ����
        if (objectiveDictionary.ContainsKey(objectiveID))
        {
            objectiveText.text = objectiveDictionary[objectiveID];
        }
    }

    private IEnumerator ClearTextAfterDelay(TextMeshProUGUI text, float delay)
    {
        yield return new WaitForSeconds(delay);
        StartCoroutine(FadeOutText(text, 1f)); // �ؽ�Ʈ ���̵� �ƿ�
    }

    private IEnumerator FadeInText(TextMeshProUGUI text, float duration)
    {
        yield return FadeText(text, duration, 0f, 1f);
    }

    private IEnumerator FadeOutText(TextMeshProUGUI text, float duration)
    {
        yield return FadeText(text, duration, 1f, 0f);
    }

    private IEnumerator FadeText(TextMeshProUGUI text, float duration, float startAlpha, float endAlpha)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            SetTextAlpha(text, alpha);
            yield return null;
        }

        SetTextAlpha(text, endAlpha);
    }

    private void SetTextAlpha(TextMeshProUGUI text, float alpha)
    {
        Color color = text.color;
        color.a = alpha;
        text.color = color;
    }
}

[System.Serializable]
public class DialogueData
{
    public List<Dialogue> dialogues; // ��� ����Ʈ
}

[System.Serializable]
public class Dialogue
{
    public string id; // ��� ID
    public string content; // ��� ����
}

[System.Serializable]
public class ObjectiveData
{
    public List<Objective> objective; // ��ǥ ����Ʈ
}

[System.Serializable]
public class Objective
{
    public string id; // ��ǥ ID
    public string content; // ��ǥ ����
}
