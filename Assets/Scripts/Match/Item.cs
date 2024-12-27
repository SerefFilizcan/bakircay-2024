using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Match
{
    public class Item : MonoBehaviour
    {
        public int matchID = -1;
        public Rigidbody selfRigidbody;

        public bool isDragged = false;
        public bool isPlaced = false;
        public ItemData itemData;

        private List<Collider> _colliders = new List<Collider>();

        private void Awake()
        {
            selfRigidbody = GetComponent<Rigidbody>();
            _colliders.AddRange(GetComponentsInChildren<Collider>());
        }

        public bool IsMatching(Item otherItem)
        {
            return this != otherItem && matchID == otherItem.matchID;
        }

        public void SetCollidersActive(bool isActive)
        {
            foreach (var col in _colliders)
            {
                col.enabled = isActive;
            }
        }

        private void FixedUpdate()
        {
            if (transform.position.y < -5)
            {
                ReCenterObject();
            }
        }

        private void ReCenterObject()
        {
            selfRigidbody.velocity = Vector3.zero;
            selfRigidbody.angularVelocity = Vector3.zero;
            
            selfRigidbody.isKinematic = true; 
            transform.position = Vector3.up * 2f;
            selfRigidbody.isKinematic = false;
        }
    }
}