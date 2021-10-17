using UnityEngine.Events;

public class Events
{
    [System.Serializable] public class CorrectStarChosen : UnityEvent<int> { }
    [System.Serializable] public class WrongStarChosen : UnityEvent<int> { }
    [System.Serializable] public class HintRequested : UnityEvent<int,int> { }
    //  [System.Serializable] public class GameStarted : UnityEvent { }

    [System.Serializable] public class EventGameState : UnityEvent<GameManager.GameState, GameManager.GameState> { }
}
