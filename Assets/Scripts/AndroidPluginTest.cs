using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;
using TMPro;
using System;

public class AndroidPluginTest : MonoBehaviour
{
    // Start is called before the first frame update
    public  TMP_Text DebugText;

     public GameObject deviceButtonPrefab;

     public GameObject scrollContent;
     private AndroidJavaObject bluetoothPluginManager;

    void Start()
    {
        // try
        // {
        //     using (AndroidJavaClass pluginClass = new AndroidJavaClass("com.onlykk.androidbluetoothplugin.BluetoothManager"))
        //     {
        //         // Get the Unity activity context
        //        // AndroidJavaObject unityActivity = GetUnityActivity();
        //         bluetoothManager = pluginClass.Call<AndroidJavaObject>("newInstance");
        //     }
        // }
        // catch (AndroidJavaException ex)
        // {
        //     Debug.LogError("AndroidJavaException caught: " + ex.Message);
        // }
        // catch (System.Exception ex)
        // {
        //     Debug.LogError("Exception caught: " + ex.Message);
        // }
    }

    void setDebugMsg(string msg){
         DebugText.text = msg;
    }

     public void testPluginLoad()
    {
        Debug.Log("Testing Plugin Load:");
        try
        {
            using (AndroidJavaClass pluginClass = new AndroidJavaClass("com.onlykk.androidbluetoothplugin.BluetoothPluginManager"))
            {
                Debug.Log("AndroidJavaClass created...");

                if (pluginClass != null)
                {
                    string message = pluginClass.CallStatic<string>("checkPluginLoad");
                    Debug.Log("Plugin Loaded: " + message);
                    setDebugMsg("Plugin Loaded: " + message);
                }
                else
                {
                    Debug.LogError("pluginClass is null.");
                }
            }
        }
        catch (AndroidJavaException ex)
        {
            Debug.LogError("AndroidJavaException caught: " + ex.Message);
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Exception caught: " + ex.Message);
        }
    }

    public void testBluetoothSupport()
    {
         Debug.Log("Testing Bluetooth Support:");
        try
        {
             using (AndroidJavaClass pluginClass = new AndroidJavaClass("com.onlykk.androidbluetoothplugin.BluetoothPluginManager"))
            {
                // Get the instance of the BluetoothManager
                bluetoothPluginManager = pluginClass.CallStatic<AndroidJavaObject>("getInstance");
            }
            bluetoothPluginManager.Call("initBluetoothPlugin", GetUnityActivity());
            bool initResult = bluetoothPluginManager.Call<bool>("isBluetoothSupported");
            Debug.Log("Bluetooth Adapter Initialized: " + initResult);
            setDebugMsg("Bluetooth Adapter Initialized: " + initResult);

        }
        catch (AndroidJavaException ex)
        {
            Debug.LogError("AndroidJavaException caught: " + ex.Message);
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Exception caught: " + ex.Message);
        }
    }

     public void testIsBluetoothEnabled()
    {
         Debug.Log("Testing is Bluetooth Enabled:");
        try
        {
            bool initResult = bluetoothPluginManager.Call<bool>("isBluetoothEnabled");
            Debug.Log("Is Bluetooth Enabled : " + initResult);
                  setDebugMsg("Is Bluetooth Enabled : " + initResult);
        }
        catch (AndroidJavaException ex)
        {
            Debug.LogError("AndroidJavaException caught: " + ex.Message);
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Exception caught: " + ex.Message);
        }
    }

