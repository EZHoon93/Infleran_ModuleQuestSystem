using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AchievementDetailView : MonoBehaviour
{

    [SerializeField]
    private Image achievementIcon;
    [SerializeField]
    private TextMeshProUGUI titleText;
    [SerializeField]
    private TextMeshProUGUI description;
    [SerializeField]
    private Image rewardIcon;
    [SerializeField]
    private TextMeshProUGUI rewardText;
    [SerializeField]
    private GameObject completionScreen;

    private Quest target;
    private void OnDestroy()
    {
        if(target != null)
        {
            target.onTaskSuccessChanged -= UpdateDescription;
            target.onCompleted -= ShowCompletetionScreen;
        }
    }
    public void Setup(Quest achievement)
    {
        target = achievement;
        achievementIcon.sprite = achievement.Icon;
        titleText.text = achievement.DisplayName;

        var task = achievement.CurrentTaskGroup.Tasks[0];
        description.text = BuildTaskDscription(task);

        var reward = achievement.Rewards[0];
        rewardIcon.sprite = reward.Icon;
        rewardText.text = $"{reward.Description} +{reward.Quantity}";

        if (achievement.IsComplatable)
            completionScreen.SetActive(true);
        else
        {
            completionScreen.SetActive(false);
            achievement.onTaskSuccessChanged += UpdateDescription;
            achievement.onCompleted += ShowCompletetionScreen;
        }
    }
    private void UpdateDescription(Quest achievement, Task task, int currentSuccess, int prevSuccess) => description.text = BuildTaskDscription(task);

    private void ShowCompletetionScreen(Quest achievement) => completionScreen.SetActive(true);
    private string BuildTaskDscription(Task task) => $"{task.Description} {task.CurrentSuccess}/{task.NeedSucessToComplete}";
}
