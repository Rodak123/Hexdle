using TMPro;
using UnityEngine;

namespace Rodak.Hexdle
{
    public class LuckyNumberUI : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private TMP_Text numberText;

        private void Awake()
        {
            GameManager.Instance.OnGameStarted += GameManager_OnGameStarted;
        }

        private void GameManager_OnGameStarted()
        {
            numberText.text = $"{GameManager.Instance.LuckyNumber}";
        }
    }
}