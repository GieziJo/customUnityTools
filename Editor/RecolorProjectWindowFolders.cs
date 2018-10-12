// ===============================
// AUTHOR          : J. Giezendanner
// CREATE DATE     : 04.07.2018
// MODIFIED DATE   : 08.10.2018
// PURPOSE         : Add an overlay of a specific color ontop of specified folders in the Hierarchy window
// SPECIAL NOTES   : inspired by Immanuel-Scholz's comment here: https://feedback.unity3d.com/suggestions/ability-to-change-the-folders-co 
// ===============================
// Change History:
//  - 08.10.2018: changing the default skin did not work in newer versions of unity. Created new skin and applied it to the box.
//==================================

using UnityEngine;
using UnityEditor;


[InitializeOnLoad]
public class RecolorProjectWindowFolders
{

	static RecolorProjectWindowFolders()
    {
        EditorApplication.projectWindowItemOnGUI += RecolorLineFolder;
    }

    static void RecolorLineFolder(string GUID, Rect rect)
    {
        if (Event.current.type == EventType.Repaint)
        {
            // Specify folders and colors to be affected here
            if (setColorBaseForName(GUID, rect, "Scene", Color.yellow))
                return;
            if (setColorBaseForName(GUID, rect, "Script", Color.green))
                return;
            if (setColorBaseForName(GUID, rect, "Sprite", Color.red))
                return;
            if (setColorBaseForName(GUID, rect, "Prefab", Color.blue))
                return;
            if (setColorBaseForName(GUID, rect, "Audio", Color.cyan))
                return;
            if (setColorBaseForName(GUID, rect, "Material", Color.black))
                return;
            if (setColorBaseForName(GUID, rect, "Font", Color.magenta))
                return;
        }
    }

    static bool setColorBaseForName(string GUID, Rect rect, string topFolderName, Color folderColor)
    {
        string pth = AssetDatabase.GUIDToAssetPath(GUID);
        if (pth.Contains(topFolderName))
        {

            string[] pths = pth.Split("/"[0]);
            string assetName = pths[pths.Length - 1];

            if (!assetName.Contains("."))
            {
                Texture2D texture = new Texture2D(1, 1);
                Color c = new Color(folderColor.r, folderColor.g, folderColor.b, .1f);
                texture.SetPixel(0, 0, c);
                texture.alphaIsTransparency = true;
                texture.Apply();
                GUIStyle boxStyle = new GUIStyle(GUI.skin.box);
                boxStyle.normal.background = texture;
                GUI.Box(rect,"",boxStyle);
                return true;
            }
        }
        return false;
    }
}