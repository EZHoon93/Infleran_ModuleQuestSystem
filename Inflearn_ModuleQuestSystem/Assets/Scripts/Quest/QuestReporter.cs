using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class QuestReporter : MonoBehaviour
{

    [SerializeField]
    private Category category;
    [SerializeField]
    private TaskTarget target;
    [SerializeField]
    private int successCount;
    [SerializeField]
    private string[] colliderTags;

    private void OnTriggerEnter(Collider other)
    {
        ReportItPassCondition(other);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ReportItPassCondition(collision);
    }

    public void Report()
    {
        QuestSystem.Instance.ReceiveReport(category, target, successCount);
    }

    private void ReportItPassCondition(Component other)
    {
        if(colliderTags.Any(x => other.CompareTag(x)))
        {
            Report();
        }
    }
}
