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
using System.Collections;

public class ASInstallerWindow_MYPACKAGE : EditorWindow
{
	Texture2D m_KeyImage;
	Rect m_KeyImageRect = new Rect(4, 4, 512, 64);
	Rect m_MainAreaRect = new Rect(4, 72, 512, 324);
	
	TextAsset m_Readme;
	bool m_ViewingReadme = false;
	Vector2 m_ReadmeScrollPosition;
	
	void OnEnable()
	{
		m_KeyImage = Resources.Load("ASInstaller_MYPACKAGE-512x64", typeof(Texture2D)) as Texture2D;
		m_Readme = Resources.Load("README_MYPACKAGE", typeof(TextAsset)) as TextAsset;
		this.minSize = new Vector2(520, 400);
		this.maxSize = new Vector2(520, 400);
		this.position = new Rect(this.position.x, this.position.y, 520, 400);
	}
	
	void OnGUI()
	{
		GUI.DrawTexture(m_KeyImageRect, m_KeyImage);
		
		GUILayout.BeginArea(m_MainAreaRect, GUI.skin.box);
		
		if (m_ViewingReadme)
		{
			m_ReadmeScrollPosition = GUILayout.BeginScrollView(m_ReadmeScrollPosition, false, false, GUILayout.Width(502), GUILayout.Height(292));
			
			GUILayout.Label(m_Readme.text, EditorStyles.wordWrappedLabel);
			
			GUILayout.EndScrollView();
			
			GUILayout.BeginVertical();
			GUILayout.FlexibleSpace();
			
			if (GUILayout.Button("Done", GUILayout.Height(22)))
				m_ViewingReadme = false;
			
			GUILayout.FlexibleSpace();
			GUILayout.EndVertical();
			
		}
		else
		{
			GUILayout.FlexibleSpace();
			
			// Description
			GUILayout.Label(ASInstaller_MYPACKAGE._description, EditorStyles.wordWrappedLabel);
			
			GUILayout.BeginVertical();
			GUILayout.FlexibleSpace();
			// Install (package + example project)
			if (ASInstaller_MYPACKAGE._examplePackage)
				if (GUILayout.Button(string.Format("Install {0} + Examples", ASInstaller_MYPACKAGE._displayName), GUILayout.Height(30)))
			{
				ASInstaller_MYPACKAGE.StartFullInstall();
				this.Close();
			}
			// Install (package only)
			if (GUILayout.Button(string.Format(ASInstaller_MYPACKAGE._examplePackage ? "Install {0} Only" : "Install {0}", ASInstaller_MYPACKAGE._displayName), GUILayout.Height(30)))
			{
				ASInstaller_MYPACKAGE.StartPackageInstall();
				this.Close();
			}
			
			// Install (example project only)
			if (ASInstaller_MYPACKAGE._examplePackage)
				if (GUILayout.Button("Install Examples Only", GUILayout.Height(30)))
			{
				ASInstaller_MYPACKAGE.StartExamplesInstall();
				this.Close();
			}
			
			// View readme
			if (GUILayout.Button("View README", GUILayout.Height(30)))
				m_ViewingReadme = true;
			
			GUILayout.FlexibleSpace();
			
			// Installer version information
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();GUILayout.Label(ASInstaller_MYPACKAGE._installerVersionString, EditorStyles.miniLabel);
			GUILayout.EndHorizontal();
			
			GUILayout.EndVertical();
		}
		
		GUILayout.EndArea();
	}
}
