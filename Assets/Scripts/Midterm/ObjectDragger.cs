using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;

namespace Midterm
{
    public class ObjectDragger : MonoBehaviour
    {
        public GameObject draggedObject;

        public LayerMask raycastLayer;

        public Vector3 dragBorder;

        // Start is called before the first frame update
        void Start()
        {
            if (TouchManager.Instance == null)
            {
                Debug.LogError("TouchManager instance not found!");
                enabled = false;
                return;
            }

            TouchManager.Instance.OnTouchBegan += TouchBegan;
            TouchManager.Instance.OnTouchMoved += TouchMoved;
            TouchManager.Instance.OnTouchEnded += TouchEnded;
        }

        private void OnDestroy()
        {
            if (!TouchManager.Instance) 
                return;
            TouchManager.Instance.OnTouchBegan -= TouchBegan;
            TouchManager.Instance.OnTouchMoved -= TouchMoved;
            TouchManager.Instance.OnTouchEnded -= TouchEnded;
        }

        private void TouchBegan(TouchData touchData)
        {
            if (draggedObject != null)
            {
                ReleaseObject();
            }

            CastRay(touchData);
        }


        private void TouchMoved(TouchData touchData)
        {
            if (draggedObject != null)
            {
                MoveObject(touchData);
            }
        }


        private void TouchEnded(TouchData touchData)
        {
            if (draggedObject != null)
            {
                ReleaseObject();
            }
        }


        private void ReleaseObject()
        {
            if (draggedObject == null)
                return;

            draggedObject.GetComponent<Rigidbody>().isKinematic = false;
            draggedObject = null;
        }

        private void CastRay(TouchData touchData)
        {
            var ray = Camera.main.ScreenPointToRay(touchData.position);
            if (Physics.Raycast(ray, out var hit, 1000f, raycastLayer))
            {
                draggedObject = hit.collider.attachedRigidbody.gameObject;
            }
        }

        private void MoveObject(TouchData touchData)
        {
            var speed = 10f;
            draggedObject.GetComponent<Rigidbody>().isKinematic = true;
            var position = draggedObject.transform.position;
            position.y = 7;
            position.x += touchData.deltaPosition.x * speed * Time.deltaTime;
            position.z += touchData.deltaPosition.y * speed * Time.deltaTime;

            position.x = Mathf.Clamp(position.x, transform.position.x - dragBorder.x * 0.5f,
                transform.position.x + dragBorder.x * 0.5f);
            position.z = Mathf.Clamp(position.z, transform.position.z - dragBorder.z * 0.5f,
                transform.position.z + dragBorder.z * 0.5f);

            draggedObject.transform.position = Vector3.Lerp(draggedObject.transform.position, position,
                Time.deltaTime * speed * speed);
        }


        private void OnDrawGizmos()
        {
            Gizmos.color = (Color.blue + Color.green) / 2f;
            Gizmos.DrawWireCube(transform.position, dragBorder);
        }
    }
}