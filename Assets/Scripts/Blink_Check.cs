using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class BlinkReceiver : MonoBehaviour
{
    // A flag to indicate a blink was detected
    public static bool wasBlinkDetected = false;

    // UDP listener thread
    private Thread receiveThread;
    private UdpClient client;
    public int port = 5005; // Must match the port in the Python script

    void Start()
    {
        // Start the listening thread
        receiveThread = new Thread(new ThreadStart(ReceiveData));
        receiveThread.IsBackground = true;
        receiveThread.Start();
        Debug.Log("Started listening for blinks from Python.");
    }

    private void ReceiveData()
    {
        client = new UdpClient(port);
        while (true)
        {
            try
            {
                IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
                byte[] data = client.Receive(ref anyIP);
                string text = Encoding.UTF8.GetString(data);

                // If we receive "true", set the flag
                if (text == "true")
                {
                    wasBlinkDetected = true;
                }
            }
            catch (Exception err)
            {
                Debug.LogError(err.ToString());
            }
        }
    }

    void Update()
    {
        // In the main game loop, check if the flag was set
        if (wasBlinkDetected)
        {
            // --- YOUR GAME LOGIC HERE ---
            // Example: Make a character jump, trigger an event, etc.
            Debug.Log("Blink Detected! Performing game action.");

            // Reset the flag so the action only happens once per blink
            wasBlinkDetected = false;
        }
    }

    // Clean up the thread and socket when the application quits
    void OnApplicationQuit()
    {
        if (receiveThread != null && receiveThread.IsAlive)
        {
            receiveThread.Abort();
        }
        if (client != null)
        {
            client.Close();
        }
    }
}