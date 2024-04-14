using System;
using System.Collections;
using System.Collections.Generic;
using GameInput;
using UI;
using Unity.VisualScripting;
using UnityEngine;

namespace Gameplay
{
    public class GameplayController : MonoBehaviour
    {
        [SerializeField]
        private PlayerController player;
        [SerializeField]
        private EnemyController enemy;


        //============================================================================================================//
        private void OnEnable()
        {
            GameStateManager.OnGameStateChanged += OnGameStateChanged;
            GameplayUI.OnGameUIAction += OnGameUIAction;
        }


        private void OnDisable()
        {
            GameStateManager.OnGameStateChanged -= OnGameStateChanged;
            GameplayUI.OnGameUIAction -= OnGameUIAction;
        }

        //============================================================================================================//

        
        // Start is called before the first frame update
        void Start()
        {
            StartGame();
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        private void StartGame()
        {
            //ScreenFader.ForceSetColorBlack();
            //LevelLoader.LoadFirstLevel();
            //StartCoroutine(LoadLevelCoroutine());
            StartCoroutine(LoadGameCoroutine());
        }

        private IEnumerator LoadGameCoroutine()
        {
            // Init decks
            player.Init();
            enemy.Init();

            // Start Round
            yield return StartCoroutine(DoRound());

            yield return null;
            
        }

        private IEnumerator DoRound()
        {
            // Start Round
            GameInputDelegator.LockInputs = true;

            // Draw and flip enemy card
            yield return enemy.PlayTopCard();

            // Draw player card selection
            yield return player.DrawCards(3);

            // Wait for player to pick
            GameInputDelegator.LockInputs = false;

            while(player.playedCard == null)
            {
                yield return null;
            }

            GameInputDelegator.LockInputs = true;

            Debug.Log($"Player selected choice {player.playedCard}");

            // Place player card
            yield return player.PlayCard(player.playedCard);

            // Flip both cards and determine winner
            // Move cards to winner's deck and assign resources
            // Start new round

            yield return null;

        }


        private static IEnumerator WaitCoroutine(float seconds, Action onCompleted)
        {
            yield return new WaitForSeconds(seconds);
            onCompleted?.Invoke();
        }
        

        private void OnGameStateChanged(GAME_STATE newGameState)
        {
            switch (newGameState)
            {
                case GAME_STATE.NONE:
                    return;
                case GAME_STATE.MENU:
                    //TODO Pause the game? Possibly caused by opening the settings menu
                    break;
                case GAME_STATE.GAME:
                    
                    break;
                case GAME_STATE.CINEMATIC:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(newGameState), newGameState, null);
            }
        }

        private void OnGameUIAction(GAME_UI_ACTION action)
        {
            if(action == GAME_UI_ACTION.DRAW)
                player.DrawCards(3);
                
        }

    }
}