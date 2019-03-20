using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gyro : MonoBehaviour {
    private bool gyroEnabled;
    private Compass compass;
    private Gyroscope gyro;
    private Quaternion rot;
    private Quaternion fix;
    public GameObject cameraContainer;
    public Text magneticText;
    public Text gyroText;
    public Text wifiText;
    WifiSignalScan wifiScan;


    // Use this for initialization
    void Start () {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        gyroEnabled = EnableGyro();
        wifiScan=WifiSignalScan.GetInscance();
                
        wifiScan.SetCallback(WifiResultHandler);
        //wifiScan.StartScan();
        StartCoroutine("WifiScanThread");
    }
    public void WifiResultHandler(string result)
    {
        wifiText.text = $"test wifi : {result}";
        //wifiScan.StartScan();
        //wifiText.text = $"test wifi : {wifiScan.GetLastScanData()}";

    }
    private bool EnableGyro()
    {
        if (SystemInfo.supportsGyroscope)
        {
            gyro = Input.gyro;
            compass = Input.compass;
            gyro.enabled = true;
            compass.enabled = true;
            fix = Quaternion.Euler(0,0,-90);
            rot = new Quaternion(0, 0, 0,1);
            return true;
        }
        return false;
    }
	
	// Update is called once per frame
	void Update () {

        if(gyroEnabled)
        {
            magneticText.text = "magnetic x: " + compass.rawVector.x + " y: " + compass.rawVector.y + " z: " + compass.rawVector.z;                        
            //cameraContainer.transform.localRotation = gyro.attitude * rot * fix;            
            //cameraContainer.transform.localRotation = new Quaternion(tempQ.z, tempQ.y, tempQ.x, tempQ.w) * fix;           

            //cameraContainer.transform.localRotation = Quaternion.Euler(-tempQ.eulerAngles.y+90, -tempQ.eulerAngles.x, tempQ.eulerAngles.z-90);
            //cameraContainer.transform.localRotation = new Quaternion(0.5f, 0.5f, -0.5f, 0.5f) * gyro.attitude * new Quaternion(0,0,1,0);
            cameraContainer.transform.localRotation = DeviceRotation.Get();
            gyroText.text = "gyro x: " + cameraContainer.transform.localRotation.eulerAngles.x + " y: " + cameraContainer.transform.localRotation.eulerAngles.y + " z: " + cameraContainer.transform.localRotation.eulerAngles.z;
            //wifiText.text = $"test wifi : {wifiScan.GetLastScanData()}";
            //Debug.Log($"test wifi : {wifiScan.GetLastScanData()}" );
        }        
    }
    IEnumerator WifiScanThread()
    {        
        while (true)
        {
            wifiScan.StartScan();
            yield return new WaitForSeconds(30.0f);
        }
    }
}