    public void testHasBluetoothPermission() {

         Debug.Log("Checking Permission!");
        if (Permission.HasUserAuthorizedPermission("android.permission.BLUETOOTH"))
        {
            Debug.Log("android.permission.BLUETOOTH permission has been granted.");
        }

         if (Permission.HasUserAuthorizedPermission("android.permission.BLUETOOTH_ADMIN"))
        {
            Debug.Log("android.permission.BLUETOOTH_ADMIN permission has been granted.");
        }

         if (!Permission.HasUserAuthorizedPermission("android.permission.ACCESS_FINE_LOCATION"))
        {
             Permission.RequestUserPermissions(new string[] {
    "android.permission.ACCESS_FINE_LOCATION"
  });
        }else{
            Debug.Log("android.permission.ACCESS_FINE_LOCATION permission has been granted.");

        }

          if (!Permission.HasUserAuthorizedPermission("android.permission.BLUETOOTH_SCAN"))
        {
             Permission.RequestUserPermissions(new string[] {
    "android.permission.BLUETOOTH_SCAN"
  });
        }else{
            Debug.Log("android.permission.BLUETOOTH_SCAN permission has been granted.");

        }

         if (!Permission.HasUserAuthorizedPermission("android.permission.BLUETOOTH_CONNECT"))
        {
             Permission.RequestUserPermissions(new string[] {
    "android.permission.BLUETOOTH_CONNECT"
  });
        }else{
            Debug.Log("android.permission.BLUETOOTH_CONNECT permission has been granted.");

        }
    
             setDebugMsg("All permission permitted! " );
    }


     public void teststartDiscovery()
    {
         Debug.Log("Testing is initializeReceiver :");
        try
        {
            bool startDiscovery = bluetoothPluginManager.Call<bool>("startDiscoveredDevices");
            Debug.Log("startDiscovery : " + startDiscovery);
            setDebugMsg("startDiscovery : " + startDiscovery);

        }
        catch (AndroidJavaException ex)
        {
            Debug.LogError("AndroidJavaException caught: " + ex.Message);
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Exception caught: " + ex.Message);
        }
    }


    // private IEnumerator CheckDiscoveredDevices()
    // {
    //     yield return new WaitForSeconds(10); // Wait for 10 seconds to discover devices

    //     string[] discoveredDevices = bluetoothPluginManager.Call<string[]>("getDiscoveredDeviceDetails");

    //     foreach (string device in discoveredDevices)
    //     {
    //         Debug.Log("Discovered Device: " + device);
    //     }
    // }


     public void testgetDiscoveredDevices()
    {
         Debug.Log("Testing is getDiscoveredDevices :");
        try
        {
            //List<string> devices = bluetoothManager.Call<List<string>>("getDiscoveredDevices");
            //string combinedString = string.Join( ",", devices.ToArray() );
            //Debug.Log("getDiscoveredDevices : " + combinedString );
            //StartCoroutine(CheckDiscoveredDevices());
            string[] discoveredDevices = bluetoothPluginManager.Call<string[]>("getDiscoveredDeviceDetails");
 Debug.Log("Discovered Device: " + discoveredDevices.Length);
  setDebugMsg("Discovered Device:  " + discoveredDevices.Length);
        foreach (string device in discoveredDevices)
        {
            Debug.Log("Discovered Device: " + device);
            GameObject deviceButton = Instantiate(deviceButtonPrefab, scrollContent.transform);
            deviceButton.SetActive(true); // Enable the text object
            // Set the text of the button
              TMP_Text buttonText = deviceButton.GetComponentInChildren<TMP_Text>();
            if (buttonText != null)
            {
                buttonText.text = device;
            }


            // Optionally, you can add a listener to the button if you want to handle button clicks
            deviceButton.GetComponent<Button>().onClick.AddListener(() => OnDeviceButtonClick(device));
        }
        }
        catch (AndroidJavaException ex)
        {
            Debug.LogError("AndroidJavaException caught: " + ex.Message);
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Exception caught: " + ex.Message);
        }
    }

     public void testStartGATTServer()
    {
         Debug.Log("Testing StartGATT Server:");
        try
        {
            bluetoothPluginManager.Call("startGattServer");
            Debug.Log("StartGATT Server Started : ");
              setDebugMsg("StartGATT Server Started : Server");
        }
        catch (AndroidJavaException ex)
        {
            Debug.LogError("AndroidJavaException caught: " + ex.Message);
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Exception caught: " + ex.Message);
        }
    }

     public void testBLEScan()
    {
         Debug.Log("Testing Service Scan:");
        try
        {
            bluetoothPluginManager.Call("startBLEDiscovery");
            Debug.Log("StartGATT Service Scan : ");
             setDebugMsg("StartGATT Service Scan : Client ");

        }
        catch (AndroidJavaException ex)
        {
            Debug.LogError("AndroidJavaException caught: " + ex.Message);
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Exception caught: " + ex.Message);
        }
    }
string connectedDevice = "";

