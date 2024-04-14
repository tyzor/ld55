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
        private PlayerController playerController;
        [SerializeField]
        private EnemyController enemyController;


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
            playerController.Init();
            enemyController.Init();

            // Start Round
            yield return StartCoroutine(DoRound());

            yield return null;
            
        }

        private IEnumerator DoRound()
        {
            // Start Round
            GameInputDelegator.LockInputs = true;

            // Check for win condition
            if(!enemyController.CanDraw())
            {
                yield return StartCoroutine(WinCoroutine());
                yield break;
            }
            
            // Check for lose condition
            if(!playerController.CanDraw())
            {
                yield return StartCoroutine(LoseCoroutine());
                yield break;
            }

            // Draw and flip enemy card
            yield return enemyController.PlayTopCard();

            // Draw player card selection
            yield return playerController.DrawCards();

            // Wait for player to pick
            GameInputDelegator.LockInputs = false;

            while(playerController.playedCard == null)
            {
                yield return null;
            }

            GameInputDelegator.LockInputs = true;

            Debug.Log($"Player selected choice {playerController.playedCard}");

            // Place player card
            yield return playerController.PlayCard(playerController.playedCard);

            // Flip both cards and determine winner
            yield return StartCoroutine(BattleCards());

            yield return new WaitForSeconds(0.3f);

            // Start new round
            StartCoroutine(DoRound());

        }


        private static IEnumerator WaitCoroutine(float seconds, Action onCompleted)
        {
            yield return new WaitForSeconds(seconds);
            onCompleted?.Invoke();
        }
        
        // Compare played cards and return winner
        private IEnumerator BattleCards() {
            
            int playerValue = playerController.playedCard.cardData.value;
            int enemyValue = enemyController.playedCard.cardData.value;

            if(playerValue == enemyValue)
            {
                // We have a tie here -- do special tie rules
                yield return playerController.BattleTied();
                yield return enemyController.BattleTied();
                yield break;   
            }

            // TODO -- add power custom logic here
            if(playerValue > enemyValue)
                yield return playerController.BattleCards(enemyController);
            else
                yield return enemyController.BattleCards(playerController);
    
        }

        private IEnumerator WinCoroutine()
        {
            // TODO -- win VFX
            Debug.Log("PLAYER WON!!!!");
            yield return new WaitForSeconds(.5f);
        }

        private IEnumerator LoseCoroutine()
        {
            // TODO -- win VFX
            Debug.Log("PLAYER LOST!!!!");
            // TODO -- show replay level
            yield return new WaitForSeconds(.5f);
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
            /*
            if(action == GAME_UI_ACTION.DRAW)
                playerController.DrawCards(3);
                |*/
                
        }

    }
}