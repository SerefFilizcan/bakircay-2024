using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Match
{
    public class Item : MonoBehaviour
    {
        
        public int matchID = -1;

        public bool isDragged = false;
        public bool isPlaced = false;
        public ItemData itemData;

        public bool IsMatching(Item otherItem)
        {
            return this != otherItem && matchID == otherItem.matchID;
        }
    }
}