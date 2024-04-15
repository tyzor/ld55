using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
        [SerializeField, Header("Option UI")]
        private GameObject optionUI;
        [SerializeField]
        private Button displayButton;
        [SerializeField]
        private TextMeshProUGUI displayButtonText;
        [SerializeField]
        private TextMeshProUGUI displayText;

        public static event Action<GAME_UI_ACTION> OnGameUIAction; 

        // Start is called before the first frame update
        private void Start()
        {
            InitUI();
        }
        
        void InitUI()
        {
            // TODO -- perform default initialization here
            displayText.text = string.Empty;
            displayButtonText.text = string.Empty;
            optionUI.SetActive(false);
        }

        private void OnDrawButtonPressed()
        {
            OnGameUIAction?.Invoke(GAME_UI_ACTION.DRAW);
        }

        public static void DisplayOptionWindow(string displayText, string optionText, Action onButtonPressed)
        {
            Instance?.TryDisplayOptionWindow(displayText,optionText, onButtonPressed);
        }

        private void TryDisplayOptionWindow(string message, string buttonText, Action onButtonPressed)
        {
            optionUI.SetActive(true);
            displayText.text = message;
            displayButtonText.text = buttonText;
            displayButton.onClick.RemoveAllListeners();
            displayButton.onClick.AddListener(() =>
            {
                onButtonPressed?.Invoke();
                optionUI.SetActive(false);
            });
        }
    }

}