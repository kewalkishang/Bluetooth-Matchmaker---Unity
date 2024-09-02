using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;
using TMPro;
using System;

public class AndroidPluginTest : MonoBehaviour
{
    public TMP_Text DebugText;
    public GameObject deviceButtonPrefab;
    public GameObject scrollContent;
    private AndroidJavaObject bluetoothPluginManager;
    private string connectedDevice = "";

    void Start()
    {
        testPluginLoad();
    }

    void setDebugMsg(string msg)
    {
        DebugText.text = msg;
    }

    public void testPluginLoad()
    {
        try
        {
            using (AndroidJavaClass pluginClass = new AndroidJavaClass("com.onlykk.androidbluetoothplugin.BluetoothPluginManager"))
            {
                bluetoothPluginManager = pluginClass.CallStatic<AndroidJavaObject>("getInstance");
                setDebugMsg("Plugin Loaded");
            }
        }
        catch (AndroidJavaException ex)
        {
            Debug.LogError("AndroidJavaException caught: " + ex.Message);
        }
    }

    public void testBluetoothSupport()
    {
        try
        {
            bluetoothPluginManager.Call("initBluetoothPlugin", GetUnityActivity());
            bool initResult = bluetoothPluginManager.Call<bool>("isBluetoothSupported");
            setDebugMsg("Bluetooth Supported: " + initResult);
        }
        catch (AndroidJavaException ex)
        {
            Debug.LogError("AndroidJavaException caught: " + ex.Message);
        }
    }

    public void testIsBluetoothEnabled()
    {
        try
        {
            bool initResult = bluetoothPluginManager.Call<bool>("isBluetoothEnabled");
            setDebugMsg("Bluetooth Enabled: " + initResult);
        }
        catch (AndroidJavaException ex)
        {
            Debug.LogError("AndroidJavaException caught: " + ex.Message);
        }
    }

    public void testHasBluetoothPermission()
    {
        // Implement permission checks as needed
        setDebugMsg("Checking permissions...");
    }

    public void teststartDiscovery()
    {
        try
        {
            bool startDiscovery = bluetoothPluginManager.Call<bool>("startDiscoveredDevices");
            setDebugMsg("Discovery started: " + startDiscovery);
        }
        catch (AndroidJavaException ex)
        {
            Debug.LogError("AndroidJavaException caught: " + ex.Message);
        }
    }

    public void testgetDiscoveredDevices()
    {
        try
        {
            string[] discoveredDevices = bluetoothPluginManager.Call<string[]>("getDiscoveredDeviceDetails");
            setDebugMsg("Discovered Devices: " + discoveredDevices.Length);
            foreach (string device in discoveredDevices)
            {
                Debug.Log("Discovered Device: " + device);
                GameObject deviceButton = Instantiate(deviceButtonPrefab, scrollContent.transform);
                deviceButton.SetActive(true); // Enable the text object
                TMP_Text buttonText = deviceButton.GetComponentInChildren<TMP_Text>();
                if (buttonText != null)
                {
                    buttonText.text = device;
                }
                deviceButton.GetComponent<Button>().onClick.AddListener(() => OnDeviceButtonClick(device));
            }
        }
        catch (AndroidJavaException ex)
        {
            Debug.LogError("AndroidJavaException caught: " + ex.Message);
        }
    }

    public void testStartGATTServer()
    {
        try
        {
            bluetoothPluginManager.Call("startGattServer");
            setDebugMsg("GATT Server started");
        }
        catch (AndroidJavaException ex)
        {
            Debug.LogError("AndroidJavaException caught: " + ex.Message);
        }
    }

    public void testBLEScan()
    {
        try
        {
            bluetoothPluginManager.Call("startBLEDiscovery");
            setDebugMsg("BLE Discovery started");
        }
        catch (AndroidJavaException ex)
        {
            Debug.LogError("AndroidJavaException caught: " + ex.Message);
        }
    }

    public void OnDeviceButtonClick(string deviceInfo)
    {
        setDebugMsg("Trying to connect to: " + deviceInfo);
        string[] parts = deviceInfo.Split(new string[] { " - " }, StringSplitOptions.None);
        if (parts.Length > 1)
        {
            string deviceAddress = parts[1];
            try
            {
                bluetoothPluginManager.Call("connectToServer", deviceAddress);
                connectedDevice = deviceAddress;
                setDebugMsg("Connecting to server: " + deviceAddress);
            }
            catch (AndroidJavaException ex)
            {
                Debug.LogError("AndroidJavaException caught: " + ex.Message);
            }
        }
        else
        {
            setDebugMsg("Failed to extract device address from info: " + deviceInfo);
        }
    }

    public void testSendToServer()
    {
        if (!string.IsNullOrEmpty(connectedDevice))
        {
            try
            {
                bluetoothPluginManager.Call("sendMessageToServer", "Hello from client");
                setDebugMsg("Message sent to server: " + connectedDevice);
            }
            catch (AndroidJavaException ex)
            {
                Debug.LogError("AndroidJavaException caught: " + ex.Message);
            }
        }
        else
        {
            setDebugMsg("No devices connected");
        }
    }

    public void testSendToClient()
    {
        try
        {
            bluetoothPluginManager.Call("sendMessageToClient", "Hello from server");
            setDebugMsg("Message sent to client");
        }
        catch (AndroidJavaException ex)
        {
            Debug.LogError("AndroidJavaException caught: " + ex.Message);
        }
    }

    private AndroidJavaObject GetUnityActivity()
    {
        using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            return unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        }
    }
}
