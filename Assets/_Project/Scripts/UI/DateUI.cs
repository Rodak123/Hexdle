using UnityEngine;
using TMPro;

namespace Rodak.Hexdle
{
    public class DateUI : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private TMP_Text dateText;

        private void Awake()
        {
            dateText.text = Words.Today.ToString("MM/dd\nyyyy");
        }
    }
}
