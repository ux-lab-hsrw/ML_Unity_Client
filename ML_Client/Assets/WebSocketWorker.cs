using UnityEngine;
using WebSocketSharp;

public class WebSocketClient : MonoBehaviour
{
    private WebSocket ws;

    private const string serverUrl = "ws://localhost:8765"; // Replace with your WebSocket server URL

    void Start()
    {
        // Initialize WebSocket connection
        ws = new WebSocket(serverUrl);

        // Set up event handlers
        ws.OnOpen += OnWebSocketOpen;
        ws.OnMessage += OnWebSocketMessage;
        ws.OnError += OnWebSocketError;
        ws.OnClose += OnWebSocketClose;

        // Connect to the WebSocket server
        ws.Connect();
    }

    void OnDestroy()
    {
        // Close the WebSocket connection when the script is destroyed
        if (ws != null && ws.IsAlive)
        {
            ws.Close();
        }
    }

    void OnWebSocketOpen(object sender, System.EventArgs e)
    {
        Debug.Log("WebSocket opened");
        
        // Send a sample message after the connection is opened
        SendWebSocketMessage("Deez Nuts");
    }

    void OnWebSocketMessage(object sender, MessageEventArgs e)
    {
        Debug.Log("WebSocket message received: " + e.Data);

        // Handle the received message here
    }

    void OnWebSocketError(object sender, ErrorEventArgs e)
    {
        Debug.LogError("WebSocket error: " + e.Message);
    }

    void OnWebSocketClose(object sender, CloseEventArgs e)
    {
        Debug.Log("WebSocket closed with code: " + e.Code + ", reason: " + e.Reason);
    }

    void SendWebSocketMessage(string message)
    {
        // Check if the WebSocket connection is open before sending a message
        if (ws != null && ws.IsAlive)
        {
            ws.Send(message);
        }
        else
        {
            Debug.LogWarning("WebSocket connection is not open. Unable to send message.");
        }
    }
}
