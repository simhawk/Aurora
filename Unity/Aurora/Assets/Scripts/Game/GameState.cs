public enum GameState 
{
    //Initialization
    MainMenu, // Starting Scene with a single button to start the game (maybe a little graphic)
    PlayerSync, // Each player synchronizes their physical location to the board game console by touching a quadrant
    CivSelect, // Each player selects a civilization they want to play with
    PlayerOrderSelect, // Wait until everyone has rolled the dice (three booleans are true on update)Each play rolls the dice to see who goes first (P1, P2, P3)
    InitialSettlementPlacement, // Go in PlayerOrder wait until player interacts and placed different components, then move on to next person and so on. if all complete move state
    InitialRoadPlacement,
    //Gameplay
    ResourceRoll, // Wait for active player to roll dice when rolled, dole out resources to different players 
    Trading, // Prompt user to trade with anybody confirm trade or decline with UI buttons 
    BuildOrDevelopmentCard, // Ask user to build or not user may also play a development card if they have them (Check win conditions, if no winner go to resource roll and switch active player)
    Victory
}