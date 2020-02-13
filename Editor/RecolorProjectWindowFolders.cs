// ===============================
// AUTHOR          : J. Giezendanner
// CREATE DATE     : 04.07.2018
// MODIFIED DATE   : 08.10.2018
// PURPOSE         : Add an overlay of a specific color ontop of specified folders in the Hierarchy window
// SPECIAL NOTES   : inspired by Immanuel-Scholz's comment here: https://feedback.unity3d.com/suggestions/ability-to-change-the-folders-co 
// ===============================
// Change History:
//  - 08.10.2018: changing the default skin did not work in newer versions of unity. Created new skin and applied it to the box.
//  - 13.02.2020: applying the box skin does not work in unity 2019.3: applying a new skin. Optimised the function a bit
//==================================

using UnityEngine;
using UnityEditor;


[InitializeOnLoad]
public class RecolorProjectWindowFolders
{
	static Texture2D texture = new Texture2D(1, 1);
	
	static GUIStyle boxStyle = new GUIStyle();
	static private string pth;
	static private Rect rect;

	static RecolorProjectWindowFolders()
	{
		EditorApplication.projectWindowItemOnGUI += RecolorLineFolder;
	}

	static void RecolorLineFolder(string GUID, Rect focusedRect)
	{
		if (Event.current.type == EventType.Repaint)
		{
			pth = AssetDatabase.GUIDToAssetPath(GUID);
			rect = focusedRect;
			// Specify folders and colors to be affected here
			if (setColorBaseForName("Scene", Color.yellow))
				return;
			if (setColorBaseForName("Animation", Color.white))
				return;
			if (setColorBaseForName("Script", Color.green))
				return;
			if (setColorBaseForName("Sprite", Color.red))
				return;
			if (setColorBaseForName("Prefab", Color.blue))
				return;
			if (setColorBaseForName("Audio", Color.cyan))
				return;
			if (setColorBaseForName("Material", Color.black))
				return;
			if (setColorBaseForName("Font", Color.magenta))
				return;
		}
	}

	static bool setColorBaseForName(string topFolderName, Color folderColor)
	{
		if (pth.Contains(topFolderName))
		{
			string[] pths = pth.Split("/"[0]);
			string assetName = pths[pths.Length - 1];

			if (!assetName.Contains("."))
			{
				Color c = new Color(folderColor.r, folderColor.g, folderColor.b, .3f);
				texture.SetPixel(0, 0, c);
				texture.alphaIsTransparency = true;
				texture.Apply();
				boxStyle.normal.background = texture;
				GUI.Box(rect, "", boxStyle);
				return true;
			}
		}
		return false;
	}
}
