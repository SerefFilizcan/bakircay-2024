using System;
using System.Collections;
using System.Collections.Generic;
using Common;
using DG.Tweening;
using UnityEngine;

namespace Midterm
{
    public class ObjectDragger : MonoBehaviour
    {
        public GameObject draggedObject;

        public LayerMask raycastLayer;

        public Vector3 dragBorder;


        [SerializeField] private Vector3 _targetPosition;
        private Plane _dragPlane = new Plane(Vector3.up, new Vector3(0, 3, 0));
        private float _verticalOffset = 6f;

        // Start is called before the first frame update
        void Start()
        {
            if (TouchManager.Instance == null)
            {
                Debug.LogError("TouchManager instance not found!");
                enabled = false;
                return;
            }

            _dragPlane = new Plane(Vector3.up, new Vector3(0, _verticalOffset, 0));

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
            SetTargetPosition(touchData.position);
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
            _targetPosition = Vector3.zero;
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
                DOTween.Kill(draggedObject.transform);
            }
        }

        private void MoveObject(TouchData touchData)
        {
            var speed = 10f;
            var forwardOffset = 1f;
            draggedObject.GetComponent<Rigidbody>().isKinematic = true;
            var position = _targetPosition;
            position += Vector3.forward * forwardOffset;

            position.x = Mathf.Clamp(position.x, transform.position.x - dragBorder.x * 0.5f,
                transform.position.x + dragBorder.x * 0.5f);
            position.z = Mathf.Clamp(position.z, transform.position.z - dragBorder.z * 0.5f,
                transform.position.z + dragBorder.z * 0.5f);

            draggedObject.transform.position = Vector3.Lerp(draggedObject.transform.position, position,
                Time.deltaTime * speed * speed);
        }

        private void SetTargetPosition(Vector2 screenPosition)
        {
            Ray ray = Camera.main.ScreenPointToRay(screenPosition);
            if (_dragPlane.Raycast(ray, out float distance))
            {
                _targetPosition = ray.GetPoint(distance);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = (Color.blue + Color.green) / 2f;
            Gizmos.DrawWireCube(transform.position, dragBorder);

            if (_targetPosition != Vector3.zero)
            {
                Gizmos.color = (Color.red + Color.green) / 2f;
                Gizmos.DrawSphere(_targetPosition, 1f);
            }
        }
    }
}