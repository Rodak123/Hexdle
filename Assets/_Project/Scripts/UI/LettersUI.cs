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
            for (int i = 0; i < Words.Letters.Count; i++)
            {
                char letter = Words.Letters[i];
                LetterUI letterUI = Instantiate(letterPrefab, lettersContainer);
                letterUI.Letter = letter;

                if (i == Words.Letters.Count / 2 - 1)
                {
                    for (int n = 0; n < 3; n++)
                    {
                        GameObject gameObject = new("Space", typeof(RectTransform));
                        gameObject.transform.SetParent(lettersContainer);
                    }
                }
            }
        }
    }
}
