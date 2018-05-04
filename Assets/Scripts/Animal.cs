using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Animal : MonoBehaviour 
{
    // Properties
    /// <summary>
    /// Is an animal injured?
    /// </summary>
    public bool IsInjured { get; set; }
    /// <summary>
    /// Does the enclosure need repairs?
    /// </summary>
    public bool NeedsRepair { get; set; }
    public ENCLOSURE CurrentEnclosure
    {
        get
        {
            return currentEnclosure_UseProperty;
        }
        set
        {
            // If the value is within the range of the enum, assign the variable
            if ((int)value < (System.Enum.GetNames(typeof(ENCLOSURE))).Length)
            {
                currentEnclosure_UseProperty = value;
                UpdateEnclosure();
            }
        }
    }
    /// <summary>
    /// The animal's happiness.
    /// Should be a value between 0 and 1 (percentage-based)
    /// </summary>
    public float Happiness
    {
        get
        {
            return happiness_UseProperty;
        }
        private set
        {
            // Ensure that happiness is between 0 and 1, inclusive
            happiness_UseProperty = Mathf.Clamp(value, 0f, 1f);
        }
    }
    /// <summary>
    /// Total cost of food per day
    /// </summary>
    public float TotalFoodCost { get { return foodCost_UseProperty * NumberInExhibit; } }
    /// <summary>
    /// Total number of animals on display
    /// </summary>
    public int NumberInExhibit
    {
        get
        {
            return numberInExhibit_UseProperty;
        }
        private set
        {
            numberInExhibit_UseProperty = value;
        }
    }
    /// <summary>
    /// The type of animal (lion, penguin, etc.)
    /// </summary>
    public string Species { get { return species_UseProperty; } }
    /// <summary>
    /// The calculation for animal enclosure maintenance
    /// </summary>
    public float MaintenanceCost
    {
        get
        {
            return maintenanceCost_UseProperty;
        }
        set
        {
            maintenanceCost_UseProperty = value;
        }
    }

    // SerializeFields
    [Tooltip("The cost of food per animal per day.")]
    [SerializeField]
    float foodCost_UseProperty;
    [Tooltip("The number of animals on display.")]
    [SerializeField]
    int numberInExhibit_UseProperty;
    [Tooltip("The type of animal (lion, penguin, etc.)")]
    [SerializeField]
    string species_UseProperty;
    [Tooltip("Happiness modifiers for each enclosure level (starting at none)")]
    [SerializeField]
    List<float> enclosureHappinessModifiers;
    [Tooltip("Breakdown chance for each enclosure level (starting at none)")]
    [SerializeField]
    List<float> enclosureBreakChance;
    [Tooltip("The general cost of maintence for this animal's enclosure")]
    [SerializeField]
    float maintenanceCost_UseProperty;
    [Tooltip("The temperature ranges where animals need extra heating or air conditioning(first is low temp, second is high temp)")]
    [SerializeField]
    List<int> temperatureRange;
    [Tooltip("The button attached to the enclosure.")]
    [SerializeField]
    Button button;

    // Fields
    /// <summary>
    /// The possible tiers of enclosures
    /// </summary>
    public enum ENCLOSURE { None, Bronze, Silver, Gold };
    /// <summary>
    /// The current tier of enclosure
    /// </summary>
    ENCLOSURE currentEnclosure_UseProperty = 0;
    /// <summary>
    /// Happiness
    /// </summary>
    private float happiness_UseProperty;

    void Start()
    {
        Happiness = 1f;
    }

    /// <summary>
    /// Make calculations for the day
    /// </summary>
    public void DailyRoutine()
    {
        UpdateEnclosure();
        if (currentEnclosure_UseProperty != ENCLOSURE.None)
        {
            
            UpdateHappiness();
            CheckInjury();
            CheckMalfunction();
        }
    }

    private void CheckInjury()
    {
        if (!IsInjured)
        {
            float random = Random.Range(0.01f, 1f);
            if (random > Happiness)
                IsInjured = true;
        }
    }

    public void UpdateMaintenanceCost(int temperature)
    {
        if(temperature < temperatureRange[0] && temperature > temperatureRange[1])
        {
            MaintenanceCost += 100;
        }

    }

    private void UpdateHappiness()
    {
        // If there isn't a problem, use the happiness modifier
        if (!IsInjured && !NeedsRepair)
            Happiness *= enclosureHappinessModifiers[(int)CurrentEnclosure] * FindObjectOfType<GameManager>().AnimalHappinessModifier;
        // If there's an injury, cut that by 25% (generously)
        else if (IsInjured && !NeedsRepair)
            Happiness *= .75f * enclosureHappinessModifiers[(int)CurrentEnclosure] * FindObjectOfType<GameManager>().AnimalHappinessModifier;
        // If the enclosure is broken, ditch the happiness modifier
        if (NeedsRepair)
            Happiness *= .75f * FindObjectOfType<GameManager>().AnimalHappinessModifier;
    }

    private void CheckMalfunction()
    {
        if(!NeedsRepair)
        {
            float random = Random.Range(.01f, 1f);
            if (random > enclosureBreakChance[(int)CurrentEnclosure])
                NeedsRepair = true;
        }
    }

    // TODO Implement function to enable/disable (if value is 0)
    // and change texture of enclosure in-game based on the value of the enum
    private void UpdateEnclosure()
    {
        switch(CurrentEnclosure)
        {
            case ENCLOSURE.None:
                {
                    MeshRenderer[] renderers = gameObject.GetComponentsInChildren<MeshRenderer>();
                    foreach (MeshRenderer r in renderers)
                        r.enabled = false;
                    button.interactable = false;
                    // TODO reset texture
                    break;
                }
            case ENCLOSURE.Bronze:
                {
                    MeshRenderer[] renderers = gameObject.GetComponentsInChildren<MeshRenderer>();
                    foreach (MeshRenderer r in renderers)
                        r.enabled = true;
                    button.interactable = true;
                    // TODO set bronze texture
                    break;
                }
            case ENCLOSURE.Silver:
                {
                    MeshRenderer[] renderers = gameObject.GetComponentsInChildren<MeshRenderer>();
                    foreach (MeshRenderer r in renderers)
                        r.enabled = true;
                    button.interactable = true;
                    // TODO set bronze texture
                    break;
                }
            case ENCLOSURE.Gold:
                {
                    MeshRenderer[] renderers = gameObject.GetComponentsInChildren<MeshRenderer>();
                    foreach (MeshRenderer r in renderers)
                        r.enabled = true;
                    button.interactable = true;
                    // TODO set bronze texture
                    break;
                }
        }
    }
}
