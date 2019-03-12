public enum GameState 
{
    //Initialization
    MainMenu, // Starting Scene with a single button to start the game (maybe a little graphic)
    InitialSettlementPlacement, // Go in PlayerOrder wait until player interacts and placed different components, then move on to next person and so on. if all complete move state
    InitialRoadPlacement,
    
    //Gameplay
    ResourceRoll, // Wait for active player to roll dice when rolled, dole out resources to different players 
    ResourceRollDone, // after button clicked, perform the resource doling
    PlaceThief, // if rolled a 7, this state is entered
    PlaceThiefDone, // finished placing the thief, actually do stuff
    Trading, // Prompt user to trade with anybody confirm trade or decline with UI buttons 
    BuildOrDevelopmentCard, // Ask user to build or not user may also play a development card if they have them (Check win conditions, if no winner go to resource roll and switch active player)
    BuildingSelected,
    Victory
}