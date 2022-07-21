using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class VirtualJoystick : MonoBehaviour
{
    public Vector2 Vector2Position { get => new Vector2(transform.position.x, transform.position.y); }

    [SerializeField] Camera _mainCamera;
    [SerializeField] private float _radius;
    [SerializeField] private GameObject _joystickBackground;
    [SerializeField] private GameObject _joyButton;

    [SerializeField] private bool _isTouched;

    // Start is called before the first frame update
    void Start()
    {
        _radius = _joystickBackground.transform.localScale.x / 2;
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
        //HandleJoystickInput2();
    }

    private void HandleJoystickInput()
    {
        if (Input.touchCount > 0 && Time.timeScale > 0)
        {
            Touch touch = Input.GetTouch(0);

            Vector2 touchPosition = _mainCamera.ScreenToWorldPoint(touch.position);
            Vector2 joystickDirection = touchPosition - Vector2Position;

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    OnJoystickPressed(joystickDirection);

                    break;

                case TouchPhase.Ended:

                    _joyButton.transform.position = transform.position;
                    OnJoystickReleased(joystickDirection);
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

                        OnJoystickHeld(joystickDirection);


                        break;
                    }


            }
        }
    }

    private void HandleJoystickInput2()
    {
        if (Input.touchCount > 0 && Time.timeScale > 0)
        {
            Touch touch = Input.GetTouch(0);

            Debug.Log("touch");
            if (_isTouched)
            {
                Vector2 touchPosition = _mainCamera.ScreenToWorldPoint(touch.position);
                Vector2 joystickDirection = touchPosition - Vector2Position;

                if (joystickDirection.magnitude > _radius / 10) // Joystick was dragged enough to have an effect
                {
                    DisplayJoystickDrag(joystickDirection, touchPosition);
                    HandleTouch(joystickDirection, touch);
                }
            }
        }
    }

    private void DisplayJoystickDrag(Vector2 direction, Vector2 touchPosition)
    {
        if (direction.magnitude <= _radius)
        {
            _joyButton.transform.position = touchPosition;
        }
        else
        {
            _joyButton.transform.position = transform.position + (new Vector3(direction.normalized.x, direction.normalized.y) * _radius);
        }
    }

    private void HandleTouch(Vector2 direction, Touch touch)
    {
        switch (touch.phase)
        {
            case TouchPhase.Began:
                //ToggleVisibility(true);
                //Vector2 touchPosition = _mainCamera.ScreenToWorldPoint(touch.position);
                //transform.position = touchPosition;
                OnJoystickPressed(direction);

                break;

            case TouchPhase.Ended:

                _joyButton.transform.position = transform.position;
                OnJoystickReleased(direction);
                //ToggleVisibility(false);

                break;

            default:

                OnJoystickHeld(direction);
                break;
        } // end of switch
    }

    private void OnMouseDown()
    {
        _isTouched = true;
    }

    private void OnMouseUp()
    {
        _isTouched = false;
    }

    protected abstract void OnJoystickHeld(Vector3 direction);
    protected abstract void OnJoystickPressed(Vector3 direction);
    protected abstract void OnJoystickReleased(Vector3 direction);

    private void ToggleVisibility(bool state)
    {
        _joystickBackground.SetActive(state);
        _joyButton.SetActive(state);
    }
}
