using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Collections.Concurrent;
using TMPro;
public class QuestCompleteionNotifier : MonoBehaviour
{
    [SerializeField]
    private string titleDescription;

    [SerializeField]
    private TextMeshProUGUI titleText;
    [SerializeField]
    private TextMeshProUGUI rewardText;
    [SerializeField]
    private float showTime = 3f;

    private Queue<Quest> reserverdQuests = new Queue<Quest>();
    private StringBuilder stringBuilder = new StringBuilder();

    private void Start()
    {
        var questSystem = QuestSystem.Instance;
        questSystem.onQuestCompleted += Notify;
        questSystem.onAchievementCompleted += Notify;

        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        var questSystem = QuestSystem.Instance;
        if(questSystem != null)
        {
            questSystem.onQuestCompleted -= Notify;
            questSystem.onAchievementCompleted -= Notify;
        }
    }

    private void Notify(Quest quest)
    {
        reserverdQuests.Enqueue(quest);
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
            StartCoroutine("ShowNotice");
        }
    }

    //클리어한 함수를 보여줌
    private IEnumerator ShowNotice()
    {
        var wawitSeconds = new WaitForSeconds(showTime);

        Quest quest;
        while(reserverdQuests.Count > 0)
        {
            quest = reserverdQuests.Dequeue();
            titleText.text = titleDescription.Replace("%{dn}", quest.DisplayName); //%는 mark, 이것을이용해서 퀘스트가 title이 문자열 어디에 출력될지 유동적으로가능 퀘스트로 어떤정보를보여줄때 가장일반적인 방식
            foreach(var reward in quest.Rewards)        //for문을 돌려서 +로 문자열을 합치는경우 성능에 굉장히 안좋아서 stringBuidler로 이용. 
            {
                stringBuilder.Append(reward.Description);
                stringBuilder.Append(" ");
                stringBuilder.Append(reward.Quantity);
                stringBuilder.Append(" ");
            }
            rewardText.text = stringBuilder.ToString();
            stringBuilder.Clear();

            yield return wawitSeconds;
        }
        gameObject.SetActive(false);
    }
}
