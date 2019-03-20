using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WifiSignalScan
{
    private static WifiSignalScan instance = null;
    private AndroidJavaObject activityContext = null;
    private AndroidJavaClass wifiClass = null;
    private AndroidJavaObject wifiClassInstance = null;
    private AndroidJavaObject pluginClass = null;

    class AndroidPluginCallback : AndroidJavaProxy
    {
        Action<string> resultCallback = null;
        public AndroidPluginCallback(Action<string> callback) : base("com.kakao.wifisignalscan.IPluginCallback")
        {
            resultCallback = callback;
        }

        public void onScanResult(string result)
        {
            if(resultCallback!=null)
            {
                resultCallback(result);
            }            
            Debug.Log($"ENTER callback onSuccess: \n");
            /*
            string[] strs = result.Split('\n');
            foreach(string s in strs)
            {
                Debug.Log(s);
            }
            */
        }        
    }

    public static WifiSignalScan GetInscance()
    {
        if(instance==null)
        {
            instance = new WifiSignalScan();
        }
        return instance;
    }
    private WifiSignalScan()
    {
        activityContext = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        var context = activityContext.GetStatic<AndroidJavaObject>("currentActivity");
        wifiClass = new AndroidJavaClass("com.kakao.wifisignalscan.WifiSignalScan");
        wifiClass.CallStatic("SetContext", context);        
    }
    public void SetCallback(Action<string> callback)
    {
        AndroidPluginCallback c = new AndroidPluginCallback(callback);
        wifiClass.CallStatic("SetResultCallback", c);
    }
    public void StartScan()
    {
        wifiClass.CallStatic("StartScan",1);
    }
    public void StartScanInfinite()
    {
        wifiClass.CallStatic("StartScanInfinite");
    }
    public string GetLastScanData()
    {
        return wifiClass.CallStatic<string>("GetLastScanData");
    }
}
