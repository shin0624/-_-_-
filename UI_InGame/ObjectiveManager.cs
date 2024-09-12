using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class ObjectiveManager : MonoBehaviour
{
    public TextMeshProUGUI newObjective;
    public TextMeshProUGUI objective;
    public TextMeshProUGUI newObjectiveText;
    private Dictionary<string, string> objectiveDictionary = new Dictionary<string, string>();

    void Start()
    {

        LoadObjectivesFromJSON();
        Color initialColor = newObjectiveText.color;
        initialColor.a = 0;
        newObjectiveText.color = initialColor;
    }

    void LoadObjectivesFromJSON()
    {
        // Resources �������� JSON ���� �ε�
        TextAsset jsonTextAsset = Resources.Load<TextAsset>("Objective/objective");
        if (jsonTextAsset != null)
        {
            string jsonString = jsonTextAsset.text;
            // JSON �Ľ�
            try
            {
                ObjectiveData objectiveData = JsonUtility.FromJson<ObjectiveData>(jsonString);
                foreach (Objective objective in objectiveData.objective)
                {
                    // ��ųʸ��� ��ǥ ID�� ���� �߰�
                    objectiveDictionary.Add(objective.id, objective.content);
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError("JSON �Ľ� ����: " + ex.Message);
            }
        }
        else
        {
            Debug.LogError("Objective JSON ������ ã�� �� �����ϴ�.");
        }
    }

    public void DisplayObjectives(string newObjectiveID, string objectiveID)
    {
        if (objectiveDictionary.ContainsKey(objectiveID))
        {
            objective.text = objectiveDictionary[objectiveID];
        }

        if (objectiveDictionary.ContainsKey(newObjectiveID))
        {
            StopAllCoroutines();
            newObjective.text = objectiveDictionary[newObjectiveID];
            StartCoroutine(FadeInTexts(newObjective, newObjectiveText, 1f));
            StartCoroutine(ClearTextAfterDelay(newObjective, newObjectiveText, 5f));
        }
    }

    private IEnumerator ClearTextAfterDelay(TextMeshProUGUI newObjective, TextMeshProUGUI newObjectiveText, float delay)
    {
        yield return new WaitForSeconds(delay);
        StartCoroutine(FadeOutTexts(newObjective, newObjectiveText, 1f));
    }

    private IEnumerator FadeInTexts(TextMeshProUGUI newObjective, TextMeshProUGUI newObjectiveText, float duration)
    {
        Color colorNewObjective = newObjective.color;
        Color colorNewText = newObjectiveText.color;
        colorNewObjective.a = 0;
        colorNewText.a = 0;
        newObjective.color = colorNewObjective;
        newObjectiveText.color = colorNewText;

        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / duration);
            colorNewObjective.a = alpha;
            colorNewText.a = alpha;
            newObjective.color = colorNewObjective;
            newObjectiveText.color = colorNewText;
            yield return null;
        }
    }

    private IEnumerator FadeOutTexts(TextMeshProUGUI newObjective, TextMeshProUGUI newObjectiveText, float duration)
    {
        Color colorNewObjective = newObjective.color;
        Color colorNewText = newObjectiveText.color;
        float startAlphaNewObjective = colorNewObjective.a;
        float startAlphaNewText = colorNewText.a;

        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(startAlphaNewObjective - (elapsedTime / duration));
            colorNewObjective.a = alpha;
            colorNewText.a = alpha;
            newObjective.color = colorNewObjective;
            newObjectiveText.color = colorNewText;
            yield return null;
        }

        newObjective.text = string.Empty;
        Color finalColor = newObjectiveText.color;
        finalColor.a = 0;
        newObjectiveText.color = finalColor;
    }
}

[System.Serializable]
public class ObjectiveData
{
    public List<Objective> objective; // JSON�� �迭�� �޴� ����Ʈ
}

[System.Serializable]
public class Objective
{
    public string id;
    public string content;
}
