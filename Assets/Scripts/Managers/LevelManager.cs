using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vectrosity;


public class LevelManager : Singleton<LevelManager>
{
    [SerializeField] Button okButton;
    [SerializeField] GameObject environment;
    [SerializeField] GameObject preGameMessageScreen;

    public bool isLastLevel;


    private List<StarController> starsChosen;   // Keeps a record of the stars that were pressed.
    private VectorLine connectorLine;  // A continous line that uses the starsChosen list to connect the positions of the stars.

    public struct SolvingStats
    {
        public string timeStamp;
        public string studentName;
        public int level_num;
        public int playerMoves;
        public int errorsMade;
        public int hintsCount;

        public TimeSpan totalSolveTime;
        public TimeSpan instructReadingTime;


    }

    public SolvingStats stats;

    private float startLevelTime;
    private float startSolvingTime;

    //Keeps track on how many points were added to the line (line is created between 2 points).
    private int pointIndex = 0;  

    private void Start()
    {

        InitStats();
        startLevelTime = Time.time;

        GameManager.Instance.OnGameStateChanged.AddListener(HandleGameStateChanged);

        okButton.onClick.AddListener(HandleOkButtonPressed);

        starsChosen = new List<StarController>();

        connectorLine = new VectorLine("ConnectorLine", new List<Vector3>(), 1.0f, LineType.Continuous);
    }


    private void HandleGameStateChanged(GameManager.GameState currentState, GameManager.GameState previousState)
    {
        if (currentState == GameManager.GameState.POSTGAME)
            environment.gameObject.SetActive(false);
    }

    private void HandleOkButtonPressed()
    {
        stats.instructReadingTime = TimeSpan.FromSeconds(startLevelTime - Time.time);
        Debug.Log(stats.instructReadingTime.ToString("mm':'ss':'ff"));
        startSolvingTime = Time.time;

        environment.gameObject.SetActive(true);
        preGameMessageScreen.SetActive(false);
    }

    private void InitStats()
    {
        stats.timeStamp = System.DateTime.Now.ToString();
        stats.studentName = "Amit";
        stats.level_num = GameManager.Instance.currentLevelIndex;
        stats.playerMoves = 0;
        stats.errorsMade = 0;
        stats.hintsCount = 0;



    }

    // Called from a star that is a part of the shape.
    public void OnCorrectStarPressed(StarController currentStarPressed)
    {
        stats.playerMoves++;

        //In the case this is the first correct star that was chosen.
        if (pointIndex == 0)
        {
            starsChosen.Add(currentStarPressed);
            GameManager.Instance.CorrectStarChosen.Invoke(currentStarPressed.starIndex);
            AddPointToLine(currentStarPressed.transform.position);

        }
        else
        {
            var previousStar = starsChosen[pointIndex - 1];

            if (currentStarPressed.starIndex != previousStar.starIndex)
            {
                //Checks if the player pressed on the star that was chosen before the last (he wants to erase the last line).
                if (pointIndex > 1 && currentStarPressed.starIndex == starsChosen[pointIndex - 2].starIndex)
                {
                    GameManager.Instance.CorrectStarChosen.Invoke(starsChosen[pointIndex - 2].starIndex);
                    starsChosen.Remove(starsChosen[pointIndex - 1]);
                    RemovePointFromLine(previousStar.transform.position);

                }
                else
                {
                    // Checks if the star pressed is one of the correct possible stars (and wasn't chosen before).
                    if (currentStarPressed.starIndex == previousStar.nextStarIndex1 || currentStarPressed.starIndex == previousStar.nextStarIndex2)
                    {
                         GameManager.Instance.isGameRunning = ClosedShapeCheck(currentStarPressed.starIndex);

                        starsChosen.Add(currentStarPressed);
                        GameManager.Instance.CorrectStarChosen.Invoke(currentStarPressed.starIndex);
                        AddPointToLine(currentStarPressed.transform.position);

                    }
                    else
                    {
                        // The wrong star was chosen.
                        stats.errorsMade++;
                        GameManager.Instance.WrongStarChosen.Invoke(currentStarPressed.starIndex);
                    }
                }
            }
        }
    }
    public void WrongStarPressed()
    {
        stats.playerMoves++;
        stats.errorsMade++;
       // Debug.Log("Wrong star clicked");
    }

    public void HintPressed()
    {
        stats.hintsCount++;

        if (pointIndex > 0)
        {
            GameManager.Instance.HintRequested.Invoke(starsChosen[pointIndex-1].nextStarIndex1, starsChosen[pointIndex-1].nextStarIndex2);
        } else
        {
            GameManager.Instance.HintRequested.Invoke(0, 0);
        }
    }

    public bool ClosedShapeCheck(int lastStarIndexChosen)
    {
        foreach (StarController star in starsChosen)
        {
            if (star.starIndex == lastStarIndexChosen)
            {
                stats.totalSolveTime = TimeSpan.FromSeconds(startSolvingTime - Time.time);
               // Debug.Log(stats.totalSolveTime.ToString("mm':'ss':'ff"));


                StatsToCSV();
                GameManager.Instance.UpdateGameState(GameManager.GameState.POSTGAME);
               // Debug.Log("Shape Closed");
                return true;
            }
        }

        return false;
    }

    public void StatsToCSV()
    {
        string[] statsInStrings = new string[8] {
            stats.timeStamp,
            stats.studentName,
            stats.level_num.ToString(), 
            stats.playerMoves.ToString(), 
            stats.errorsMade.ToString(),
            stats.hintsCount.ToString(),
            stats.totalSolveTime.ToString("mm':'ss':'ff"),
            stats.instructReadingTime.ToString("mm':'ss':'ff")
        };

        CSVManager.AppendToReport(statsInStrings);
    }

    //Adds a point to the line being drawn on screen to create the constellation.
    private void AddPointToLine(Vector3 point)
    {

        connectorLine.points3.Add(point);   // Creates a line between the last star pressed and the one before it.
        connectorLine.Draw3D();

        pointIndex++;
    }

    //Removes a point from the line being drawn on screen to create the constellation.
    private void RemovePointFromLine(Vector3 point)
    {

        Debug.Log("This point has been removed: " + point);
        connectorLine.points3.Remove(point);   // Creates a line between the last star pressed and the one before it.
        connectorLine.Draw3D();

        pointIndex--;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        starsChosen.Clear();
        VectorLine.Destroy(ref connectorLine);
    }
}
