using UnityEngine;

public class CaptureMeshRenderer : MonoBehaviour
{
    public MeshRenderer meshRenderer;
    public string savePath = "Assets/SavedImages/";
    public string fileName = "CapturedImage.jpg"; // Name of the image file
    public float updateInterval = 1f; // Update interval in seconds

    private float lastCaptureTime = 0f;

    void OnDisable()
    {
        // Save the final captured image when the script is disabled
        CaptureAndSaveImage();
    }

    void Start()
    {
        if (meshRenderer == null)
        {
            Debug.LogError("MeshRenderer not assigned! Please assign a MeshRenderer in the inspector.");
            return;
        }

        // Create the save directory if it doesn't exist
        if (!System.IO.Directory.Exists(savePath))
        {
            System.IO.Directory.CreateDirectory(savePath);
        }

        // Set up the initial capture
        CaptureAndSaveImage();
    }

    void Update()
    {
        // Check if it's time to capture a new image
        if (Time.time - lastCaptureTime >= updateInterval)
        {
            CaptureAndSaveImage();
            lastCaptureTime = Time.time;
        }
    }

    void CaptureAndSaveImage()
    {
        // Ensure the camera is rendering to the screen
        Camera camera = Camera.main;
        camera.targetTexture = null;

        // Create a new Texture2D and read the pixels from the screen
        Texture2D texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        texture.Apply();

        // Convert the texture to JPEG format
        byte[] bytes = texture.EncodeToJPG();

        // Save the captured texture as a JPEG file (overwrite the existing file)
        System.IO.File.WriteAllBytes(savePath + fileName, bytes);

        Debug.Log("Image captured and saved at: " + savePath + fileName);
    }
}
    