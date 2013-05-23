// Note: This installer will not work for Unity versions under 3.3, as it uses AssetDatabase.ImportPackage.

// Usage:
// 1. Rename MYPACKAGE within all *.cs files to a unique name for your Asset Store package. Be sure to do this with all file names as well.
// 2. Be sure to edit the included key image and the README.txt.
// 3. Add any additional pre- and post- callbacks that your package may require.

using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public static class ASInstaller_MYPACKAGE
{

	#region Settings - Edit these
	// The display name for the package, shown to the user.
	internal static string _displayName = "MYPACKAGE";
	
	// A short description for interactive installation.
	internal static string _description = "This wizard will install MYPACKAGE for your Unity version into your project. Please select one of the following options to continue.";
	
	// The base folder under which all .unitypackage files should be placed
	// THIS FOLDER SHOULD BE UNIQUE, and not the same folder that the package is installed into!
	const string _baseFolder = "ASInstaller_MYPACKAGE";
	
	// If enabled, install proceeds silently without any user intervention needed. 
	// Any example packages will also automatically be installed
	static bool _silentInstall = false;
	
	// These values control whether or not the actual package import process should be interactive or silent.
	static bool _interactivePackageInstall = false;
	static bool _interactiveExamplePackageInstall = true;
	
	// If enabled, installation files will be removed. This will remove the entire base folder.
	// It is highly recommended to keep this enabled, or you will HAVE to handle cleanup manually!
	// Disabling this without a proper cleanup procedure will cause the installation to repeat every time a script is compiled.
	static bool _deleteInstallationFiles = true;
	
	// If enabled, a seperate .unitypackage will be installed with example scripts, if the user desires.
	// It is recommended to put all files in one package if silent install is enabled.
	internal static bool _examplePackage = true;
	
	// The name of the .unitypackage (without extension), by version.
	// Includes a fallback version (should contain the latest version of your package).
	// Feel free to simplify this list depending on your product offering across Unity versions.
	// Always make sure to include the fallback package, or the installer will not work on newer Unity versions!
	// If a specific version is not supported (and above the minimum uploaded version on the Asset Store),
	// use the string "!UNSUPPORTED".
	const string _packageName =
#if UNITY_3_3
		"MYPACKAGE-3.3"
#elif UNITY_3_4_0
		"MYPACKAGE-3.4"
#elif UNITY_3_4_1
		"MYPACKAGE-3.4.1"
#elif UNITY_3_4_2
		"MYPACKAGE-3.4.2"		
#elif UNITY_3_5_0
		"MYPACKAGE-3.5"	
#elif UNITY_3_5_1
		"MYPACKAGE-3.5.1"	
#elif UNITY_3_5_2
		"MYPACKAGE-3.5.2"
#elif UNITY_3_5_3
		"MYPACKAGE-3.5.3"
#elif UNITY_3_5_4
		"MYPACKAGE-3.5.4"
#elif UNITY_3_5_5
		"MYPACKAGE-3.5.5"
#elif UNITY_3_5_6
		"MYPACKAGE-3.5.6"		
#elif UNITY_3_5_7
		"MYPACKAGE-3.5.7"		
#elif UNITY_4_0_0
		"MYPACKAGE-4.0.0"
#elif UNITY_4_0_1
		"MYPACKAGE-4.0.1"		
#elif UNITY_4_1_0
		"MYPACKAGE-4.1.0"
#elif UNITY_4_1_2
		"MYPACKAGE-4.1.2"
#else
		"MYPACKAGE-4.1.2"
#endif
			;
	
	const string _examplePackageName = 
#if UNITY_3_3
		"MYPACKAGE-Examples-3.3"
#elif UNITY_3_4_0
		"MYPACKAGE-Examples-3.4"
#elif UNITY_3_4_1
		"MYPACKAGE-Examples-3.4.1"
#elif UNITY_3_4_2
		"MYPACKAGE-Examples-3.4.2"		
#elif UNITY_3_5_0
		"MYPACKAGE-Examples-3.5"	
#elif UNITY_3_5_1
		"MYPACKAGE-Examples-3.5.1"	
#elif UNITY_3_5_2
		"MYPACKAGE-Examples-3.5.2"
#elif UNITY_3_5_3
		"MYPACKAGE-Examples-3.5.3"
#elif UNITY_3_5_4
		"MYPACKAGE-Examples-3.5.4"
#elif UNITY_3_5_5
		"MYPACKAGE-Examples-3.5.5"
#elif UNITY_3_5_6
		"MYPACKAGE-Examples-3.5.6"		
#elif UNITY_3_5_7
		"MYPACKAGE-Examples-3.5.7"		
#elif UNITY_4_0_0
		"MYPACKAGE-Examples-4.0.0"
#elif UNITY_4_0_1
		"MYPACKAGE-Examples-4.0.1"		
#elif UNITY_4_1_0
		"MYPACKAGE-Examples-4.1.0"
#elif UNITY_4_1_2
		"MYPACKAGE-Examples-4.1.2"
#else
		"MYPACKAGE-Examples-4.1.2"
#endif	
			;		
	#endregion
	
	#region Callbacks
	// This callback runs before installation. Feel free to subscribe to this event from any editor script.
	// param: string - the name of the package that will be installed
	// returns: bool - whether or not the action was successful. If false, installation will abort.
	static event System.Func<string, bool> OnPreInstall;
	
	// This callback runs after installation, but before any file deletion. Feel free to subscribe to this event from any editor script.
	// param: string - the name of the package that was installed
	// returns: bool - whether or not the action was successful. If false, installation will abort.
	static event System.Func<string, bool> OnPostInstall;
	#endregion
				
	
	#region Internal stuff					
	// Runs on compile, immediately after installing from the Asset Store
	static ASInstaller_MYPACKAGE()
	{		
		if (_silentInstall)
		{
			StartFullInstall();
		}
		else
		{
			// Open the editor window for interactive installation
			EditorWindow.GetWindow<ASInstallerWindow_MYPACKAGE>(true, string.Format("{0} Installer", _displayName), true);
		}
	}
	
	static int __eventID;
	static float __lastEventTime;
	const float __delay = 3f;
	
	internal static void StartFullInstall()
	{
		EditorApplication.update += UpdateInstallFull;
	}
	
	internal static void StartPackageInstall()
	{
		EditorApplication.update += UpdateInstallPackage;
	}
	
	internal static void StartExamplesInstall()
	{
		EditorApplication.update += UpdateInstallExamples;
	}
	
	// Due to how Unity implements ImportPackage, package import needs to happen over a period of time, with a delay in between (or all packages will not import)
	// Unity also seems to not throw any exceptions when package import fails. :(
	static void UpdateInstallFull()
	{
		if (Time.realtimeSinceStartup > __lastEventTime + __delay)
		{
			switch(__eventID)
			{
				case 0:
					if (!InstallPackage())
					{
						EditorApplication.update -= UpdateInstallFull;
						Debug.LogError("Error during installation!");
					}
					break;
				case 1:
					if (_examplePackage)
					{
						if (!InstallExamples())
						{
							EditorApplication.update -= UpdateInstallFull;
							Debug.LogError("Error during installation!");
						}
					}
					else if (_deleteInstallationFiles)
					{
						DeleteBaseFolder();
					}
					else
					{
						EditorApplication.update -= UpdateInstallFull;
					}
					break;
				case 2:
					if (_deleteInstallationFiles)
					{
						DeleteBaseFolder();
					}
					else
					{
						EditorApplication.update -= UpdateInstallFull;
					}
					break;
				default:
					EditorApplication.update -= UpdateInstallFull;
					break;
			}
			
			__eventID++;
			__lastEventTime = Time.realtimeSinceStartup;
		}
	}
	
	static void UpdateInstallPackage()
	{
		if (Time.realtimeSinceStartup > __lastEventTime + __delay)
		{
			switch(__eventID)
			{
				case 0:
					if (!InstallPackage())
					{
						EditorApplication.update -= UpdateInstallPackage;
						Debug.LogError("Error during installation!");
					}
					break;
				case 1:
					if (_deleteInstallationFiles)
					{
						DeleteBaseFolder();
					}
					else
					{
						EditorApplication.update -= UpdateInstallPackage;
					}
					break;
				default:
					EditorApplication.update -= UpdateInstallPackage;
					break;
			}
			
			__eventID++;
			__lastEventTime = Time.realtimeSinceStartup;
		}
	}
	
	static void UpdateInstallExamples()
	{
		if (Time.realtimeSinceStartup > __lastEventTime + __delay)
		{
			switch(__eventID)
			{
				case 0:
					if (_examplePackage)
					{
						if (!InstallExamples())
						{
							EditorApplication.update -= UpdateInstallExamples;
							Debug.LogError("Error during installation!");
						}
					}
					break;
				case 1:
					if (_deleteInstallationFiles)
					{
						DeleteBaseFolder();
					}
					else
					{
						EditorApplication.update -= UpdateInstallExamples;
					}
					break;
				default:
					EditorApplication.update -= UpdateInstallExamples;
					break;
			}
			
			__eventID++;
			__lastEventTime = Time.realtimeSinceStartup;
		}
	}
	
	static bool InstallPackage()
	{
		if (OnPreInstall != null)
			if (!OnPreInstall(_packageName)) return false;
		
		try
		{
			//AssetDatabase.ImportPackage(string.Format("{0}.unitypackage", _packageName), _interactivePackageInstall);
		}
		catch
		{
			return false;	
		}
		
		if (OnPostInstall != null)
			return OnPostInstall(_packageName);
	
		return true;
	}
	
	static bool InstallExamples()
	{
		if (OnPreInstall != null)
			if (!OnPreInstall(_examplePackageName)) return false;
	
		try
		{
			//AssetDatabase.ImportPackage(string.Format("{0}.unitypackage", _examplePackageName), _interactiveExamplePackageInstall);
		}
		catch
		{
			return false;
		}
		
		if (OnPostInstall != null)
			return OnPostInstall(_examplePackageName);
	
		return true;
	}
	
	static void DeleteBaseFolder()
	{
		// Some sanity checks
		if (string.IsNullOrEmpty(_baseFolder) || _baseFolder.StartsWith("/") || _baseFolder.StartsWith("\\"))
			return;
		
		FileUtil.DeleteFileOrDirectory(string.Format("Assets/{0}", _baseFolder));
	}
	#endregion
}
