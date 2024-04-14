using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

namespace UI
{
    public enum GAME_UI_ACTION 
    {
        NONE,
        DRAW
    }

    public class GameplayUI : HiddenSingleton<GameplayUI>
    {
        [SerializeField]
        private Button drawButton;

        public static event Action<GAME_UI_ACTION> OnGameUIAction; 

        // Start is called before the first frame update
        void Start()
        {
            //drawButton.onClick.AddListener(OnDrawButtonPressed);
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        void InitUI()
        {
            // TODO -- perform default initialization here
        }

        private void OnDrawButtonPressed()
        {
            OnGameUIAction?.Invoke(GAME_UI_ACTION.DRAW);
        }
    }

}