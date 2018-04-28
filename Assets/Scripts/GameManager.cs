using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    /// Amount each person spends in a day
    /// </summary>
    float baseSpentPerPerson = 5f;
    float spentPerPerson;
    /// <summary>
    /// The active enclosure
    /// </summary>
    int activeAnimal;
    /// <summary>
    /// Current day (should be between 1 and 28)
    /// </summary>
    int currentDay = 1;
    /// <summary>
    /// Number of people visiting in a day
    /// </summary>
    int basePeoplePerDay = 250;
    int peoplePerDay;
    /// <summary>
    /// List of all possible events
    /// </summary>
    List<IZooEvent> possibleEvents = new List<IZooEvent>();
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
    public float money = 10000;
    /// <summary>
    /// List of animals in the game
    /// </summary>
    [Tooltip("Instance of each animal. Must start Lion, Elephant, Giraffe, Polar Bear.")]
    [SerializeField]
    public List<Animal> animals;
    /// <summary>
    /// Text file containing the events
    /// </summary>
    [Tooltip("Text file to define events.")]
    [SerializeField]
    TextAsset eventFile;

    // Properties
    /// <summary>
    /// Calculates the average of every animal's satisfaction
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
    //UI Messages
    [SerializeField]
    Text eventSituation, buttonOptionOne, buttonOptionTwo, dayText, statusSummary, enclosureText, animalEnclosureText, endText, reportOverallText, seasonalAnimalText;
    [SerializeField]
    GameObject  endDayButtonVisible;
    [SerializeField]
    Button optionOneButton, optionTwoButton, vetButton, repairButton, upgradeButton, endDayButton;
    [SerializeField]
    GameObject eventPanel, enclosurePanel, instructionPanel, exitPanel, endPanel, reportPanel, seasonalAnimalPanel;
    [SerializeField]
    int upgradeCost = 1500, vetCost = 2000, repairCost = 1000;


    private bool dayEnd;
    private bool eventEnd;
    private int seasonModifier;
    private int dayModifier;
    private float dailyCosts;

    private int eventNumber = 0;

    private int weatherAffect;

    [SerializeField]
    private int overallMaintenanceCost = 3000;
    [SerializeField]
    private int employeeCosts = 2000;
    //Will be replaced when temp and weather are incorporated
    [SerializeField]
    private int utilityCosts;

    private float governmentGrant = 2000;

    private float dailyEarnings;
	// Use this for initialization
	void Start ()
    {        
        
        // TODO Start Game Loop	
        StartCoroutine(GameLoop());
        
    }


    void InitializeEvents()
    {
        // Split the file up by line
        var eventContent = eventFile.text.Split('\n');
        // Run through each line individually
        foreach (string s in eventContent)
        {
            // Declare variables to use for results
            int season = -1, resultOne = -1, resultTwo = -1, eventType = -1, animal = -1, tem = -1;
            Debug.Log(s);
            // Split the line by tabs (so I can use commas in the event descriptions)
            var individualEventContent = s.Split('\t');
            // Declare an event of some variety so it doesn't freak out over the next part
            IZooEvent tempEvent = null;
            // Determine what type of event it is
            // 0 - Money Event
            // 1 - Animal Event
            if (int.TryParse(individualEventContent[0], out tem))
                eventType = tem;
            tem = -1;
            // Initialize an event of the proper type
            switch(eventType)
            {
                // Initialize a Money Event
                case 0:
                    tempEvent = new MoneyEvent();
                    break;
                // Initialize an Animal Event with animal type
                case 1:
                    if (int.TryParse(individualEventContent[7], out tem))
                        animal = tem;
                    tempEvent = new AnimalEvent(animal);
                    tem = -1;
                    break;
                default:
                    break;
            }
            // Separate out description text
            tempEvent.DescriptionText = individualEventContent[1];
            // And option 1
            tempEvent.OptionOne = individualEventContent[2];
            // ...and option 2
            tempEvent.OptionTwo = individualEventContent[3];
            // Get the season of the event
            if (int.TryParse(individualEventContent[4], out tem))
                season = tem;
            tempEvent.Season = (SEASONS)season;
            tem = -1;
            // Get the first result
            if (int.TryParse(individualEventContent[5], out tem))
                resultOne = tem;
            tempEvent.ResultOne = resultOne;
            tem = -1;
            // ...and the second
            if (int.TryParse(individualEventContent[6], out tem))
                resultTwo = tem;
            tempEvent.ResultTwo = resultTwo;
            tem = -1;
            possibleEvents.Add(tempEvent);
        }
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
     private IEnumerator GameLoop()
    {
        foreach (Animal a in animals)
        {
            a.DailyRoutine();
        }
        InitializeEvents();
        
        for(SEASONS i = 0; i <= SEASONS.WINTER; i++)
        {
            dailyCosts = 0;
            dailyEarnings = 0;
            animals[(int)i + 4].CurrentEnclosure = Animal.ENCLOSURE.Bronze;
            seasonalAnimalPanel.SetActive(true);
            seasonalAnimalText.text = animals[(int)i + 4].Species + "s";
            statusSummary.text = "Current Money: " + money + "\nMoney Spent: " + dailyCosts + "\nMoney Earned: " + dailyEarnings + "\nOverall Animal Happiness: " + OverallSatisfaction;

            if (i == SEASONS.SUMMER)
                seasonModifier = 3;
            else if(i == SEASONS.SPRING || i == SEASONS.FALL)
                seasonModifier = 2;  
            else
                seasonModifier = 1;

            if (OverallSatisfaction >= .75)
                dailyEarnings += governmentGrant;
            else if (OverallSatisfaction >= .5 && OverallSatisfaction <= .75)
                dailyEarnings += governmentGrant / 2;
            else
                dailyEarnings += 0;

            for(DAYS d = 0; d <= DAYS.Sunday; d++)
            {
                if (money > 0)
                {
                    if (d > 0)
                    {
                        dailyCosts = 0;
                        dailyEarnings = 0;
                    }
                    WeatherChooser(i);
                    eventPanel.SetActive(true);
                    enclosurePanel.SetActive(false);
                    endDayButtonVisible.SetActive(false);
                    dayText.text = d.ToString() + " Day: " + currentDay;
                    eventSituation.text = possibleEvents[eventNumber].DescriptionText;
                    buttonOptionOne.text = possibleEvents[eventNumber].OptionOne;
                    buttonOptionTwo.text = possibleEvents[eventNumber].OptionTwo;
                    while (!eventEnd)
                        yield return null;
                    eventNumber++;
                    eventEnd = false;
                    // TODO weekends and people count and spent calculations
                    if (d == DAYS.Saturday || d == DAYS.Sunday)
                    {
                        eventPanel.SetActive(true);
                        eventSituation.text = possibleEvents[eventNumber].DescriptionText;
                        buttonOptionOne.text = possibleEvents[eventNumber].OptionOne;
                        buttonOptionTwo.text = possibleEvents[eventNumber].OptionTwo;

                        while (!eventEnd)
                            yield return null;
                        eventNumber++;
                    }
                    eventEnd = false;
                    endDayButtonVisible.SetActive(true);
                    eventPanel.SetActive(false);
                    if (d == DAYS.Saturday || d == DAYS.Sunday || d == DAYS.Friday)
                    {
                        dayModifier = 4;
                    }
                    else if (d == DAYS.Monday || d == DAYS.Tuesday || d == DAYS.Wednesday || d == DAYS.Thursday)
                    {
                        dayModifier = 2;
                    }
                    peoplePerDay = basePeoplePerDay * dayModifier * seasonModifier;
                    spentPerPerson = baseSpentPerPerson * seasonModifier;


                    while (!dayEnd)
                        yield return null;
                    CalculateMoney();
                    Summary();
                    currentDay++;
                    dayEnd = false;
                }
                else
                {
                    continue;
                }
            }

          
        }

        //End Panel
        endPanel.SetActive(true);
        if(money > 0)
        {
            endText.text = "Sucess!";
        }
        else
        {
            endText.text = "Failure";
        }

    }

    void WeatherChooser(SEASONS s)
    {

        if(s == SEASONS.SPRING || s == SEASONS.SUMMER || s == SEASONS.FALL)
        {
            int r = Random.Range(0, 2);
            
        }
        else
        {
            int r = Random.Range(0, 3);

        }

    }


    // TODO Event Response button function
    /*
     *  This function is used for both of the event response buttons and takes an int result
     *  This result should be 1 or 2 and be passed to the active event's EventAction function
     *  Then set eventEnd or whatever to true and the game loop will continue
     */ 
     public void EventResponse(int choice)
    {
        possibleEvents[eventNumber].EventAction(choice);
        //TODO response, hide event panel

        eventEnd = true;
    }

    public void Enclosure(int animal)
    {
        repairButton.interactable = false;
        vetButton.interactable = false;
        upgradeButton.interactable = false;
        activeAnimal = animal;
        enclosurePanel.SetActive(true);
        enclosureText.text = "Enclosure: " + animals[activeAnimal].Species + " Animal Happiness: " + animals[activeAnimal].Happiness + " Animal Health: Healthy.";

        

        if (animals[activeAnimal].IsInjured == true)
        {
            
            enclosureText.text = "Enclosure: " + animals[activeAnimal].Species + " Animal Happiness: " + animals[activeAnimal].Happiness + " Animal Health: In need of vet.";
            // TODO move money condition in here
            if (money >= vetCost)
            {
                vetButton.interactable = true;
                
                // TODO remove these on back button
                vetButton.onClick.AddListener(delegate { Vet(activeAnimal); });
            }
        }
        if(animals[activeAnimal].NeedsRepair == true && money >= repairCost)
        {
            repairButton.interactable = true;
            
            repairButton.onClick.AddListener(delegate { Repair(activeAnimal); });
        }
        if(animals[activeAnimal].CurrentEnclosure < Animal.ENCLOSURE.Gold && money >= upgradeCost)
        {
            upgradeButton.interactable = true;
            
            upgradeButton.onClick.AddListener(delegate { Upgrade(activeAnimal); });
        }

        

    }

    public void ExitEnclosurePanel()
    {
        enclosurePanel.SetActive(false);
        vetButton.onClick.RemoveListener(delegate { Vet(activeAnimal); });
        repairButton.onClick.RemoveListener(delegate { Vet(activeAnimal); });
        upgradeButton.onClick.RemoveListener(delegate { Vet(activeAnimal); });
    }
   

    // TODO Upgrade enclosure button function
    // Add the upgrade cost to the daily cost and increase the value of the enum in animal by 1
    // Should not be available if the enclosure is already gold
    public void Upgrade(int animal)
    {
        
        dailyCosts += upgradeCost;
        animals[animal].CurrentEnclosure++;
        upgradeButton.interactable = false;

    }


    // TODO Repair enclosure button function
    // Add the cost to the daily cost and set the needs repair bool in animal to true
    public void Repair(int animal)
    {
        dailyCosts += repairCost;
        animals[animal].NeedsRepair = false;
        repairButton.interactable = false;
    }


    // TODO Vet button function
    // Add the cost to the daily cost and set the injured bool in animal to true
    public void Vet(int animal)
    {
        dailyCosts += vetCost;
        animals[animal].IsInjured = false;
        vetButton.interactable = false;
    }


    private void CalculateMoney()
    {
        foreach (Animal a in animals)
        {
            dailyCosts += a.TotalFoodCost;
        }

        dailyCosts += employeeCosts + overallMaintenanceCost + utilityCosts;
        dailyEarnings += (peoplePerDay * spentPerPerson);
        money += dailyEarnings;
        money -= dailyCosts;
        
    }

    // TODO Summary function
    // Total up the daily costs and display them
    // Calculate the visitors for the day and how much they spent
    // Run each animal's daily routine
    private void Summary()
    {
        
        foreach (Animal a in animals)
        {
            a.DailyRoutine();
            

        }
        statusSummary.text = "Current Money: " + money + "\nMoney Spent: " + dailyCosts + "\nMoney Earned: " + dailyEarnings + "\nOverall Animal Happiness: " + OverallSatisfaction;
        
    }


    // TODO end day button
    // Tell the game loop to continue
    public void EndDay()
    {
        dayEnd = true;
    }


    public void Instructions()
    {
        instructionPanel.SetActive(true);
    }

    public void Continue()
    {
        instructionPanel.SetActive(false);
        exitPanel.SetActive(false);
        
    }

    public void Quit()
    {
        exitPanel.SetActive(true);
    }

    public void ExitToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void MoveToResults()
    {
        reportPanel.SetActive(true);
        reportOverallText.text = "The End Results";
    }

    public void SeasonalContinue()
    {
        seasonalAnimalPanel.SetActive(false);
    }
}
