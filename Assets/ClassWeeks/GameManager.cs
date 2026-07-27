// using System;
// using UnityEngine;
//
// /// <summary>
// ///     Game space manager. Level transitions
// ///     play, pause, game over inputs
// /// </summary>
// ///public class Enums : MonoBehaviour
// ///{
//     private enum GameState
//     {
//         Start,
//         Playing,
//         Paused,
//         GameOver
//     }
//
//     // constant = value that never changes
//     private const int MaxPresses = 3;
//
//     //To store current game states
//     private GameState _currentgameState = GameState.Start;
//     private int _spaceBarPressCount;
//
//     // Start is called once before the first execution of Update after the MonoBehaviour is created
//     private void Start()
//     {
//         StartGame();
//     }
//
//     // Update is called once per frame
//     private void Update()
//     {
//         if (_currentgameState == GameState.Start)
//         {
//             BeginPlay();
//         }
//
//         if (_currentgameState == GameState.Playing)
//         {
//             CheckGameInput();
//         }
//
//         if (Input.GetKeyDown(KeyCode.Escape))
//         {
//             if (_currentgameState == GameState.Playing)
//             {
//                 PauseGame();
//             }
//
//             if (_currentgameState == GameState.Paused)
//             {
//                 ResumeGame();
//             }
//         }
//         
//         if (_currentgameState == GameState.GameOver) || _currentgameState == GameState.Playing)
//         {
//             RestartGame();
//         }
//
//         #region Methodes
//
//         private void StartGame()
//         {
//             _currentgameState = GameState.Start;
//             Debug.Log("Game Start");
//             Debug.Log("Press P to Start Playing");
//         }
//         
//         private void BeginPlay()
//         {
//             //Detect P pressed
//             if (Input.GetKeyDown(KeyCode.P))
//             {
//                 _currentgameState = GameState.Playing;
//                 _spaceBarPressCount = 0;
//
//                 Debug.Log("Game Is Playing");
//                 Debug.Log("Space 3 times for game over");
//                 Debug.Log("Press Enter to Pause");
//             }
//         }
//         
//         private void CheckGameInput()
//         {
//             // Detect space bar press
//             if (!Input.GetKeyDown(KeyCode.Space))
//             {
//                 return;
//             }
//
//             // Increment the press counter
//             _spaceBarPressCount++;
//             Debug.Log($"Space bar pressed! Count: {_spaceBarPressCount}/{MaxPresses}");
//
//             // Trigger game over when reaching maximun presses
//             if (_spaceBarPressCount >= MaxPresses)
//             {
//                 TriggerGameOver();
//             }
//         }
//
//         private void TriggerGameOver()
//         {
//             _currentgameState = GameState.GameOver;
//             Debug.Log($"You've pressed the space bar {_spaceBarPressCount} times!");
//             Debug.Log("Press R to Restart");
//             Time.timeScale = 0;
//         }
//
//         private void PauseGame()
//         {
//             _currentgameState = GameState.Paused;
//             //Freezes time entirely
//             Time.timeScale = 0f;
//             Debug.Log("Game Paused");
//             Debug.Log("Press ESC to Resume");
//         }
//
//         private void ResumeGame()
//         {
//             _currentgameState = GameState.Playing;
//             //Restore normal time flow
//             Time.timeScale = 1f;
//             Debug.Log("Game Resumed");
//         }
//
//         private void RestartGame()
//         {
//             if (Input.GetKeyDown(KeyCode.R) == false)
//             {
//                 return;
//             }
//
//             Debug.Log("Game Restart");
//
//             //Reset all counters
//             _spaceBarPressCount = 0;
//             StartGame();
//         }
//         
//         private void TriggerGameOver()
//         {
//             // Change state to GameOver
//             _currentgameState = GameState.GameOver;
//             Debug.Log("=== Game Over ===");
//             Debug.Log($"You pressed space {_spaceBarPressCount} times!");
//             Debug.Log("Press R to restart");
//         
//             // Freeze time during game over screen
//             // Time.timeScale = 0;
//         }
//
//         #endregion Methodes
//     }
// }