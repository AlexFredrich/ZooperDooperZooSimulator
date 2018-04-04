using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyEvent : MonoBehaviour, IZooEvent
{
    public string DescriptionText { get; set; }
    public string OptionOne { get; set; }
    public string OptionTwo { get; set; }
    public GameManager.SEASONS Season { get; set; }
    public int ResultOne { get; set; }
    public int ResultTwo { get; set; }
    public bool EventOccurred { get; set; }

    public void EventAction(int result)
    {
        throw new System.NotImplementedException();
    }
}
