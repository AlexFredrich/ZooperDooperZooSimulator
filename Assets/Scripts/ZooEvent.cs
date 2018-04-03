using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IZooEvent
{
    /// <summary>
    /// The description of the event.
    /// Used in the event panel.
    /// </summary>
    string DescriptionText { get; set; }
    /// <summary>
    /// The text for option one.
    /// Used for the first option button.
    /// </summary>
    string OptionOne { get; set; }
    /// <summary>
    /// The text for option two.
    /// Used for the second option button.
    /// </summary>
    string OptionTwo { get; set; }
    /// <summary>
    /// The season in which the event occurs.
    /// Should match with the enum in Game Manager.
    /// </summary>
    int Season { get; set; }
    /// <summary>
    /// The benefit/deficit of result one.
    /// This will be added to whatever value is determined in the child classes.
    /// </summary>
    int ResultOne { get; set; }
    /// <summary>
    /// The benefit/deficit of result two.
    /// This will be added to whatever value is determined in the child classes.
    /// </summary>
    int ResultTwo { get; set; }
    /// <summary>
    /// Has the event occurred?
    /// This will prevent an event from repeating, if we use random selection.
    /// </summary>
    bool EventOccurred { get; set; }

    /// <summary>
    /// The action taken by the event.
    /// Every event will have one, but each class will implement it differently.
    /// </summary>
    void EventAction();
}
