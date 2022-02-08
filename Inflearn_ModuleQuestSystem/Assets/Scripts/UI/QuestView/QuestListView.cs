using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.Events;

public class QuestListView : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI elementTextPrefab;

    private Dictionary<Quest, GameObject> elementsByQuest = new Dictionary<Quest, GameObject>();
    private ToggleGroup _toggleGroup;

    private void Awake()
    {
        _toggleGroup = GetComponent<ToggleGroup>();

        
    }
    public void AddElement(Quest quest , UnityAction<bool> onClidked)
    {
        var element = Instantiate(elementTextPrefab, transform);
        element.text = quest.DisplayName;

        var toggle = element.GetComponent<Toggle>();
        toggle.group = _toggleGroup;
        toggle.onValueChanged.AddListener(onClidked);

        elementsByQuest.Add(quest, element.gameObject);

    }

    public void RemoveElement(Quest quest)
    {
        Destroy(elementsByQuest[quest]);
        elementsByQuest.Remove(quest);
    }
}
