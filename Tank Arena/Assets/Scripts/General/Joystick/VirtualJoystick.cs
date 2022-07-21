using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class VirtualJoystick : MonoBehaviour
{
    public Vector2 Vector2Position { get => new Vector2(transform.position.x, transform.position.y); }

    [SerializeField] private Vector2 Vector2PosInspector;
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private float _radius;
    [SerializeField] private GameObject _joystickBackground;
    [SerializeField] private GameObject _joyButton;

    [SerializeField] private bool _isTouched;

    [SerializeField] private PhotonView _pv; // TODO: get rid of this

    [SerializeField] private UnityEvent _onStart;

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
        //HandleJoystickInput2();
    }

    private void HandleJoystickInput()
    {
        if (Input.touchCount > 0 && Time.timeScale > 0)
        {
            Touch touch = Input.GetTouch(0);

            Vector2 touchPosition = touch.position;
            float touchDistanceFromJoystick = (Vector2Position - touchPosition).magnitude;

            if (touchDistanceFromJoystick <= _radius || _isTouched)
            {
                
                Vector2 joystickDirection = touchPosition - Vector2Position;

                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        _isTouched = true;
                        OnJoystickPressed(joystickDirection);

                        break;

                    case TouchPhase.Ended:
                        _isTouched = false;
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


                } // end of switch
            }      
        }
    }

    //private void HandleJoystickInput2()
    //{
    //    if (Input.touchCount > 0 && Time.timeScale > 0)
    //    {
    //        Touch touch = Input.GetTouch(0);

    //        Debug.Log("touch");
    //        if (_isTouched)
    //        {
    //            Vector2 touchPosition = touch.position;
    //            Vector2 joystickDirection = touchPosition - Vector2Position;

    //            if (joystickDirection.magnitude > _radius / 10) // Joystick was dragged enough to have an effect
    //            {
    //                DisplayJoystickDrag(joystickDirection, touchPosition);
    //                HandleTouch(joystickDirection, touch);
    //            }
    //        }
    //    }
    //}

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
        _pv.RPC("RPC_DebugBool", RpcTarget.All, _isTouched);
    }

    private void OnMouseUp()
    {
        _isTouched = false;
        _pv.RPC("RPC_DebugBool", RpcTarget.All, _isTouched);
    }

    [PunRPC]
    private void RPC_DebugBool(bool givenBool)
    {
        Debug.Log("isTouched is " + givenBool);
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
