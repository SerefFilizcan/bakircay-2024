using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Match
{
    [CreateAssetMenu(fileName = "ItemRepository", menuName = "Match/ItemRepository")]
    public class ItemRepository : ScriptableObject
    {
        public List<ItemData> itemDatas = new List<ItemData>();

        public List<ItemData> GetRandomItems(int count)
        {
            List<ItemData> result = new List<ItemData>();
            if (count > itemDatas.Count)
            {
                Debug.LogWarning("Count is greater than the number of items in the repository");
                return result;
            }

            var shuffledList = Shuffle(itemDatas);
            result.AddRange(shuffledList.GetRange(0, count));
            return result;
        }

        private List<ItemData> Shuffle(List<ItemData> objects)
        {
            for (int i = 0; i < objects.Count; i++)
            {
                int randomIndex = Random.Range(i, objects.Count);
                var temp = objects[i];
                objects[i] = objects[randomIndex];
                objects[randomIndex] = temp;
            }

            return objects;
        }
    }
}