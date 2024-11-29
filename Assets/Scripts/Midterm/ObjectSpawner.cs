using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Midterm
{
    public class ObjectSpawner : MonoBehaviour
    {
        public GameObject objectPrefab;
        [Range(1, 50)] public int spawnCount = 10;
        public Vector3 spawnArea = new Vector3(5, 1, 5);
        [Range(1, 10)] public float spawnDistance = 1.7f;

        public List<Transform> spawnedObjects = new List<Transform>();

        private void Start()
        {
            SpawnObjects();
        }

        [ContextMenu("Spawn Objects")]
        private void SpawnObjects()
        {
            spawnedObjects.ForEach(x => Destroy(x.gameObject));
            spawnedObjects.Clear();

            int maxTries = 100;
            int currentTryCount = 0;
            for (int i = 0; i < spawnCount; i++)
            {
                Vector3 spawnPosition = transform.position + GetRandomPos();

                if (spawnedObjects.Any(x => Vector3.Distance(x.position, spawnPosition) < spawnDistance))
                {
                    currentTryCount++;
                    if (currentTryCount > maxTries)
                    {
                        Debug.LogWarning("Max tries reached");
                    }
                    else
                    {
                        i--;
                        continue;
                    }
                }

                currentTryCount = 0;
                var instance = Instantiate(objectPrefab, spawnPosition, Quaternion.identity);
                spawnedObjects.Add(instance.transform);
            }
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