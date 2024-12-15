using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Match
{
    public class ItemSpawner : MonoBehaviour
    {
        public ItemRepository itemRepository;
        [Range(1, 8)] public int spawnCount = 8;
        public Vector3 spawnArea = new Vector3(5, 1, 5);
        [Range(1, 10)] public float spawnDistance = 1.7f;

        public List<Transform> spawnedObjects = new List<Transform>();

        private void Start()
        {
            SpawnObjects();
        }

        private void Update()
        {
            if (spawnedObjects.Count == 0)
                return;

            if (spawnedObjects.All(x => x.gameObject.activeSelf == false))
            {
                Debug.Log("All objects are inactive");
                SpawnObjects();
            }
        }

        [ContextMenu("Spawn Objects")]
        private void SpawnObjects()
        {
            const int pairCount = 2;
            ClearSpawnedObjects();

            var itemData = itemRepository.GetRandomItems(spawnCount);
            if (itemData.Count == 0)
            {
                Debug.LogError("No items in the repository");
                return;
            }

            for (int i = 0; i < spawnCount; i++)
            {
                // Spawn two items for each item in the repository
                for (int j = 0; j < pairCount; j++)
                {
                    Vector3 spawnPosition = GetValidSpawnPosition();

                    var itemPrefab = itemData[i].itemPrefab;

                    var instance = Instantiate(itemPrefab, spawnPosition, Quaternion.identity);

                    instance.GetComponent<Item>().matchID = i;
                    spawnedObjects.Add(instance.transform);
                }
            }
        }

        private void ClearSpawnedObjects()
        {
            //wait for 2 seconds before destroying the objects, to prevent any ongoing operations
            const float delay = 2f;
            spawnedObjects.ForEach(x => Destroy(x.gameObject, delay));
            spawnedObjects.Clear();
        }

        private Vector3 GetValidSpawnPosition()
        {
            Vector3 spawnPosition;
            const int maxTries = 100;
            int currentTryCount = 0;
            bool isValid = false;
            do
            {
                spawnPosition = transform.position + GetRandomPos();
                currentTryCount++;
                isValid = !spawnedObjects.Any(x => Vector3.Distance(x.position, spawnPosition) < spawnDistance);
            } while (!isValid && currentTryCount <= maxTries);

            if (currentTryCount > maxTries)
            {
                Debug.LogWarning("Max tries reached");
            }

            return spawnPosition;
        }

        private Vector3 GetRandomPos()
        {
            return new Vector3(
                UnityEngine.Random.Range(-spawnArea.x * 0.5f, spawnArea.x * 0.5f),
                UnityEngine.Random.Range(-spawnArea.y * 0.5f, spawnArea.y * 0.5f),
                UnityEngine.Random.Range(-spawnArea.z * 0.5f, spawnArea.z * 0.5f)
            );
        }


        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(transform.position, spawnArea);
        }
    }
}