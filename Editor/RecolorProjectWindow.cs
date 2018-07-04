// ===============================
// AUTHOR          : J. Giezendanner
// CREATE DATE     : 04.07.2018
// PURPOSE         : Add an overlay of a specific color ontop of specified folders in the Hierarchy window
// SPECIAL NOTES   : inspired by Immanuel-Scholz's comment here: https://feedback.unity3d.com/suggestions/ability-to-change-the-folders-co 
// ===============================
// Change History:
//
//==================================

using UnityEngine;
using UnityEditor;


[InitializeOnLoad]
public class RecolorProjectWindow
{

    static RecolorProjectWindow()
    {
        EditorApplication.projectWindowItemOnGUI += RecolorLineFolder;
    }
    
    static void RecolorLineFolder(string GUID, Rect rect)
    {
        if(Event.current.type == EventType.Repaint)
        {
            // Specify folders and colors to be affected here
            if(setColorBaseForName(GUID, rect, "Scene", Color.yellow))
                return;
            if(setColorBaseForName(GUID, rect, "Script", Color.green))
                return;
            if(setColorBaseForName(GUID, rect, "Sprite", Color.red))
                return;
            if(setColorBaseForName(GUID, rect, "Prefab", Color.blue))
                return;
        }
    }
    
    static bool setColorBaseForName(string GUID,Rect rect, string topFolderName, Color folderColor)
    {
        string pth = AssetDatabase.GUIDToAssetPath(GUID);
        if(pth.Contains(topFolderName))
        {
            
            string[] pths = pth.Split("/"[0]);
            string assetName = pths[pths.Length - 1];
            
            if(!assetName.Contains("."))
            {
                Texture2D texture = new Texture2D(1, 1);
                Color c = new Color(folderColor.r,folderColor.g,folderColor.b,.1f);
                texture.SetPixel(0,0,c);
                texture.Apply();
                GUI.skin.box.normal.background = texture;
                GUI.Box(rect, GUIContent.none);
                return true;
            }
        }
        return false;
    }
}