using UnityEngine;
using System.IO;

public class LogManager : MonoBehaviour
{
    public static LogManager Instance { get; private set; }
    private string logFilePath;

    void Awake()
    {
        Instance = this;
        logFilePath = Application.dataPath + "/savunma_gunlugu.txt";
        File.WriteAllText(logFilePath, "--- Simülasyon Günlüğü Başlangıcı ---\n");
    }

    public void WriteLog(string message)
    {
        string logEntry = $"[{Time.time:F2}sn] {message}\n";
        Debug.Log(logEntry);

        File.AppendAllText(logFilePath, logEntry);
    }
}