    public void OnDeviceButtonClick(string deviceInfo)
{
    setDebugMsg("Trying to connect to : " + deviceInfo);
      Debug.Log("Trying to connect to : " + deviceInfo);
    // You can expand this to connect to the device or perform other actions

     string[] parts = deviceInfo.Split(new string[] { " - " }, StringSplitOptions.None);
    if (parts.Length > 1)
    {
        string deviceAddress = parts[1];

        // Proceed with the connection using the extracted address
        try
        {
            bluetoothPluginManager.Call("connectToServer", deviceAddress);
            connectedDevice = deviceAddress;
            Debug.Log("Connecting to server: " + deviceAddress);
            setDebugMsg("Connecting to server: " + deviceAddress);
        }
        catch (AndroidJavaException ex)
        {
            Debug.LogError("AndroidJavaException caught: " + ex.Message);
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Exception caught: " + ex.Message);
        }
    }
    else
    {
        Debug.LogError("Failed to extract device address from info: " + deviceInfo);
        setDebugMsg("Failed to extract device address from info: " + deviceInfo);
    }

}

  public void testmsgFromServer()
{
      setDebugMsg("Trying testmsgFromServer : ");
      Debug.Log("Trying testmsgFromServer : ");
    // You can expand this to connect to the device or perform other actions

  
        // Proceed with the connection using the extracted address
        try
        {
            string msg = bluetoothPluginManager.Call<string>("getMsgFromServer");
            Debug.Log("testmsgFromServer : " + msg);
            setDebugMsg("testmsgFromServer : " + msg);
        }
        catch (AndroidJavaException ex)
        {
            Debug.LogError("AndroidJavaException caught: " + ex.Message);
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Exception caught: " + ex.Message);
        }
       
}

  public void testmsgFromClient()
{
      setDebugMsg("Trying testmsgFromClient : ");
      Debug.Log("Trying testmsgFromClient : ");
    // You can expand this to connect to the device or perform other actions


        // Proceed with the connection using the extracted address
        try
        {
             string msg = bluetoothPluginManager.Call<string>("getMsgFromClient");
            Debug.Log("testmsgFromClient : " + msg);
            setDebugMsg("testmsgFromClient : " + msg);
        }
        catch (AndroidJavaException ex)
        {
            Debug.LogError("AndroidJavaException caught: " + ex.Message);
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Exception caught: " + ex.Message);
        }
     
}


  public void testSendToServer()
{
      setDebugMsg("Trying to send data to : " + connectedDevice);
      Debug.Log("Trying to connect to : " + connectedDevice);
    // You can expand this to connect to the device or perform other actions

        if(connectedDevice != "")
        {
        // Proceed with the connection using the extracted address
        try
        {
            bluetoothPluginManager.Call("sendMessageToServer", "Hello from client");
            Debug.Log("send data to : " + connectedDevice);
            setDebugMsg("send data to : " + connectedDevice);
        }
        catch (AndroidJavaException ex)
        {
            Debug.LogError("AndroidJavaException caught: " + ex.Message);
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Exception caught: " + ex.Message);
        }
        }
        else{
            Debug.Log("No devices connected " );
            setDebugMsg("No devices connected " );
        }
}


  public void testSendToClient()
{
      setDebugMsg("Trying to testSendToClient: ");
      Debug.Log("Trying to testSendToClient : ");
    // You can expand this to connect to the device or perform other actions

        
        // Proceed with the connection using the extracted address
        try
        {
            bluetoothPluginManager.Call("sendMessageToClient", "Hello from server!");
            Debug.Log("send data to Client: " );
            setDebugMsg("send data to Client: ");
        }
        catch (AndroidJavaException ex)
        {
            Debug.LogError("AndroidJavaException caught: " + ex.Message);
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Exception caught: " + ex.Message);
        }
}


    private AndroidJavaObject GetUnityActivity()
    {
        using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            var activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            return activity.Call<AndroidJavaObject>("getApplicationContext");
        }
    }
    
}
