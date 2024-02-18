using UnityEngine;
using WebSocketSharp;

public class WebSocketExample : MonoBehaviour
{
    // Specify the IP address and port for the WebSocket server
    private string serverIp = "192.168.0.104";
    private int serverPort = 8765;

    private WebSocket ws;

    void Start()
    {
        // Create a WebSocket instance with the specified URL
        string url = $"ws://{serverIp}:{serverPort}";
        ws = new WebSocket(url);

        // Event handler for when the WebSocket connection is opened
        ws.OnOpen += (sender, e) =>
        {
            Debug.Log("WebSocket connection opened");
        };

        // Event handler for receiving messages
        ws.OnMessage += (sender, e) =>
        {
            Debug.Log($"Received message: {e.Data}");
        };

        // Event handler for when the WebSocket connection is closed
        ws.OnClose += (sender, e) =>
        {
            Debug.Log("WebSocket connection closed");
        };

        // Start the WebSocket connection
        ws.Connect();
    }

    void Update()
    {
        // Perform any further actions or send messages as needed

        // For example, you can send a message
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ws.Send("Hello, WebSocket!");
        }
    }

    void OnDestroy()
    {
        // Close the WebSocket connection when the GameObject is destroyed
        if (ws != null && ws.IsAlive)
        {
            ws.Close();
        }
    }
}
