using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour 
{
    /// <summary>
    /// The type of animal (lion, penguin, etc.)
    /// </summary>
    public string Species { get; private set; }
    /// <summary>
    /// The animal's happiness.
    /// Should be a value between 0 and 1 (percentage-based)
    /// </summary>
    public float Happiness { get; private set; }
    /// <summary>
    /// Cost of food per animal per day
    /// </summary>
    public float FoodCost { get; private set; }
    /// <summary>
    /// Minimum comfortable temperature for the animal
    /// </summary>
    public float MinTemp { get; private set; }
	/// <summary>
    /// Maximum comfortable temperature for the animal
    /// </summary>
    public float MaxTemp { get; private set; }
    /// <summary>
    /// Total number of animals on display
    /// </summary>
    public int NumberInExhibit { get; private set; }
    /// <summary>
    /// Is an animal injured?
    /// </summary>
    public bool IsInjured { get; private set; }

    //TODO Add a reference to the current enclosure in use for its stats/modifiers

    /// <summary>
    /// Make calculations for the day
    /// </summary>
    public void DailyRoutine()
    {
        UpdateHappiness();
        CheckInjury();
    }

    private void CheckInjury()
    {
        float random = Random.Range(0.01f, 1f);
        if (random < Happiness)
            IsInjured = true;
    }

    private void UpdateHappiness()
    {
        // TODO Implement enclosure happiness modifier
        if (!IsInjured)
            Happiness *= (.99f - (.01f * NumberInExhibit));
        else
            Happiness *= (.75f - (.01f * NumberInExhibit));
    }
}
