# Unity Bluetooth Low Energy (BLE) Plugin
This Unity project demonstrates how to integrate a custom Bluetooth Low Energy (BLE) plugin, packaged as an .aar file, into a Unity project. The plugin enables Unity to communicate with BLE devices on Android, allowing for connection management, data transfer, and other BLE functionalities. This project includes a sample scene to demonstrate basic BLE operations.


## Features :
* BLE initialization & Management
* Listen for Bluetooth state changes
* Scan for discoverable Devices
* Start a Gatt Server
* Start a Gatt Client
* Connect to a server and transfer data between them

## How to use, Checklist : 
* Integrate with Unity : Copy the .aar file into the Plugins > Android of your Unity Project
* Ensure your plugin's platform is set to android in the import settings
* Ensure you are requesting for all the permission in your project - 
  - android.permission.BLUETOOTH
  - android.permission.BLUETOOTH_ADMIN
  - android.permission.ACCESS_FINE_LOCATION
  - android.permission.BLUETOOTH_ADVERTISE
  - android.permission.BLUETOOTH_CONNECT
  - android.permission.BLUETOOTH_SCAN
* Have a GameObject with the name "BLEPlugin" in your scene to receive the callbacks. (The name of your Script doesn't matter, only the name of the method does)
* Check out the sample scene (Android BLR Plugin Testbed) for reference.

## Methods :

| Method Name                      | Description                                                               | Parameters                                                                                         | Return Value |
|----------------------------------|---------------------------------------------------------------------------|----------------------------------------------------------------------------------------------------|--------------|
| `checkPluginLoad()`              | Checks if the plugin is loaded successfully.                              | None                                                                                               | `String`     |
| `getInstance()`                  | Returns the singleton instance of `BLEPluginManager`.                     | None                                                                                               | `BLEPluginManager` |
| `initBLEPlugin(Context context, String SERVICE_UUID_HOST, String CHARACTERISTIC_UUID_HOST)` | Initializes the BLE plugin with the given context and UUIDs.            | `context`: `Context` - Application context. <br> `SERVICE_UUID_HOST`: `String` - Service UUID. <br> `CHARACTERISTIC_UUID_HOST`: `String` - Characteristic UUID. | `void`       |
| `enableBluetoothAdapter()`       | Enables the Bluetooth adapter if it is not already enabled.               | None                                                                                               | `void`       |
| `isBluetoothSupported()`         | Checks if Bluetooth is supported on the device.                           | None                                                                                               | `boolean`    |
| `isBluetoothEnabled()`           | Checks if Bluetooth is enabled on the device.                             | None                                                                                               | `boolean`    |
| `getBluetoothDeviceName()`       | Returns the name of the user's Bluetooth device.                          | None                                                                                               | `String`     |
| `enableBluetoothStateReceiver()` | Registers a BroadcastReceiver to listen for Bluetooth state changes.      | None                                                                                               | `void`       |
| `disableBluetoothStateReceiver()`| Unregisters the Bluetooth state BroadcastReceiver.                        | None                                                                                               | `void`       |
| `startServer()`                  | Starts the GATT server and begins advertising.                            | None                                                                                               | `void`       |
| `stopServer()`                   | Stops the GATT server and advertising.                                    | None                                                                                               | `void`       |
| `startScan()`                    | Starts scanning for BLE devices.                                          | None                                                                                               | `void`       |
| `stopScan()`                     | Stops the BLE device scan.                                                | None                                                                                               | `void`       |
| `connectToDevice(String deviceAddress)` | Connects to a specified BLE device using its address.                  | `deviceAddress`: `String` - The address of the device to connect to.                                | `void`       |
| `stopClient()`                   | Stops the GATT client.                                                    | None                                                                                               | `void`       |
| `sendDataToServer(String data)`  | Sends data to the GATT server.                                            | `data`: `String` - The data to send.                                                                | `void`       |
| `sendDataToClient(String data)`  | Sends data to all connected clients.                                      | `data`: `String` - The data to send.                                                                | `void`       |

## Callbacks

| Callback Method                          | Description                                               | Parameters                                |
|------------------------------------------|-----------------------------------------------------------|----------------------------------------|
| `OnBLEPluginInitialized(String message)` | Called when the BLE plugin is initialized.                | `message`: `String` - A message indicating the result of the initialization. Example: `"BluetoothAdapter Initialized!"` or `"BluetoothAdapter is Null!"`. |
| `OnBluetoothEnabled(String message)`     | Called when the Bluetooth adapter is successfully enabled. | `message`: `String` - A message indicating the status. Example: `"Enabled programmatically"` or `"Already enabled"`. |
| `OnBluetoothError(String message)`       | Called when there is an error enabling Bluetooth.          | `message`: `String` - An error message indicating what went wrong. Example: `"Failed to enable"`. |
| `OnBluetoothStateReceiverEnabled(String status)` | Called when the Bluetooth state receiver is enabled or failed to enable. | `status`: `String` - Status message indicating success (`"ENABLED"`) or failure (`"FAILED"`). |
| `OnBluetoothStateReceiverDisabled(String status)` | Called when the Bluetooth state receiver is disabled or failed to disable. | `status`: `String` - Status message indicating success (`"SUCCESS"`) or failure (`"FAILED"`). |
| `OnDeviceConnected(String deviceInfo)`   | Called when a BLE device is successfully connected.        | `deviceInfo`: `String` - JSON string containing information about the connected device (e.g., name, address). |
| `OnDeviceDisconnected(String deviceInfo)` | Called when a BLE device is disconnected.                  | `deviceInfo`: `String` - JSON string containing information about the disconnected device (e.g., name, address). |
| `OnDataReceivedFromClient(String data)`  | Called when data is received from a connected client.      | `data`: `String` - The data received from the client. |
| `OnDataReceivedFromServer(String data)`  | Called when data is received from the BLE server.          | `data`: `String` - The complete data received from the server. |
| `OnScanStarted()`                        | Called when BLE device scanning starts.                    | None                                      |
| `OnScanFinished()`                       | Called when BLE device scanning ends.                      | None                                      |
| `OnScanFailed(int errorCode)`            | Called when BLE device scanning fails.                     | `errorCode`: `int` - An error code indicating the reason for the scan failure. |
| `OnAdvertisingStartSuccess(String serviceUuid)` | Called when BLE advertising starts successfully.          | `serviceUuid`: `String` - The UUID of the advertised service. |
| `OnAdvertisingStartFailure(String errorCode)` | Called when BLE advertising fails to start.               | `errorCode`: `String` - The error code indicating why advertising failed. |
| `OnBluetoothStateChange(String state)`   | Called when the Bluetooth state changes.                  | `state`: `String` - The new state of Bluetooth (`"off"`, `"turning_off"`, `"on"`, `"turning_on"`). |
| `onConnectionStateChange(String message)` | Called when there is a change in the connection state.     | `message`: `String` - A string containing the new state and status of the connection. |
| `OnDeviceDisconnected(String deviceItem)` | Called when a BLE device is disconnected after receiving a disconnect command. | `deviceItem`: `String` - JSON string containing information about the disconnected device. |
| `OnDataReceivedFromServer(String completeMessage)` | Called when the complete data is received from the BLE server. | `completeMessage`: `String` - The data received after assembling all chunks. |

## Extending the Plugin :
If you want to build upon the existing plugin look into the, android implementation at - <br>
https://github.com/kewalkishang/BLE-Unity-Plugin


## Future Plans : 
- [ ] BLE Plugin for iOS
- [ ] Handle MTU better for sending larger chunks of data
- [ ] Extended support for 2+ devices to seamlessly connect together
- [ ] Data Sync feature (Maybe using Merkle trees)

