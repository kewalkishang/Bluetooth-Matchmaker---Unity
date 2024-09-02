using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Android;
using UnityEngine.UI;
using System;

// Class to represent a Bluetooth device with a name and address
[System.Serializable]
public class BluetoothDevice
{
    public string name;
    public string address;
    public BluetoothDevice(string _name, string _address)
    {
        name = _name;
        address = _address;
    }
}

[System.Serializable]
public class Message
{
    public string state;
    public string payload;

    public Message(string _state, string _payload)
    {
        state = _state;
        payload = _payload;
    }
}

public class BLEPlugin : MonoBehaviour
{
    public string SERVICE_UUID = "0000180d-0000-1000-8000-00805f9b34fb";
    public string CHARACTERISTIC_UUID = "00002a37-0000-1000-8000-00805f9b34fb";

    public TMP_Text DebugText;
    public GameObject deviceButtonPrefab;

    public GameObject scanScrollContent;

    public GameObject BLEStatusGO;

    public TMP_InputField inputField; 

    private AndroidJavaObject bluetoothPluginManager;
    private string connectedDevice = "";
    public bool inDebugMode = false;
    private Dictionary<string, GameObject> deviceButtons = new Dictionary<string, GameObject>();

    public static BLEPlugin Instance;


    // Initialize the singleton instance
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            //  Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Start is called before the first frame update 
        PluginLoad();
        InstanceCreation();
        StartCoroutine(PeriodicUpdate());  // Start periodic update coroutine to clear scanned devices
    }

    IEnumerator PeriodicUpdate()
    {
        // Periodically refresh the device list
        while (true)
        {
            yield return new WaitForSeconds(15f);
            //setDebugMsg("Refresh called");
            RefreshDeviceList();
        }
    }

    // Clear existing device buttons
    private void RefreshDeviceList()
    {
        // Clear existing buttons
        foreach (var deviceButton in deviceButtons.Values)
        {
            Destroy(deviceButton);
        }
        deviceButtons.Clear();

    }

    //Is bluetooth enabled.
    public void isBluetoothEnabled()
    {
        try
        {
            using (AndroidJavaClass pluginClass = new AndroidJavaClass("com.onlykk.bleunityplugin.BLEPluginManager"))
            {
                bool isEnabled = pluginClass.Call<bool>("isBluetoothEnabled");
                setDebugMsg("isBluetoothEnabled :" + isEnabled);
                if (!isEnabled)
                {
                    BLEStatusGO.SetActive(true);
                }
            }
        }
        catch (AndroidJavaException ex)
        {
            Debug.LogError("AndroidJavaException caught: " + ex.Message);
        }

    }

    // Set debug message if in debug mode
    void setDebugMsg(string msg)
    {
        if (inDebugMode)
        {
            DebugText.text = DebugText.text + "\n" + msg;
        }
    }

    // Test if the plugin is loaded
    public void PluginLoad()
    {
        try
        {
            using (AndroidJavaClass pluginClass = new AndroidJavaClass("com.onlykk.bleunityplugin.BLEPluginManager"))
            {
                setDebugMsg("Plugin : " + pluginClass.CallStatic<string>("checkPluginLoad"));
            }
        }
        catch (AndroidJavaException ex)
        {
            Debug.LogError("AndroidJavaException caught: " + ex.Message);
        }
    }

    // Create an instance of the plugin manager
    public void InstanceCreation()
    {
        try
        {
            CheckAndRequestPermissions(); // Check and request permissions
            using (AndroidJavaClass pluginClass = new AndroidJavaClass("com.onlykk.bleunityplugin.BLEPluginManager"))
            {
                bluetoothPluginManager = pluginClass.CallStatic<AndroidJavaObject>("getInstance");
                setDebugMsg("Instance Created ");
            }

        }
        catch (AndroidJavaException ex)
        {
            Debug.LogError("AndroidJavaException caught: " + ex.Message);
        }
    }

    // Initialize the Bluetooth plugin
    public void InitBLEPlugin()
    {
        try
        {
            if (bluetoothPluginManager != null)
            {
                bluetoothPluginManager.Call("initBLEPlugin", GetUnityActivity(), SERVICE_UUID, CHARACTERISTIC_UUID);
                setDebugMsg("BLE initiated! ");
            }
            else
            {
                setDebugMsg("BluetoothPluginManager is NULL!");
            }
        }
        catch (AndroidJavaException ex)
        {
            Debug.LogError("AndroidJavaException caught: " + ex.Message);
        }
    }

    // Enable the Bluetooth adapter
    public void EnableBluetoothAdapter()
    {
        try
        {
            if (bluetoothPluginManager != null)
            {
                bluetoothPluginManager.Call("enableBluetoothAdapter");
                setDebugMsg("EnableBluetoothAdapter! ");
            }
            else
            {
                setDebugMsg("BluetoothPluginManager is NULL!");
            }
        }
        catch (AndroidJavaException ex)
        {
            Debug.LogError("AndroidJavaException caught: " + ex.Message);
        }
    }

    // Check and request necessary permissions
    public void CheckAndRequestPermissions()
    {
        string[] permissions = new string[]
    {
        "android.permission.BLUETOOTH",
        "android.permission.BLUETOOTH_ADMIN",
        "android.permission.ACCESS_FINE_LOCATION",
        "android.permission.BLUETOOTH_ADVERTISE",
        "android.permission.BLUETOOTH_CONNECT",
        "android.permission.BLUETOOTH_SCAN"
    };

        List<string> permissionsToRequest = new List<string>();

        foreach (string permission in permissions)
        {
            if (!Permission.HasUserAuthorizedPermission(permission))
            {
                permissionsToRequest.Add(permission);
            }
        }

        if (permissionsToRequest.Count > 0)
        {
            Permission.RequestUserPermissions(permissionsToRequest.ToArray());
        }
    }

    // Start scanning for Bluetooth devices
    public void StartScan()
    {
        try
        {
            bluetoothPluginManager.Call("startScan");
            setDebugMsg("Scan Started!");
        }
        catch (AndroidJavaException ex)
        {
            Debug.LogError("AndroidJavaException caught: " + ex.Message);
        }
    }

    // Start the Bluetooth server
    public void StartServer()
    {
        try
        {
            bluetoothPluginManager.Call("startServer");
            setDebugMsg("Server Started!");
        }
        catch (AndroidJavaException ex)
        {
            Debug.LogError("AndroidJavaException caught: " + ex.Message);
        }
    }


    // Handle device button click event
    public void OnDeviceButtonClick(string deviceAddress)
    {
        setDebugMsg("Trying to connect to: " + deviceAddress);
        try
        {
            bluetoothPluginManager.Call("connectToDevice", deviceAddress);
            setDebugMsg("Connecting to server: " + deviceAddress);
        }
        catch (AndroidJavaException ex)
        {
            Debug.LogError("AndroidJavaException caught: " + ex.Message);
        }
    }

    // Enable the Bluetooth state receiver
    public void EnableBluetoothStateReceiver()
    {
        try
        {
            setDebugMsg("EnableBluetoothStateReceiver: ");
            bluetoothPluginManager.Call("enableBluetoothStateReceiver");

        }
        catch (AndroidJavaException ex)
        {
            Debug.LogError("AndroidJavaException caught: " + ex.Message);
        }
    }

    // Disable the Bluetooth state receiver
    public void DisableBluetoothStateReceiver()
    {
        try
        {
            setDebugMsg("DisableBluetoothStateReceiver: ");
            bluetoothPluginManager.Call("disableBluetoothStateReceiver");

        }
        catch (AndroidJavaException ex)
        {
            Debug.LogError("AndroidJavaException caught: " + ex.Message);
        }

    }
    // Get the Bluetooth device name
    public void GetBluetoothDeviceName()
    {
        string name = "";
        try
        {
            name = bluetoothPluginManager.Call<string>("getBluetoothDeviceName");
            setDebugMsg("Name : " + name );
        }
        catch (AndroidJavaException ex)
        {
            Debug.LogError("AndroidJavaException caught: " + ex.Message);
        }
      
    }

    // Send a message to the client
    public void SendMsgToClient(string msg)
    {
        try
        {
            string userInput = inputField.text;
             setDebugMsg("Sending msg to Client : " + userInput);
            bluetoothPluginManager.Call("sendDataToClient", userInput);
            setDebugMsg("Sent msg to Client : ");
        }
        catch (AndroidJavaException ex)
        {
            Debug.LogError("AndroidJavaException caught: " + ex.Message);
        }

    }

    // Send a message to the server
    public void SendMsgToServer(string msg)
    {
        try
        {
            string userInput = inputField.text;
            setDebugMsg("Sending msg to Server : " + userInput);
            bluetoothPluginManager.Call("sendDataToServer", userInput);
            setDebugMsg("Sent msg to Server : ");
        }
        catch (AndroidJavaException ex)
        {
            Debug.LogError("AndroidJavaException caught: " + ex.Message);
        }

    }


    // Stop scanning for Bluetooth devices
    public void StopScan()
    {
        try
        {
            bluetoothPluginManager.Call("stopScan");
            setDebugMsg("Scan Stopped : ");
        }
        catch (AndroidJavaException ex)
        {
            Debug.LogError("AndroidJavaException caught: " + ex.Message);
        }

    }

    // Test Unity callback to pass as context to the plugin
    private AndroidJavaObject GetUnityActivity()
    {
        using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            return unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        }
    }


    // Convert the game state to JSON
    string ConvertStateToJson(string state, string payload)
    {
        Message msg = new Message(state, payload);
        return JsonUtility.ToJson(msg);
    }

    // Callback to Handle connection state change
    public void onConnectionStateChange(string value)
    {
        setDebugMsg("onConnectionStateChange : " + value);
    }

    // Update the device list UI
    public void UpdateDeviceList(string deviceItemJson)
    {
        BluetoothDevice data = JsonUtility.FromJson<BluetoothDevice>(deviceItemJson);

        string name = data.name;
       
        string value = deviceItemJson;
        if (!deviceButtons.ContainsKey(value))
        {
             setDebugMsg("UpdateDeviceList : "+ deviceItemJson);
            GameObject deviceButton = Instantiate(deviceButtonPrefab, scanScrollContent.transform);
            deviceButton.SetActive(true); // Enable the text object
            TMP_Text buttonText = deviceButton.GetComponentInChildren<TMP_Text>();
            if (buttonText != null)
            {
                buttonText.text = name;
            }
            deviceButton.GetComponent<Button>().onClick.AddListener(() => OnDeviceButtonClick(data.address));
            deviceButtons[value] = deviceButton;
        }
    }

    // Callback to Display an error message
    public void ThrowError(string value)
    {
        setDebugMsg("Error : " + value);
    }

    //Callback to Handle Bluetooth state change
    public void OnBluetoothStateChange(string state)
    {
        setDebugMsg("Bluetooth state changed: " + state);

        // Handle different states
        switch (state)
        {
            case "off":
                BLEStatusGO.SetActive(true);
                // Bluetooth is off
                break;
            case "turning_off":
                // Bluetooth is turning off
                break;
            case "on":
                BLEStatusGO.SetActive(false);
                // Bluetooth is on
                break;
            case "turning_on":
                // Bluetooth is turning on
                break;
        }
    }

    // Callback to Handle device connected event
    public void OnDeviceConnected(string deviceAddress)
    {
        setDebugMsg("Device connected: " + deviceAddress);
        BluetoothDevice data = JsonUtility.FromJson<BluetoothDevice>(deviceAddress);
    }

    // Disconnect the client from the server
    public void DisconnectClient()
    {

        try
        {
            setDebugMsg("Trying to disconnect " + connectedDevice);
            SendMsgToClient("DisconnectClient");
        }
        catch (AndroidJavaException ex)
        {
            Debug.LogError("AndroidJavaException caught: " + ex.Message);
        }
    }

    // Stop the server
    public void stopServer()
    {
        try
        {
            bluetoothPluginManager.Call("stopServer");
            setDebugMsg("Stopping Server");
        }
        catch (AndroidJavaException ex)
        {
            Debug.LogError("AndroidJavaException caught: " + ex.Message);
        }
    }


    // Stop the client
    public void stopClient()
    {
        try
        {
            bluetoothPluginManager.Call("stopClient");
            setDebugMsg("Client stopped!");
        }
        catch (AndroidJavaException ex)
        {
            Debug.LogError("AndroidJavaException caught: " + ex.Message);
        }
    }


    /*
      Callbacks from the BLE Manager
    */
     //Callback to Handle device disconnected event
    public void OnDeviceDisconnected(string deviceAddress)
    {
        setDebugMsg("Device disconnected: " + deviceAddress);

    }

    //Callback to Handle data received from client
    public void OnDataReceivedFromClient(string data)
    {
        setDebugMsg("Data from Client" + data);
        //gridManager.UpdateGridState(data);
    }


    // Callback to Handle Bluetooth state receiver enabled event
    public void OnBluetoothStateReceiverEnabled(string message)
    {
        setDebugMsg("OnBluetoothStateReceiverEnabled : " + message);
    }


    // Callback to Handle Bluetooth state receiver disabled event
    public void OnBluetoothStateReceiverDisabled(string message)
    {
        setDebugMsg("OnBluetoothStateReceiverDisabled : " + message);
    }

    // Callback to Handle data received from server
    public void OnDataReceivedFromServer(string data)
    {
        setDebugMsg("Data from server: " + data);
    }

    // Callback to Handle scan started event 
    public void OnScanStarted(string val)
    {
        setDebugMsg("Scan Started");
    }

    // Callback to Handle BLE plugin initialized event
    public void OnBLEPluginInitialized(string value)
    {
        setDebugMsg("OnBluetoothEnabled : " + value);
    }

    // Callback to Handle Bluetooth enabled event
    public void OnBluetoothEnabled(string value)
    {
        setDebugMsg("OnBluetoothEnabled : " + value);
    }

    // Callback to Handle Bluetooth error event
    public void OnBluetoothError(string value)
    {
        setDebugMsg("OnBluetoothError : " + value);
    }

    // Callback to Handle advertising start success event
    public void OnAdvertisingStartSuccess()
    {
        setDebugMsg("Advertising started successfully.");
    }

    // Callback Handle advertising start failure even
    public void OnAdvertisingStartFailure(string errorCode)
    {
        setDebugMsg("Advertising failed with error code: " + errorCode);
    }

    // Callback to Handle scan failed event
    public void OnScanFailed(string errorCode)
    {
        setDebugMsg("Scan failed with error code: " + errorCode);
    }


}