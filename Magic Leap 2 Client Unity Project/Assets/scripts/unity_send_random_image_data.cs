using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using UnityEngine.UI;

using UnityEngine;

public class Send_image_data : MonoBehaviour
{

    public CameraManager cameraManager;
    public MeshRenderer mymeshRenderer;
    
    public RawImage myRawImage;
    private Socket socket = new Socket(IPAddress.Any.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

    // a buffer for sending and receiving small amounts of data. 
    private byte[] buffer = new byte[1024];

    // Dictionary<string, float> dict_pose = new Dictionary<string, float>();   	
    //private byte[,,] img = new byte[200, 320, 3];

    // buffer for image data
    private const int img_w = 320;
    private const int img_h = 200;
    private byte[] img = new byte[img_w * img_h * 3];

    // convert an integer to a byte array
    byte[] int_to_bytes(int x)
    {
        // https://stackoverflow.com/questions/1318933/c-sharp-int-to-byte
        byte[] b = BitConverter.GetBytes(x);
        if (BitConverter.IsLittleEndian)
        {
            Array.Reverse(b);
        }
        return b;
    }


    // convert the string to a ascii encoded byte buffer 
    // and copy it to a fixed length buffer
    byte[] str_to_byte_buffer(string str, byte[] buffer)
    {
        // set buffer to zero
        for (int i = 0; i < buffer.Length; i++) { buffer[i] = 0; }


        // encode the string into a new buffer
        byte[] str_bytes = System.Text.Encoding.ASCII.GetBytes(str);

        // check if the encoded string into the buffer
        if (str_bytes.Length <= buffer.Length)
        {
            str_bytes.CopyTo(buffer, 0);
        }
        else
        {
            Console.WriteLine($"error: string does not fit into byte-buffer");
        }
        return buffer;
    }

byte[] ConvertTextureToBytes(WebCamTexture webCamTexture)
    {
        Texture2D _texture2D = new Texture2D(webCamTexture.width, webCamTexture.height);
        _texture2D.SetPixels32(webCamTexture.GetPixels32());

        byte[] pixelData = _texture2D.GetRawTextureData();
        return pixelData;
    }
    

    // Start is called before the first frame update
    void Start()
    {
        try
        {
            //var address = "0.0.0.0";
            var address = "127.0.0.1";
            //var address = "192.168.0.168";

            var port = 65432;
            var endpoint = new IPEndPoint(IPAddress.Parse(address), port);

            socket.Connect(endpoint);
            Console.WriteLine($"socket connected to: {address}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"socket operation failed: {e.Message}");
            throw;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // generate a random image data 
        //var rnd = new System.Random();
        //rnd.NextBytes(img);
        //var tmp = img[0];
        //Console.WriteLine($"value at img[0]: {tmp}");

        try
        {
            Console.WriteLine("trying to send");

            // method1: send integers converted to byte buffer:
            //socket.Send(int_to_bytes(img_w), SocketFlags.None);
            //socket.Send(int_to_bytes(img_h), SocketFlags.None);

            // method2: send a string converted to an ascii-encoded byte buffer
            //string str = $"image:raw,{img_w},{img_h},3,{img.Length}";
            //Console.WriteLine(str);
            //buffer = str_to_byte_buffer(str, buffer);
            //socket.Send(buffer, SocketFlags.None);

            // now send the image
            socket.Send(ConvertTextureToBytes(cameraManager.frontCam), SocketFlags.None);
            Console.WriteLine("finished sending");


            // receive data from python
            Console.WriteLine("trying to receive data");
            var received = socket.Receive(buffer, SocketFlags.None);
            var response = Encoding.UTF8.GetString(buffer, 0, received);
            Console.WriteLine(response);
        }
        catch (Exception e)
        {
            Console.WriteLine($"socket operation failed: {e.Message}");
            //throw(e);
        }
    }
}
