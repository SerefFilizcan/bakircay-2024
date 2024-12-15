using System;
using UnityEngine;

namespace Common
{
    public struct TouchData
    {
        public bool hasValue;
        public Vector2 position;
        public Vector2 deltaPosition;
    }

    public class TouchManager : MonoBehaviour
    {
        private static TouchManager _instance;

        public static TouchManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<TouchManager>();
                }

                if (_instance == null)
                {
                    Debug.LogWarning("TouchManager not found in scene");
                }

                return _instance;
            }
        }

        public Action<TouchData> OnTouchBegan;
        public Action<TouchData> OnTouchMoved;
        public Action<TouchData> OnTouchEnded;

        private TouchData _currentTouchData = new TouchData() { hasValue = false };

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                TouchBegan(Input.mousePosition);
            }
            else if (Input.GetMouseButton(0))
            {
                if (_currentTouchData.hasValue == false)
                {
                    TouchBegan(Input.mousePosition);
                    return;
                }

                ;

                TouchMoved(Input.mousePosition);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                if (_currentTouchData.hasValue == false)
                {
                    return;
                }

                ;
                TouchEnded(Input.mousePosition);
            }
        }


        private void TouchBegan(Vector2 position)
        {
            var data = new TouchData()
            {
                hasValue = true,
                position = position,
                deltaPosition = Vector2.zero
            };

            _currentTouchData = data;
            OnTouchBegan?.Invoke(_currentTouchData);
        }


        private void TouchMoved(Vector2 position)
        {
            var data = new TouchData()
            {
                hasValue = true,
                position = position,
                deltaPosition = position - _currentTouchData.position
            };

            _currentTouchData = data;
            OnTouchMoved?.Invoke(_currentTouchData);
        }

        private void TouchEnded(Vector2 position)
        {
            var data = new TouchData()
            {
                hasValue = false,
                position = position,
                deltaPosition = position - _currentTouchData.position
            };

            _currentTouchData = data;
            OnTouchEnded?.Invoke(_currentTouchData);
        }
    }
}