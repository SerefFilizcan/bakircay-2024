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
    
        public bool IsMatching(Item otherItem)
        {
            return matchID == otherItem.matchID;
        }
        
    }
}