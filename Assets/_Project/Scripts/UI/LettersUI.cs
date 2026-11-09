using UnityEngine;

namespace Rodak.Hexdle
{
    public class LettersUI : MonoBehaviour
    {
        [SerializeField] private Map map;

        [Header("UI")]
        [SerializeField] private LetterUI letterPrefab;
        [SerializeField] private Transform lettersContainer;

        private void Awake()
        {
            foreach (char letter in Words.Letters)
            {
                LetterUI letterUI = Instantiate(letterPrefab, lettersContainer);
                letterUI.Letter = letter;
                letterUI.Map = map;
            }
        }
    }
}
