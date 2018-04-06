using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalEvent : MonoBehaviour, IZooEvent
{
    public string DescriptionText { get; set; }
    public string OptionOne { get; set; }
    public string OptionTwo { get; set; }
    public GameManager.SEASONS Season { get; set; }
    public int ResultOne { get; set; }
    public int ResultTwo { get; set; }
    public bool EventOccurred { get; set; }
    /// <summary>
    /// The animal type offerred by the event
    /// 0 = Lion, 1 = Elephant, 2 = Giraffe, 3 = Polar Bear
    /// </summary>
    public int AnimalType { get; private set; }

    public AnimalEvent(int type)
    {
        AnimalType = type;
    }

    public void EventAction(int result)
    {
        if (result == 1)
            FindObjectOfType<GameManager>().animals[AnimalType].CurrentEnclosure++;
    }
}
