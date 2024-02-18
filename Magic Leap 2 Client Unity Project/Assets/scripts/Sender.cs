using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class SendImage : MonoBehaviour
{
    public string serverURL = "http://192.168.0.125:8080/upload";
    public string imagePath = "Assets/th.jpeg"; // Change this to your image path


    public void SendImageToDetection() {
       StartCoroutine(UploadImage()); 
    }

    IEnumerator UploadImage()
    {
        byte[] imageBytes = System.IO.File.ReadAllBytes(imagePath);

        WWWForm form = new WWWForm();
        form.AddBinaryData("img", imageBytes, "sample.png", "image/png");

        UnityWebRequest request = UnityWebRequest.Post(serverURL, form);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log("Image uploaded successfully!");
        }
        else
        {
            Debug.LogError("Error uploading image: " + request.error);
        }
    }
} 
