using UnityEngine;
using System.IO;
using System.Diagnostics;
using System;

public class ShutdownBatExecutor : MonoBehaviour
{
    [Header("Configuración de Trigger")]
    [Tooltip("Tag del objeto que activará el apagado")]
    public string triggerTag = "Player";

    [Header("Configuración de Apagado")]
    [Tooltip("Tiempo de espera antes del apagado (segundos)")]
    public int shutdownDelay = 0;
    
    [Tooltip("Mostrar mensaje de advertencia en pantalla")]
    public bool showWarning = true;
    
    private bool shutdownTriggered = false;
    private float shutdownTime = 0f;

    void Update()
    {
        // Manejar el temporizador de apagado
        if (shutdownTriggered && shutdownTime > 0 && Time.time > shutdownTime)
        {
            ExecuteShutdown();
            shutdownTriggered = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!shutdownTriggered && other.CompareTag(triggerTag))
        {
            TriggerShutdown();
        }
    }

    private void TriggerShutdown()
    {
        shutdownTriggered = true;
        
        // Programar el apagado
        if (shutdownDelay > 0)
        {
            shutdownTime = Time.time + shutdownDelay;
        }
        else
        {
            ExecuteShutdown();
        }
    }

    private void ExecuteShutdown()
    {
        // Verificar compatibilidad con Windows
        if (!IsWindowsPlatform())
        {
            return;
        }

        // Crear y ejecutar el comando de apagado
        try
        {
            if (Application.isEditor)
            {
                // Método para Editor
                ExecuteInEditor();
            }
            else
            {
                // Método para build final
                ExecuteInBuild();
            }
        }
        catch (Exception e)
        {
        }
    }

    private void ExecuteInEditor()
    {
        #if UNITY_EDITOR
        // Crear archivo .bat temporal
        string batPath = Path.Combine(Application.temporaryCachePath, "ShutdownPC.bat");
        File.WriteAllText(batPath, "@echo off\nshutdown /s /t 0\n");

        // Ejecutar el archivo .bat
        ProcessStartInfo startInfo = new ProcessStartInfo()
        {
            FileName = batPath,
            WindowStyle = ProcessWindowStyle.Hidden,
            CreateNoWindow = true,
            UseShellExecute = true
        };

        Process.Start(startInfo);
        #endif
    }

    private void ExecuteInBuild()
    {
        // Ejecutar directamente sin archivo .bat
        ProcessStartInfo startInfo = new ProcessStartInfo()
        {
            FileName = "cmd.exe",
            Arguments = "/C rundll32.exe powrprof.dll,SetSuspendState 0,1,0",
            WindowStyle = ProcessWindowStyle.Hidden,
            CreateNoWindow = true,
            UseShellExecute = false
        };

        Process.Start(startInfo);
    }

    private bool IsWindowsPlatform()
    {
        return Application.platform == RuntimePlatform.WindowsPlayer || 
               Application.platform == RuntimePlatform.WindowsEditor;
    }

    private void OnGUI()
    {
        if (shutdownTriggered && showWarning)
        {
            GUI.color = Color.red;
            GUI.Box(new Rect(Screen.width/2 - 200, 20, 400, 60), "¡ADVERTENCIA!");
            GUI.color = Color.white;
            
            string message = (shutdownDelay > 0) ? 
                $"El sistema se apagará en {Mathf.CeilToInt(shutdownTime - Time.time)} segundos" : 
                "¡El sistema se apagará inmediatamente!";
            
            GUI.Label(new Rect(Screen.width/2 - 190, 50, 380, 30), message);
        }
    }
}
