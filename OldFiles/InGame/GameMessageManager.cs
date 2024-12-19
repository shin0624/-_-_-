/*
 * GameMessageManager.cs
 * �� ��ũ��Ʈ�� ���� ������ ��� �� ��ǥ �޽����� ����
 * JSON ���Ͽ��� ��� �� ��ǥ �����͸� �ε��ϰ�, �̸� ó���Ͽ� UI ��Ҹ� ������Ʈ
 * Assets/Resources/JSON ��ο� dialogue.json, objective.json ���� �ʿ�
 */
using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class GameMessageManager : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI dialogueText;            // ��� �ؽ�Ʈ UI ���
    public TextMeshProUGUI ObjectiveText;           // ��ǥ �ؽ�Ʈ UI ���
    public TextMeshProUGUI currentObjectiveText;    // ���� ��ǥ �ؽ�Ʈ UI ���

    // ���� ��ǥ ID�� ������ �����ϴ� ��ųʸ�
    private Dictionary<string, string> dialogueDictionary = new Dictionary<string, string>();
    private Dictionary<string, string> objectiveDictionary = new Dictionary<string, string>();

    // ���� ��ǥ�� �����ϴ� ť
    private Queue<string> dialogueQueue = new Queue<string>();
    private Queue<(string, string)> objectiveQueue = new Queue<(string, string)>();

    // ���� ��ǥ�� ó���ϴ� �ڷ�ƾ�� ���� ����
    private Coroutine dialogueCoroutine;
    private Coroutine objectiveCoroutine;

    void Start()
    {
        // JSON ���Ͽ��� ��� �� ��ǥ �����͸� �ε�
        LoadData();
    }

    // JSON ���Ͽ��� ���� ��ǥ �����͸� �ε��ϴ� �޼���
    void LoadData()
    {
        LoadDialoguesFromJSON();
        LoadObjectivesFromJSON();
    }

    // JSON ���Ͽ��� ��� �����͸� �ε��Ͽ� ��ųʸ��� �����ϴ� �޼���
    void LoadDialoguesFromJSON()
    {
        TextAsset jsonTextAsset = Resources.Load<TextAsset>("JSON/dialogue");
        if (jsonTextAsset != null)
        {
            DialogueData dialogueData = JsonUtility.FromJson<DialogueData>(jsonTextAsset.text);
            foreach (Dialogue dialogue in dialogueData.dialogues)
            {
                dialogueDictionary[dialogue.id] = dialogue.content;
            }
        }
        else
        {
            // Debug.LogWarning("Dialogue JSON ������ �ε��� �� �����ϴ�.");
        }
    }

    // JSON ���Ͽ��� ��ǥ �����͸� �ε��Ͽ� ��ųʸ��� �����ϴ� �޼���
    void LoadObjectivesFromJSON()
    {
        TextAsset jsonTextAsset = Resources.Load<TextAsset>("JSON/objective");
        if (jsonTextAsset != null)
        {
            ObjectiveData objectiveData = JsonUtility.FromJson<ObjectiveData>(jsonTextAsset.text);
            foreach (Objective objective in objectiveData.objective)
            {
                objectiveDictionary[objective.id] = objective.content;
            }
        }
        else
        {
            // Debug.LogWarning("Objective JSON ������ �ε��� �� �����ϴ�.");
        }
    }

    // ��縦 ť�� �߰��ϰ�, ��簡 ó���ǰ� ���� ���� ��� �ڷ�ƾ�� ����
    public void DisplayDialogue(string dialogueID)
    {
        if (dialogueDictionary.TryGetValue(dialogueID, out string dialogueContent))
        {
            dialogueQueue.Enqueue(dialogueID);
            if (dialogueCoroutine == null)
            {
                dialogueCoroutine = StartCoroutine(ProcessDialogueQueue());
            }
        }
    }

    // �� ��ǥ�� ���� ��ǥ�� ť�� �߰��ϰ�, ��ǥ�� ó���ǰ� ���� ���� ��� �ڷ�ƾ�� ����
    public void DisplayObjective(string objectiveID, string currentObjectiveID)
    {
        if (objectiveDictionary.TryGetValue(objectiveID, out string objectiveContent))
        {
            objectiveQueue.Enqueue((objectiveID, currentObjectiveID));
            if (objectiveCoroutine == null)
            {
                objectiveCoroutine = StartCoroutine(ProcessObjectiveQueue());
            }
        }
    }

    // ��� ť�� ó���ϴ� �ڷ�ƾ. ��� ������ ǥ���ϰ�, ���̵� ȿ���� ����
    private IEnumerator ProcessDialogueQueue()
    {
        while (dialogueQueue.Count > 0)
        {
            string dialogueID = dialogueQueue.Dequeue();
            dialogueText.text = dialogueDictionary[dialogueID];
            yield return StartCoroutine(FadeInText(dialogueText, 1f));
            yield return new WaitForSeconds(3f);
            yield return StartCoroutine(FadeOutText(dialogueText, 1f));
        }
        dialogueCoroutine = null;
    }

    // ��ǥ ť�� ó���ϴ� �ڷ�ƾ. �� ��ǥ�� ���� ��ǥ�� ǥ���ϰ�, ���̵� ȿ���� ����
    private IEnumerator ProcessObjectiveQueue()
    {
        while (objectiveQueue.Count > 0)
        {
            var (objectiveID, currentObjectiveID) = objectiveQueue.Dequeue();
            if (objectiveDictionary.TryGetValue(objectiveID, out string objectiveContent))
            {
                ObjectiveText.text = objectiveContent;
                yield return StartCoroutine(FadeInText(ObjectiveText, 1f));
                yield return new WaitForSeconds(3f);
                yield return StartCoroutine(FadeOutText(ObjectiveText, 1f));
            }

            if (objectiveDictionary.TryGetValue(currentObjectiveID, out string currentObjectiveContent))
            {
                currentObjectiveText.text = currentObjectiveContent;
            }
        }
        objectiveCoroutine = null;
    }

    // �ؽ�Ʈ�� ���̵��� ȿ���� �����ϴ� �ڷ�ƾ
    private IEnumerator FadeInText(TextMeshProUGUI text, float duration)
    {
        yield return FadeText(text, duration, 0f, 1f);
    }

    // �ؽ�Ʈ�� ���̵�ƿ� ȿ���� �����ϴ� �ڷ�ƾ
    private IEnumerator FadeOutText(TextMeshProUGUI text, float duration)
    {
        yield return FadeText(text, duration, 1f, 0f);
    }

    // �ؽ�Ʈ�� ���̵� ȿ���� �����ϴ� �ڷ�ƾ
    private IEnumerator FadeText(TextMeshProUGUI text, float duration, float startAlpha, float endAlpha)
    {
        float elapsedTime = 0f;
        Color color = text.color;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / duration);
            SetTextAlpha(text, alpha);
            yield return null;
        }

        SetTextAlpha(text, endAlpha);
    }

    // �ؽ�Ʈ�� ���� ���� �����ϴ� �޼���
    private void SetTextAlpha(TextMeshProUGUI text, float alpha)
    {
        Color color = text.color;
        color.a = alpha;
        text.color = color;
    }
}

// ��ȭ �������� JSON ���Ŀ� �����ϴ� Ŭ����
[System.Serializable]
public class DialogueData
{
    public List<Dialogue> dialogues; // ��� ���
}

// ��ȭ �׸��� ��Ÿ���� Ŭ����
[System.Serializable]
public class Dialogue
{
    public string id;       // ��� ID
    public string content;  // ��� ����
}

// ��ǥ �������� JSON ���Ŀ� �����ϴ� Ŭ����
[System.Serializable]
public class ObjectiveData
{
    public List<Objective> objective;   // ��ǥ ���
}

// ��ǥ �׸��� ��Ÿ���� Ŭ����
[System.Serializable]
public class Objective
{
    public string id;       // ��ǥ ID
    public string content;  // ���� ���� 
}