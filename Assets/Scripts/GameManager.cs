using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour 
{
    // Public Fields
    /// <summary>
    /// The days of the week, used to determine weekends and display current day
    /// </summary>
    public enum DAYS { Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday };    
    /// <summary>
    /// The seasons, used for events and the like
    /// </summary>
    public enum SEASONS { SPRING, SUMMER, FALL, WINTER };
    /// <summary>
    /// The weather, used to impact the temperature
    /// </summary>
    public enum WEATHER { Sunny, Rainy, Windy, Snowy };

    // Private Fields
    /// <summary>
    /// Current day (should be between 1 and 28)
    /// </summary>
    int currentDay = 1;
    /// <summary>
    /// List of all possible events
    /// </summary>
    List<IZooEvent> possibleEvents;
    /// <summary>
    /// String to display for summary of injuries
    /// </summary>
    string injuryList = "No injuries.";
    /// <summary>
    /// String to display for summary of repairs
    /// </summary>
    string repairList = "No repairs needed.";

    // SerializeFields
    /// <summary>
    /// Player's money
    /// </summary>
    [Tooltip("Player's money.")]
    [SerializeField]
    float money;
    /// <summary>
    /// List of animals in the game
    /// </summary>
    [Tooltip("Instance of each animal.")]
    [SerializeField]
    List<Animal> animals;

    // Properties
    /// <summary>
    /// Calculates the average of every animal's 
    /// </summary>
    float OverallSatisfaction
    {
        get
        {
            float result = 0f;
            int activeAnimals = 0;
            foreach (Animal a in animals)
            {
                result += a.Happiness;
                if (a.CurrentEnclosure != Animal.ENCLOSURE.None)
                    activeAnimals++;
            }
            return result / activeAnimals;
        }
    }

	// Use this for initialization
	void Start ()
    {
        // TODO Start Game Loop	
    }

    // Update is called once per frame
    void Update () 
	{
	}

    // TODO Write Game Loop
    /*
     *  Game loop coroutine should work similarly to the Pisces game loop
     *  Initialize anything that needs initializing (such as reading in the event list)
     *  then start the game proper by making a nested for loop with the seasons and then
     *  the days of the week. Wait for a response from the "end day" button.
     *  Send to an end state after the last day.
     *  setupEvent()
     *  while(!eventEnd)
     *      yield return null;
     *  eventEnd = false
     *  if(weekend)
     *      setupEvent
     *      while(!eventEnd)
     *          yield return null
     *      eventEnd = false
     *  summary panel and end turn crap
     */

    // TODO Event Response button function
    /*
     *  This function is used for both of the event response buttons and takes an int result
     *  This result should be 1 or 2 and be passed to the active event's EventAction function
     *  Then set eventEnd or whatever to true and the game loop will continue
     */ 

    // TODO Upgrade enclosure button function
    // Add the upgrade cost to the daily cost and increase the value of the enum in animal by 1
    // Should not be available if the enclosure is already gold

    // TODO Repair enclosure button function
    // Add the cost to the daily cost and set the needs repair bool in animal to true

    // TODO Vet button function
    // Add the cost to the daily cost and set the injured bool in animal to true

    // TODO Summary function
    // Total up the daily costs and display them
    // Calculate the visitors for the day and how much they spent
    // Run each animal's daily routine
    
    // TODO end day button
    // Tell the game loop to continue
}
