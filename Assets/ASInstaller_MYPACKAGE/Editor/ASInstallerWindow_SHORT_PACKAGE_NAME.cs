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
using UnityEditor;
using UnityEngine;

public sealed class ASInstallerWindow_SHORT_PACKAGE_NAME : EditorWindow
{
    Texture2D m_KeyImage;
    Rect m_KeyImageRect = new Rect(4, 4, 512, 64);
    Rect m_MainAreaRect = new Rect(4, 72, 512, 324);
    TextAsset m_Readme;
    bool m_ViewingReadme;
    Vector2 m_ReadmeScrollPosition;
    
    void OnEnable()
    {
        m_KeyImage = Resources.Load("ASInstaller_SHORT_PACKAGE_NAME-512x64", typeof(Texture2D)) as Texture2D;
        m_Readme = Resources.Load("README_SHORT_PACKAGE_NAME", typeof(TextAsset)) as TextAsset;
        minSize = new Vector2(520, 400);
        maxSize = new Vector2(520, 400);
        position = new Rect(position.x, position.y, 520, 400);
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
            GUILayout.Label(ASInstaller_SHORT_PACKAGE_NAME._description, EditorStyles.wordWrappedLabel);
            
            GUILayout.BeginVertical();
            GUILayout.FlexibleSpace();
            
            // Install
            if (GUILayout.Button(string.Format("Install {0}", ASInstaller_SHORT_PACKAGE_NAME._displayName), GUILayout.Height(30)))
            {
                ASInstaller_SHORT_PACKAGE_NAME.StartInstall();
                Close();
            }

            // Custom GUI
            ASInstaller_SHORT_PACKAGE_NAME.OnInstallGUI_Invoke();
            
            // View readme
            if (m_Readme)
            {
                if (GUILayout.Button("View README", GUILayout.Height(30)))
                    m_ViewingReadme = true;
            }
            
            GUILayout.FlexibleSpace();
            
            // Installer version information
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label(ASInstaller_SHORT_PACKAGE_NAME._installerVersionString, EditorStyles.miniLabel);
            GUILayout.EndHorizontal();
            
            GUILayout.EndVertical();
        }
        
        GUILayout.EndArea();
    }
}
