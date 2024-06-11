//==========================
// - FileName: LogMgr.cs
// - Created: AjieloA
// - CreateTime: 2024-06-11 14:11:15
// - Email: 1758580256@qq.com
// - Description:
//==========================
using System;
using System.IO;
using UnityEngine;

public class LogMgr : MgrBase<LogMgr>
{
    private const bool mEnableLog = true;
    private const bool mSaveLog = true;
    private readonly string mLogFileDir = Application.dataPath.Replace("Assets", "") + "Log";
    private string mLogFileName = "";
    private StreamWriter mLogFileWriter = null;
    private bool mFirstLogTag = true;
    private const int mLogFileCount = 10;// ��ౣ�����־�ļ���
    private const string mLogHead = "> ";


    public void Log(string _msg)
    {
        if (mEnableLog)
            Debug.Log($"{mLogHead}{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}:{_msg}");
        LogToFile(_msg);
    }
    public void Error(string _msg)
    {
        if (mEnableLog)
            Debug.LogError($"{mLogHead}{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}:{_msg}");
        LogToFile(_msg);
    }
    public void Warn(string _msg)
    {
        if (mEnableLog)
            Debug.LogWarning($"{mLogHead}{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}:{_msg}");
        LogToFile(_msg);
    }
    private void LogToFile(string message, bool EnableStack = false)
    {
        if (!mSaveLog)
            return;

        if (mLogFileWriter == null)
        {
            mLogFileName = DateTime.Now.GetDateTimeFormats('s')[0].ToString();
            mLogFileName = mLogFileName.Replace("-", "_");
            mLogFileName = mLogFileName.Replace(":", "_");
            mLogFileName = mLogFileName.Replace(" ", "");
            mLogFileName = mLogFileName.Replace("T", "_");
            mLogFileName = mLogFileName + ".log";
            if (string.IsNullOrEmpty(mLogFileDir))
            {
                try
                {
                    if (!Directory.Exists(mLogFileDir))
                    {
                        Directory.CreateDirectory(mLogFileDir);
                    }
                }
                catch (Exception exception)
                {
                    Debug.Log("> ��ȡ Application.streamingAssetsPath ����" + exception.Message, null);
                    return;
                }
            }
            string path = mLogFileDir + "/" + mLogFileName;

            Debug.Log("Log Path :" + mLogFileDir + "\nLog Name :" + mLogFileName);
            try
            {
                if (!Directory.Exists(mLogFileDir))
                {
                    Directory.CreateDirectory(mLogFileDir);
                }
                mLogFileWriter = File.AppendText(path);
                mLogFileWriter.AutoFlush = true;
            }
            catch (Exception exception2)
            {
                mLogFileWriter = null;
                Debug.Log("LogToCache() " + exception2.Message + exception2.StackTrace, null);
                return;
            }
        }
        if (mLogFileWriter != null)
        {
            try
            {
                if (mFirstLogTag)
                {
                    mFirstLogTag = false;
                    PhoneSystemInfo(mLogFileWriter);
                    this.CheckClearLog();
                }
                mLogFileWriter.WriteLine(message);
                if (EnableStack)
                {
                    //���޹ص�logȥ��
                    var st = StackTraceUtility.ExtractStackTrace();
#if UNITY_EDITOR
                    for (int i = 0; i < 3; i++)
#else
                        for (int i = 0; i < 2; i++)
#endif
                    {
                        st = st.Remove(0, st.IndexOf('\n') + 1);
                    }
                    mLogFileWriter.WriteLine(st);
                }
            }
            catch (Exception)
            {
            }
        }
    }
    public void CheckClearLog()
    {
        if (!Directory.Exists(mLogFileDir))
        {
            return;
        }

        DirectoryInfo direction = new DirectoryInfo(mLogFileDir);
        var files = direction.GetFiles("*");
        if (files.Length >= mLogFileCount)
        {
            var oldfile = files[0];
            var lastestTime = files[0].CreationTime;
            foreach (var file in files)
            {
                if (lastestTime > file.CreationTime)
                {
                    oldfile = file;
                    lastestTime = file.CreationTime;
                }

            }
            oldfile.Delete();
        }

    }
    private static void PhoneSystemInfo(StreamWriter sw)
    {
        sw.WriteLine("*********************************************************************************************************start");
        sw.WriteLine("By " + SystemInfo.deviceName);
        DateTime now = DateTime.Now;
        sw.WriteLine(string.Concat(new object[] { now.Year.ToString(), "��", now.Month.ToString(), "��", now.Day, "��  ", now.Hour.ToString(), ":", now.Minute.ToString(), ":", now.Second.ToString() }));
        sw.WriteLine();
        sw.WriteLine("����ϵͳ:  " + SystemInfo.operatingSystem);
        sw.WriteLine("ϵͳ�ڴ��С:  " + SystemInfo.systemMemorySize);
        sw.WriteLine("�豸ģ��:  " + SystemInfo.deviceModel);
        sw.WriteLine("�豸Ψһ��ʶ��:  " + SystemInfo.deviceUniqueIdentifier);
        sw.WriteLine("����������:  " + SystemInfo.processorCount);
        sw.WriteLine("����������:  " + SystemInfo.processorType);
        sw.WriteLine("�Կ���ʶ��:  " + SystemInfo.graphicsDeviceID);
        sw.WriteLine("�Կ�����:  " + SystemInfo.graphicsDeviceName);
        sw.WriteLine("�Կ���ʶ��:  " + SystemInfo.graphicsDeviceVendorID);
        sw.WriteLine("�Կ�����:  " + SystemInfo.graphicsDeviceVendor);
        sw.WriteLine("�Կ��汾:  " + SystemInfo.graphicsDeviceVersion);
        sw.WriteLine("�Դ��С:  " + SystemInfo.graphicsMemorySize);
        sw.WriteLine("�Կ���ɫ������:  " + SystemInfo.graphicsShaderLevel);
        sw.WriteLine("�Ƿ�ͼ��Ч��:  " + SystemInfo.supportsImageEffects);
        sw.WriteLine("�Ƿ�֧��������Ӱ:  " + SystemInfo.supportsShadows);
        sw.WriteLine("*********************************************************************************************************end");
        sw.WriteLine("LogInfo:");
        sw.WriteLine();
    }
}
