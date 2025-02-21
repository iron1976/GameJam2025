#if UNITY_EDITOR
using System;
using System.Runtime.InteropServices;

[UnityEditor.InitializeOnLoad]
public class Autosave
{
    [DllImport("User32")]
    private static extern int ShowWindow(int hwnd, int nCmdShow);
    static Autosave()
    {
        UnityEditor.EditorApplication.playmodeStateChanged += () =>
        {  
            if (UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode && !UnityEditor.EditorApplication.isPlaying)
            {
                UnityEditor.SceneManagement.EditorSceneManager.SaveOpenScenes();
                UnityEditor.AssetDatabase.SaveAssets();
                UnityEngine.Debug.Log("Auto-saving all open scenes");
                {
                    //IntPtr swindhandle = System.Diagnostics.Process.Start("E:\\Unityapp\\WARRIOR2D\\update.bat").MainWindowHandle;
                     
                }
            } 
        };
    }
}
#endif