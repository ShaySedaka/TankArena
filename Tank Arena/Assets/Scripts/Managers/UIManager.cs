using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI _connectionStatusText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnConnected()
    {
        _connectionStatusText.text = "CONNECTED";
        _connectionStatusText.color = Color.green;
    }

    public void OnConnecting()
    {
        _connectionStatusText.text = "CONNECTING...";
    }

    public void OnConnectionError()
    {
        _connectionStatusText.text = "DISCONNECTED";
        _connectionStatusText.color = Color.red;
    }
}
