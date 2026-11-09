using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Rodak.Hexdle
{
    public class FallingLetters : MonoBehaviour
    {
        [SerializeField] private float letterSpawnInterval = 1;
        [SerializeField] private float letterSpeed = 1;
        [SerializeField] private float range = 10;
        [SerializeField] private float maxSpawned = 100;
        [SerializeField] private float spacing = 1;
        [SerializeField] private float initialSimulationTime = 60;

        [SerializeField] private bool randomFlowDirectionAngle = true;
        [SerializeField, Range(0, 360)] private float flowDirectionAngle = 0;

        [Header("UI")]
        [SerializeField] private GameObject letterPrefab;

        private readonly List<GameObject> spawnedLetters = new();
        private float letterSpawnTimer;

        private Vector2 FlowDirection => new(
                    Mathf.Cos(flowDirectionAngle * Mathf.Deg2Rad),
                    Mathf.Sin(flowDirectionAngle * Mathf.Deg2Rad)
                );

        private void Start()
        {
            if (randomFlowDirectionAngle) flowDirectionAngle = Random.Range(0, 360);

            float deltaTime = 1f / 10f;
            int frames = Mathf.CeilToInt(initialSimulationTime / deltaTime);
            for (int i = 0; i < frames; i++)
            {
                UpdateCleanup();
                UpdateMovement(deltaTime);
                UpdateSpawning(deltaTime);
            }
        }

        private void Update()
        {
            UpdateCleanup();
            UpdateMovement(Time.deltaTime);
            UpdateSpawning(Time.deltaTime);
        }

        private void UpdateSpawning(float deltaTime)
        {
            if (spawnedLetters.Count > maxSpawned) return;
            letterSpawnTimer += deltaTime;
            if (letterSpawnTimer > letterSpawnInterval)
            {
                Vector2 sideDirection = new(-FlowDirection.y, FlowDirection.x);
                Vector2 spawnPosition = -FlowDirection * range + sideDirection * range * Random.Range(-0.8f, 0.8f);
                foreach (GameObject letter in spawnedLetters)
                {
                    if (Vector2.Distance(spawnPosition, (Vector2)letter.transform.position) < spacing) return;
                }

                GameObject letterGO = Instantiate(letterPrefab, transform);
                letterGO.transform.position = spawnPosition;
                spawnedLetters.Add(letterGO);

                letterGO.GetComponentInChildren<TMP_Text>().text = $"{Words.Letters[Random.Range(0, Words.Letters.Count)]}";

                letterSpawnTimer -= letterSpawnInterval;
            }
        }

        private void UpdateMovement(float deltaTime)
        {
            foreach (GameObject letter in spawnedLetters)
            {
                letter.transform.position += (Vector3)FlowDirection * letterSpeed * deltaTime;
            }
        }

        private void UpdateCleanup()
        {
            for (int i = spawnedLetters.Count - 1; i >= 0; i--)
            {
                GameObject letter = spawnedLetters[i];
                if (letter.transform.position.magnitude <= range * 2) continue;
                spawnedLetters.RemoveAt(i);
                Destroy(letter);
            }
        }
    }
}
