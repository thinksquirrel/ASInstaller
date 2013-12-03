/*
 * Asset Store Installer
 *
 * This is an example script with additional options that you can add to the installer
 * 
 * VERY IMPORTANT: Do not test this installer within its own development environment.
 * Its default behaviour is to DELETE ITSELF after running!
 * You can define ASINSTALLER_DEVELOPMENT in order to prevent the installer window from showing up on load.
 * 
 * Note: This installer will not work for Unity versions under 3.3, as it uses AssetDatabase.ImportPackage.
 * Usage:
 * 1. Import this package into the project that will be uploaded to the Asset Store.
 * 2. In all files/file names, rename the following:
 *      * SHORT_PACKAGE_NAME - A short name for your package to be used in variable and file names
 *      * FULL_PACKAGE_NAME - A full, human readable name for your package
 * 4. Be sure to edit the included key image and the README.txt.
 * 5. Add any additional pre- and post- callbacks that your package may require.
 *
 */
using UnityEngine;
using UnityEditor;
using System.IO;

#pragma warning disable 162
#if !ASINSTALLER_DEVELOPMENT
[InitializeOnLoad]
#endif
public static class ASInstaller_SHORT_PACKAGE_NAME
{
    #region Settings - Edit these
    // The display name for the package, shown to the user.
    internal static string _displayName = "FULL_PACKAGE_NAME";
    
    // A short description for interactive installation.
    internal static string _description = string.Format("This wizard will install FULL_PACKAGE_NAME for your Unity version into your project. Please select one of the following options to continue.\n\nIf you exit this installer early, you can re-run it by going to {0}.", _menuItem.Replace("/", " > "));
    
    // Menu item, in case the installation window is closed.
    internal const string _menuItem = "Window/FULL_PACKAGE_NAME/Install FULL_PACKAGE_NAME";
    internal const int _menuItemPosition = 10000;

    // The base folder under which all .zip files should be placed
    // THIS FOLDER SHOULD BE UNIQUE, and not the same folder that the package is installed into!
    const string _baseFolder = "ASInstaller_SHORT_PACKAGE_NAME";
    
    // If enabled, install proceeds silently without any user intervention needed. 
    const bool s_SilentInstall = false;
        
    // If enabled, installation files will be removed. This will remove the entire base folder.
    // It is highly recommended to keep this enabled, or you will HAVE to handle cleanup manually!
    // Disabling this without a proper cleanup procedure will cause the installation to repeat every time a script is compiled.
    const bool s_DeleteInstallationFiles = true;
    
    // The name of the package (without extension), by version.
    // Includes a fallback version (should contain the latest version of your package).
    // Feel free to simplify or expand this list depending on your product offering across Unity versions.
    // Always make sure to include the fallback package, or the installer will not work on newer Unity versions!
    const string _packageName =    
#if UNITY_3_5_7
        "SHORT_PACKAGE_NAME-Unity-3.5.7"       
#elif UNITY_4_0 || UNITY_4_1 || UNITY_4_2
        "SHORT_PACKAGE_NAME-Unity-4.0.0"
#elif UNITY_4_3
        "SHORT_PACKAGE_NAME-Unity-4.3.0"       
#else
        "SHORT_PACKAGE_NAME-Unity-4.3.0"
#endif
        ;   
    #endregion
    
    #region Callbacks
    // This callback runs before installation. Feel free to subscribe to this event from any editor script.
    // param: string - the name of the package that will be installed
    // returns: bool - whether or not the action was successful. If false, installation will abort.
    public static event System.Func<string, bool> OnPreInstall;
    
    // This callback runs after installation, but before any file deletion. Feel free to subscribe to this event from any editor script.
    // param: string - the name of the package that was installed
    // returns: bool - whether or not the action was successful. If false, installation will abort.
    public static event System.Func<string, bool> OnPostInstall;

    // This callback runs in the GUI, between the description text and buttons. Use this to insert your own GUI elements.
    // Note that there is limited space to put elements.
    public static event System.Action OnInstallGUI;
    #endregion
                    
    #region Internal stuff
    internal const string _installerVersionString = "ASInstaller v. 1.1.0";
    
    // Runs on compile, immediately after installing from the Asset Store
    static ASInstaller_SHORT_PACKAGE_NAME()
    {       
        if (s_SilentInstall)
        {
            StartInstall();
        }
        else
        {
            EditorApplication.update += GetWindowDelayed;
        }
    }
#if !ASINSTALLER_DEVELOPMENT
    [MenuItem(_menuItem, false, _menuItemPosition)]
#endif
    static void GetWindow()
    {
        // Open the editor window for interactive installation
        EditorWindow.GetWindow<ASInstallerWindow_SHORT_PACKAGE_NAME>(true, string.Format("{0} Installer", _displayName), true);
    }

    static int s_CurrentFrame;

    static void GetWindowDelayed()
    {
        if (s_CurrentFrame++ > 10)
        {
            s_CurrentFrame = 0;
            EditorApplication.update -= GetWindowDelayed;
            GetWindow();
        }
    }
    
    internal static void OnInstallGUI_Invoke()
    {
        if (OnInstallGUI != null)
            OnInstallGUI();
    }

    internal static void StartInstall()
    {
        // OnPreInstall
        if (OnPreInstall != null)
        {
            if (!OnPreInstall(_packageName))
            {
                Debug.LogError(string.Format("Unable to install {0}! Error: OnPreInstall failure", _packageName));
                return;
            }
        }
    
        try
        {
            // Extract file
            var di = new DirectoryInfo(Application.dataPath);
            var p = Path.Combine(di.FullName, Path.Combine(_baseFolder, _packageName + ".unitypackage"));

            AssetDatabase.ImportPackage(p, false);
        }
        catch
        {
            Debug.LogError(string.Format("Unable to install {0}! Error: Package import failure", _packageName));
            return;
        }
    
        // OnPostInstall
        if (OnPostInstall != null)
        {
            if (!OnPostInstall(_packageName))
            {
                Debug.LogError(string.Format("Unable to install {0}! Error: OnPostInstall failure", _packageName));
                return;
            }
        }

        // Delete base folder
        if (s_DeleteInstallationFiles)
            DeleteBaseFolder();
    }

    static void DeleteBaseFolder()
    {
        // Some sanity checks
        if (string.IsNullOrEmpty(_baseFolder) || _baseFolder.StartsWith("/", System.StringComparison.Ordinal) || _baseFolder.StartsWith("\\", System.StringComparison.Ordinal))
            return;
        
        FileUtil.DeleteFileOrDirectory(string.Format("Assets/{0}", _baseFolder));
        if (File.Exists(string.Format("Assets/{0}.meta", _baseFolder)))
        {
            FileUtil.DeleteFileOrDirectory(string.Format("Assets/{0}.meta", _baseFolder));
        }

        AssetDatabase.Refresh();
    }
    #endregion
}