using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Rodak.Hexdle
{
    public class InfoModalUI : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private GameObject container;

        [Space]
        [SerializeField] private Tab[] tabs;

        [Space]
        [SerializeField] private TMP_Text strainText;
        [SerializeField] private GameObject notFinishedContainer;
        [SerializeField] private GameObject finishedContainer;

        [Space]
        [SerializeField] private GameObject buttonClosedIcon;
        [SerializeField] private GameObject buttonOpenIcon;

        private void Awake()
        {
            GameManager.Instance.OnGameStarted += GameManager_OnGameStarted;
            GameManager.Instance.OnGuessedRight += GameManager_OnGuessedRight;
            ShowTab(0);

            notFinishedContainer.SetActive(true);
            finishedContainer.SetActive(false);
        }

        private void Update()
        {
            if (Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                Toggle();
            }

            notFinishedContainer.SetActive(!GameStateManager.State.GuessedRight);
            finishedContainer.SetActive(GameStateManager.State.GuessedRight);
            strainText.text = $"Using\n<size=140><b>{GameStateManager.State.Penalty}</b></size>\nSTRAIN";
        }

        private void GameManager_OnGameStarted()
        {
            container.SetActive(!GameStateManager.State.SeenInfo || GameStateManager.State.GuessedRight);
            ShowTab(GameStateManager.State.GuessedRight ? 1 : 0);
        }

        private void GameManager_OnGuessedRight()
        {
            ShowTab(1);
            SetOpen(true);
        }

        private void SetOpen(bool isOpen)
        {
            container.SetActive(isOpen);
            GameStateManager.State.SeenInfo = true;
            GameStateManager.Save();

            buttonClosedIcon.SetActive(!isOpen);
            buttonOpenIcon.SetActive(isOpen);
        }

        public void Toggle()
        {
            SetOpen(!container.activeSelf);
        }

        public void ShowTab(int index)
        {
            for (int i = 0; i < tabs.Length; i++)
            {
                Tab tab = tabs[i];
                bool isShown = i == index;

                tab.Button.interactable = !isShown;
                tab.ButtonBG.SetIsInverted(isShown);
                tab.ButtonText.SetIsInverted(!isShown);
                tab.Contents.SetActive(isShown);
            }
        }

        [Serializable]
        public struct Tab
        {
            public Button Button;
            public ColoredImage ButtonBG;
            public ColoredText ButtonText;
            public GameObject Contents;
        }
    }
}
