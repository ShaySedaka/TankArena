using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class VirtualJoystick : MonoBehaviour
{
    public Vector2 Vector2Position { get => new Vector2(transform.position.x, transform.position.y); }

    private int _currentTouchIndex = 0;

    [SerializeField] private Vector2 Vector2PosInspector;
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private float _radius;
    [SerializeField] private GameObject _joystickBackground;
    [SerializeField] private GameObject _joyButton;

    [SerializeField] private bool _isTouched;

    [SerializeField] protected UnityEvent<Vector3> _onTouchStart;
    [SerializeField] protected UnityEvent<Vector3> _onTouchEnd;
    [SerializeField] protected UnityEvent<Vector3> _onTouchHeld;

    // Start is called before the first frame update
    void Start()
    {
        _radius = _joystickBackground.GetComponent<RectTransform>().sizeDelta.x / 2;
        Vector2PosInspector = Vector2Position;
        _mainCamera = RoomManager.Instance.MainCamera;
        TheBetterStart();
        //ToggleVisibility(false);
    }

    protected virtual void TheBetterStart()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleJoystickInput();
    }

    private void HandleJoystickInput()
    {
        if (Input.touchCount > 0 && Time.timeScale > 0)
        {
            int touchNumber = Input.touchCount - 1;
            if(_isTouched)
            {
                touchNumber = _currentTouchIndex;
            }

            Touch touch = Input.GetTouch(touchNumber);

            Vector2 touchPosition = touch.position;
            float touchDistanceFromJoystick = (Vector2Position - touchPosition).magnitude;

            if (touchDistanceFromJoystick <= _radius || _isTouched)
            {
                
                Vector2 joystickDirection = touchPosition - Vector2Position;

                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        _currentTouchIndex = Input.touchCount - 1;
                        _isTouched = true;
                        _onTouchStart?.Invoke(joystickDirection);

                        break;

                    case TouchPhase.Ended:
                        
                        _isTouched = false;
                        _joyButton.transform.position = transform.position;
                        _onTouchEnd?.Invoke(joystickDirection);
                        break;

                    default:

                        if (joystickDirection.magnitude <= _radius / 10)
                        {
                            break;
                        }
                        else
                        {
                            if (joystickDirection.magnitude <= _radius)
                            {
                                _joyButton.transform.position = touchPosition;
                            }
                            else
                            {
                                _joyButton.transform.position = transform.position + (new Vector3(joystickDirection.normalized.x, joystickDirection.normalized.y) * _radius);
                            }

                            _onTouchHeld?.Invoke(joystickDirection);


                            break;
                        }


                } // end of switch
            }      
        }
    }

    private void ToggleVisibility(bool state)
    {
        _joystickBackground.SetActive(state);
        _joyButton.SetActive(state);
    }
}
