using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZooEvent : MonoBehaviour
{
    // Properties
    /// <summary>
    /// The description of the event.
    /// Used in the event panel.
    /// </summary>
    public string DescriptionText { get; set; }
    /// <summary>
    /// The text for option one.
    /// Used for the first option button.
    /// </summary>
    public string OptionOne { get; set; }
    /// <summary>
    /// The text for option two.
    /// Used for the second option button.
    /// </summary>
    public string OptionTwo { get; set; }
    /// <summary>
    /// The season in which the event occurs.
    /// Should match with the enum in Game Manager.
    /// </summary>
    public GameManager.SEASONS Season { get; set; }
    /// <summary>
    /// Has the event occurred?
    /// This will prevent an event from repeating, if we use random selection.
    /// </summary>
    public bool EventOccurred { get; set; }

    // Private Fields
    /// <summary>
    /// Types used for the values of result 1
    /// 0 for money, 1 for animals, 2 for happiness
    /// 3 for people/day, 4 for money/day, 5 for assessment
    /// </summary>
    List<int> resultOneTypes = new List<int>();
    /// <summary>
    /// Values used for result 1
    /// </summary>
    List<float> resultOneValues = new List<float>();
    /// <summary>
    /// Types used for the values of result 2
    /// 0 for money, 1 for animals, 2 for happiness
    /// 3 for people/day, 4 for money/day, 5 for assessment
    /// </summary>
    List<int> resultTwoTypes = new List<int>();
    /// <summary>
    /// Values used for result 2
    /// </summary>
    List<float> resultTwoValues = new List<float>();

    /// <summary>
    /// The action taken by the event.
    /// </summary>
    public void EventAction(int result)
    {
        GameManager gm = FindObjectOfType<GameManager>();
        int resultType = 0;
        if(result == 1)
        {
            for(int i = 0; i < resultOneValues.Count; i++)
            {
                if (resultOneTypes.Count < resultOneValues.Count)
                    resultType = resultOneTypes[0];
                else
                    resultType = resultOneTypes[i];
                switch(resultType)
                {
                    // Money event. If < 0, adds to daily costs, else adds to daily earnings
                    case 0:
                        if (resultOneValues[i] < 0f)
                            gm.DailyCosts += resultOneValues[i];
                        else
                            gm.DailyEarnings += resultOneValues[i];
                        break;
                    // Animal event. The result is the animal's index in the list in GameManager, which is set active by the event
                    case 1:
                        gm.animals[(int)resultOneValues[i]].CurrentEnclosure++;
                        break;
                    // Happiness event. The result is a whole number representing a percentage to add/subtract to the modifier
                    case 2:
                        gm.AnimalHappinessModifier += resultOneValues[i] / 100f;
                        break;
                    // People/day event. Modifies the base people per day by a specified percentage.
                    case 3:
                        gm.BasePeoplePerDay = Mathf.RoundToInt(gm.BasePeoplePerDay * (1f + resultOneValues[i] / 100f));
                        break;
                    // Money/person event. Modifies the base money per person by a specified percentage.
                    case 4:
                        gm.BaseSpentPerPerson *= (1f + resultOneValues[i] / 100f);
                        break;
                    // Special events for assessment
                    case 5:
                        if (resultOneValues[i] == 1)
                            gm.QuestionAnswered((int)Season - 1);
                        break;
                }
            }
        }
        else
        {
            for (int i = 0; i < resultTwoValues.Count; i++)
            {
                if (resultTwoTypes.Count < resultTwoValues.Count)
                    resultType = resultTwoTypes[0];
                else
                    resultType = resultTwoTypes[i];
                switch (resultType)
                {
                    // Money event. If < 0, adds to daily costs, else adds to daily earnings
                    case 0:
                        if (resultTwoValues[i] < 0f)
                            gm.DailyCosts += resultTwoValues[i];
                        else
                            gm.DailyEarnings += resultTwoValues[i];
                        break;
                    // Animal event. The event has no impact if result two is picked.
                    case 1:
                        break;
                    // Happiness event. The result is a whole number representing a percentage to add/subtract to the modifier
                    case 2:
                        gm.AnimalHappinessModifier += resultTwoValues[i] / 100f;
                        break;
                    // People/day event. Modifies the base people per day by a specified percentage.
                    case 3:
                        gm.BasePeoplePerDay = Mathf.RoundToInt(gm.BasePeoplePerDay * (1f + resultTwoValues[i] / 100f));
                        break;
                    // Money/person event. Modifies the base money per person by a specified percentage.
                    case 4:
                        gm.BaseSpentPerPerson *= (1f + resultTwoValues[i] / 100f);
                        break;
                    // Special events for assessment
                    case 5:
                        if (resultTwoValues[i] == 1)
                            gm.QuestionAnswered((int)Season - 1);
                        break;
                }
            }
        }
    }

    /// <summary>
    /// Add a value to one of the lists for results
    /// </summary>
    /// <param name="listIndex">Which list to add to. 0 for types 1, 1 for values 1, 2 for types 2, 3 for values 2</param>
    /// <param name="value">The value to add</param>
    public void AddToList(int listIndex, float value)
    {
        switch(listIndex)
        {
            case 0:
                resultOneTypes.Add((int)value);
                break;
            case 1:
                resultOneValues.Add(value);
                break;
            case 2:
                resultTwoTypes.Add((int)value);
                break;
            case 3:
                resultTwoValues.Add(value);
                break;
        }
    }
}
