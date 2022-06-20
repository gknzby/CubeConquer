using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace CubeConquer.Managers
{
    public enum GameState
    {
        Menu,
        Preparation,
        Shotted,
        Goal,
        Loose
    }

    public class GameManager : MonoBehaviour, IGameManager
    {
        #region IGameManager
        public string ManagerType { get { return "GameManager"; } }
        public void SendGameAction(GameAction gameAction)
        {
            switch (gameAction)
            {
                case GameAction.PauseGame:
                    StopGame();
                    break;
                case GameAction.Shot:
                    Shot();
                    break;
                case GameAction.Lost:
                    GameLost();
                    break;
                case GameAction.Win:
                    GameWin();
                    break;
                case GameAction.Restart:
                    RestartGame();
                    break;
                case GameAction.LoadLevel:
                    LoadLevel();
                    break;
                case GameAction.EndGame:
                    EndGame();
                    break;
                case GameAction.ResumeGame:
                    ResumeGame();
                    break;
                default:
                    break;
            }
        }

        private void EndGame()
        {
            StopGame();
            ManagerProvider.GetManager<IUIManager>().ShowMenu("EndGameMenu");
            PlayerPrefs.SetInt("Level", 0);
        }

        private void GameWin()
        {
            StopGame();
            ManagerProvider.GetManager<IUIManager>().ShowMenu("WinMenu");
        }

        private void GameLost()
        {
            StopGame();
            ManagerProvider.GetManager<IUIManager>().ShowMenu("LostMenu");
        }

        private void ResumeGame()
        {
            Time.timeScale = 1f;
            ManagerProvider.GetManager<IUIManager>().ShowMenu("HUD");
            ManagerProvider.GetManager<IInputManager>().StartSendingInputs();
        }

        private void StopGame()
        {
            Time.timeScale = 0f;
            ManagerProvider.GetManager<IUIManager>().HideMenu("HUD");
            ManagerProvider.GetManager<IInputManager>().StopSendingInputs();
        }

        private void RestartGame()
        {
            ManagerProvider.GetManager<IUIManager>().ShowMenu("MainMenu");
        }
        #endregion

        #region Class Functions
        private void Shot()
        {
            ManagerProvider.GetManager<IInputManager>().StopSendingInputs();
        }

        private void LoadLevel()
        {
            PlayerPrefs.Save();

            int level = PlayerPrefs.GetInt("Level");

            ILevelManager levelManager = ManagerProvider.GetManager<ILevelManager>();
            if (levelManager.LoadLevel(level))
            {
                ResumeGame();
            }
            else
            {
                SendGameAction(GameAction.EndGame);
            }
        }
        #endregion

        #region Unity Functions => Awake, Start, OnDestroy
        private void Awake()
        {
            ManagerProvider.AddManager<IGameManager>(this);
        }

        private void Start()
        {
            //Works after all awake functions and start functions
            StartCoroutine(AfterLoad());
        }
        private IEnumerator AfterLoad()
        {
            yield return null; //Waiting first update functions
            ManagerProvider.GetManager<IUIManager>().ShowMenu("MainMenu");
        }


        private void OnDestroy()
        {
            ManagerProvider.RemoveManager<IGameManager>();
        }
        #endregion

    }
}