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
			GUILayout.EndVertical();
		}
		
		GUILayout.EndArea();
	}
}
