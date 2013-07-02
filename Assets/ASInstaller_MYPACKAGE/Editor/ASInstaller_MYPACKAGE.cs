/*
 * Asset Store Installer
 * 
 * VERY IMPORTANT: Do not test this installer within its own development environment. Its default behaviour is to DELETE ITSELF after running!
 * 
 * Note: This installer will not work for Unity versions under 3.3, as it uses AssetDatabase.ImportPackage.
 * Usage:
 * 1. Import this package into the project that will be uploaded to the Asset Store.
 * 2. Rename MYPACKAGE within all *.cs files to a unique name for your Asset Store package. Be sure to do this with all file names as well.
 * 3. Be sure to edit the included key image and the README.txt.
 * 4. Add any additional pre- and post- callbacks that your package may require.
 *
 * Copyright (c) 2013 Thinksquirrel Software, LLC
 *
 * All rights reserved.
 * 
 * Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
 * 
 * Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
 * 
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, 
 * BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT 
 * SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL 
 * DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) 
 * HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN 
 * ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 *
 */

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
	static bool _silentInstall = false;
	
	// These values control whether or not the actual package import process should be interactive or silent.
	static bool _interactivePackageInstall = false;
	
	// If enabled, installation files will be removed. This will remove the entire base folder.
	// It is highly recommended to keep this enabled, or you will HAVE to handle cleanup manually!
	// Disabling this without a proper cleanup procedure will cause the installation to repeat every time a script is compiled.
	static bool _deleteInstallationFiles = true;
	
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
	internal const string _installerVersionString = "Asset Store Installer v. 1.0.0";
	
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
		
	static bool InstallPackage()
	{
		if (OnPreInstall != null)
			if (!OnPreInstall(_packageName)) return false;
		
		try
		{
			AssetDatabase.ImportPackage(string.Format("{0}.unitypackage", _packageName), _interactivePackageInstall);
		}
		catch
		{
			return false;	
		}
		
		if (OnPostInstall != null)
			return OnPostInstall(_packageName);
	
		return true;
	}
	
	static void DeleteBaseFolder()
	{
		// Some sanity checks
		if (string.IsNullOrEmpty(_baseFolder) || _baseFolder.StartsWith("/") || _baseFolder.StartsWith("\\"))
			return;
		
		FileUtil.DeleteFileOrDirectory(string.Format("Assets/{0}", _baseFolder));
		
		AssetDatabase.Refresh();
	}
	#endregion
}
