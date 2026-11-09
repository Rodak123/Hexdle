using TMPro;
using UnityEngine;

namespace Rodak.Hexdle
{
    public class PenaltyUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text penaltyText;

        private void Update()
        {
            penaltyText.text = $"{GameManager.Instance.Penalty}";
        }
    }
}
