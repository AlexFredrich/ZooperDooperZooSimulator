using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
    
    // Private Fields
    /// <summary>
    /// Current day (should be between 1 and 28)
    /// </summary>
    int currentDay = 1;
    /// <summary>
    /// List of all possible events
    /// </summary>
    List<IZooEvent> possibleEvents;

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
    /*float OverallSatisfaction
    {
        get
        {
            // TODO find and return the average happiness of all active animals
            
            float result = 0f;
            foreach (Animal a in animals.Where<Animal.CurrentEnclosure > 0>)
                result += a.Happiness;
            return result/animals.
        }
    }*/

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
}
