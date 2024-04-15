using System;
using UnityEngine;
using Utilities;

namespace Gameplay
{
    public class GameStateManager : HiddenSingleton<GameStateManager>
    {
        public static event Action<GAME_STATE> OnGameStateChanged; 
        //============================================================================================================//

        private GAME_STATE _currentGameState;


        //============================================================================================================//

        public static void SetGameState(GAME_STATE gameState)
        {
            if (gameState == Instance._currentGameState)
                return;

            Instance._currentGameState = gameState;
            OnGameStateChanged?.Invoke(gameState);
        }
    }
}
