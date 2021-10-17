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

    public struct Stats
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

    public Stats gameStats;

    private float startLevelTime;
    private float startSolvingTime;
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
        gameStats.instructReadingTime = TimeSpan.FromSeconds(startLevelTime - Time.time);
        Debug.Log(gameStats.instructReadingTime.ToString("mm':'ss':'ff"));
        startSolvingTime = Time.time;

        environment.gameObject.SetActive(true);
        preGameMessageScreen.SetActive(false);
    }

    private void InitStats()
    {
        gameStats.timeStamp = System.DateTime.Now.ToString();
        gameStats.studentName = "Amit";
        gameStats.level_num = GameManager.Instance.currentLevelIndex;
        gameStats.playerMoves = 0;
        gameStats.errorsMade = 0;
        gameStats.hintsCount = 0;



    }

    public void StarPressed(StarController star)
    {
        Vector3 currentStarPosition = star.transform.position;


        if (pointIndex == 0)
        {
            starsChosen.Add(star);
            gameStats.playerMoves++;
            GameManager.Instance.CorrectStarChosen.Invoke(star.starIndex);
            AddPointToLine(currentStarPosition);

        }
        else
        {
            Vector3 previousStarPosition = starsChosen[pointIndex - 1].transform.position;

            int answerIndex1 = starsChosen[pointIndex - 1].nextStarIndex1;
            int answerIndex2 = starsChosen[pointIndex - 1].nextStarIndex2;

            if (currentStarPosition != previousStarPosition)
            {
                if (pointIndex > 1 && currentStarPosition == starsChosen[pointIndex - 2].transform.position)
                {
                    gameStats.playerMoves++;
                    GameManager.Instance.CorrectStarChosen.Invoke(starsChosen[pointIndex - 2].starIndex);
                    starsChosen.Remove(starsChosen[pointIndex - 1]);
                    RemovePointFromLine(previousStarPosition);

                }
                else
                {

                    if (star.starIndex == answerIndex1 || star.starIndex == answerIndex2)
                    {
                        gameStats.playerMoves++;
                        // GameManager.Instance.isGameRunning = ClosedShapeCheck(star.starIndex);

                        starsChosen.Add(star);
                        GameManager.Instance.CorrectStarChosen.Invoke(star.starIndex);
                        AddPointToLine(currentStarPosition);

                    }
                    else
                    {
                        gameStats.playerMoves++;
                        gameStats.errorsMade++;
                        GameManager.Instance.WrongStarChosen.Invoke(star.starIndex);
                    }


                }
            }
        }

    }

    public void WrongStarPressed()
    {
        gameStats.playerMoves++;
        gameStats.errorsMade++;
        Debug.Log("Wrong star clicked");
    }

    public void HintPressed()
    {
        gameStats.hintsCount++;

        if (pointIndex > 0)
        {
            GameManager.Instance.HintRequested.Invoke(starsChosen[pointIndex-1].nextStarIndex1, starsChosen[pointIndex-1].nextStarIndex2);
        } else
        {
            GameManager.Instance.HintRequested.Invoke(0, 0);
        }
    }

    public bool ClosedShapeCheck()
    {
        foreach (StarController star in starsChosen)
        {
            if (star.starIndex == starsChosen[pointIndex - 1].starIndex)
            {
                gameStats.totalSolveTime = TimeSpan.FromSeconds(startSolvingTime - Time.time);
                Debug.Log(gameStats.totalSolveTime.ToString("mm':'ss':'ff"));


                StatsToCSV();
                GameManager.Instance.UpdateGameState(GameManager.GameState.POSTGAME);
                Debug.Log("Shape Closed");
                return true;
            }
        }

        return false;
    }

    public void StatsToCSV()
    {
        string[] statsInStrings = new string[8] {
            gameStats.timeStamp,
            gameStats.studentName,
            gameStats.level_num.ToString(), 
            gameStats.playerMoves.ToString(), 
            gameStats.errorsMade.ToString(),
            gameStats.hintsCount.ToString(),
            gameStats.totalSolveTime.ToString("mm':'ss':'ff"),
            gameStats.instructReadingTime.ToString("mm':'ss':'ff")
        };

        CSVManager.AppendToReport(statsInStrings);
    }

    private void AddPointToLine(Vector3 point)
    {

        connectorLine.points3.Add(point);   // Creates a line between the last star pressed and the one before it.
        connectorLine.Draw3D();

        pointIndex++;
    }

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
