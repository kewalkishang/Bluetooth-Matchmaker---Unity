using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

public class AndroidPluginTest : MonoBehaviour
{
    // Start is called before the first frame update
     private AndroidJavaObject bluetoothManager;

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

     public void testPluginLoad()
    {
        Debug.Log("Testing Plugin Load:");
        try
        {
            using (AndroidJavaClass pluginClass = new AndroidJavaClass("com.onlykk.androidbluetoothplugin.BluetoothManager"))
            {
                Debug.Log("AndroidJavaClass created...");

                if (pluginClass != null)
                {
                    string message = pluginClass.CallStatic<string>("checkPluginLoad");
                    Debug.Log("Message received: " + message);
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
             using (AndroidJavaClass pluginClass = new AndroidJavaClass("com.onlykk.androidbluetoothplugin.BluetoothManager"))
            {
                // Get the instance of the BluetoothManager
                bluetoothManager = pluginClass.CallStatic<AndroidJavaObject>("getInstance");
            }
            bluetoothManager.Call("initBluetoothAdapter", GetUnityActivity());
            bool initResult = bluetoothManager.Call<bool>("isBluetoothSupported");
            Debug.Log("Bluetooth Adapter Initialized: " + initResult);

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
            bool initResult = bluetoothManager.Call<bool>("isBluetoothEnabled");
            Debug.Log("Is Bluetooth Enabled : " + initResult);

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
            
    }


     public void testinitializeReceiver()
    {
         Debug.Log("Testing is initializeReceiver :");
        try
        {
            bluetoothManager.Call("initializeReceiver");
            Debug.Log("initializeReceiver : ");

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


    private IEnumerator CheckDiscoveredDevices()
    {
        yield return new WaitForSeconds(10); // Wait for 10 seconds to discover devices

        string[] discoveredDevices = bluetoothManager.Call<string[]>("getDiscoveredDevices");

        foreach (string device in discoveredDevices)
        {
            Debug.Log("Discovered Device: " + device);
        }
    }


     public void testgetDiscoveredDevices()
    {
         Debug.Log("Testing is getDiscoveredDevices :");
        try
        {
            //List<string> devices = bluetoothManager.Call<List<string>>("getDiscoveredDevices");
            //string combinedString = string.Join( ",", devices.ToArray() );
            //Debug.Log("getDiscoveredDevices : " + combinedString );
            //StartCoroutine(CheckDiscoveredDevices());
            string[] discoveredDevices = bluetoothManager.Call<string[]>("getDiscoveredDevices");
 Debug.Log("Discovered Device: " + discoveredDevices.Length);
        foreach (string device in discoveredDevices)
        {
            Debug.Log("Discovered Device: " + device);
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



    private AndroidJavaObject GetUnityActivity()
    {
        using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            return unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        }
    }
    
}
