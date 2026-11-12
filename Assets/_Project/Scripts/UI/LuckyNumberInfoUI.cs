using Rodak.Hexagons;
using Rodak.Hexagons.HexGeometry;
using UnityEngine;

namespace Rodak.Hexdle
{
    public class LuckyNumberInfoUI : MonoBehaviour
    {
        [SerializeField] private GameObject infoContainer;

        private void Update()
        {
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(InputManager.Instance.ReadMapInteractionPosition());
            Hexagon mouseHexagon = HexagonGeometryExtensions.GetHexagonAt(worldPoint.x, worldPoint.y);

            infoContainer.SetActive(mouseHexagon == Hexagon.Zero);
        }
    }
}