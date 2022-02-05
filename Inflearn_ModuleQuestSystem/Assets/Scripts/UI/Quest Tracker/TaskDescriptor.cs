using System.Collections;
using System.Collections.Generic;

using TMPro;

using UnityEngine;

public class TaskDescriptor : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI text;
    [SerializeField]
    private Color normalColor;
    [SerializeField]
    private Color taskCompletionColor;
    [SerializeField]
    private Color taskSuccessCountColor;
    [SerializeField]
    private Color strikeThrouchColor;

    public void UpdateText(string text)
    {
        this.text.fontStyle = FontStyles.Normal;
        this.text.text = text;
    }

    /// <summary>
    /// task가 진행중일때 현재값 빨강 완료일때 전체문장 초록
    /// </summary>
    public void UpdateText(Task task)
    {
        text.fontStyle = FontStyles.Normal;

        if (task.IsComplete)
        {
            var colorCode  = ColorUtility.ToHtmlStringRGB(taskCompletionColor);
            text.text = BuildText(task, colorCode, colorCode);
        }
        else
        {
            text.text = BuildText(task, ColorUtility.ToHtmlStringRGB(normalColor), ColorUtility.ToHtmlStringRGB(taskSuccessCountColor));
        }
    }

    public void UpdateTextUsingStrikeThrought(Task task)
    {
        var colorCode = ColorUtility.ToHtmlStringRGB(strikeThrouchColor);
        text.fontStyle = FontStyles.Strikethrough;
        text.text = BuildText(task, colorCode, colorCode);
    }



    private string BuildText(Task task , string textColorCode , string successCountColorCode)
    {
        return $"<color=#{textColorCode}> {task.Description} <color=#{successCountColorCode}>{task.CurrentSuccess}</color>/{task.NeedSucessToComplete}</color>";
    }
}
