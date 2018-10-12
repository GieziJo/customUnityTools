// ===============================
// Original author : troien
// Modified by	   : J. Giezendanner
// SPECIAL NOTES   : original by troien here: http://answers.unity.com/answers/826188/view.html
// ===============================
// Change History:
// J. Giezendanner Modified to accomodate multiple lines, enumes, audioclips and a title
//==================================

using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System;
using System.Text.RegularExpressions;

[CustomEditor(typeof(AudioManager))]
public class AudioManagerInspector : Editor
{
	private ReorderableList reorderableList;
	float sizeFactor = 4.1f;

	private AudioManager AudioManager
	{
		get
		{
			return target as AudioManager;
		}
	}

	private void OnEnable()
	{
		reorderableList = new ReorderableList(AudioManager.clips,typeof(AudioClipValue), true, true, true, true);
		
		reorderableList.elementHeight = EditorGUIUtility.singleLineHeight * sizeFactor;

		// Add listeners to draw events
		reorderableList.drawHeaderCallback += DrawHeader;
		reorderableList.drawElementCallback += DrawElement;

		reorderableList.onAddCallback += AddItem;
		reorderableList.onRemoveCallback += RemoveItem;
	}

	private void OnDisable()
	{
		// Make sure we don't get memory leaks etc.
		reorderableList.drawHeaderCallback -= DrawHeader;
		reorderableList.drawElementCallback -= DrawElement;

		reorderableList.onAddCallback -= AddItem;
		reorderableList.onRemoveCallback -= RemoveItem;
	}

	/// <summary>
	/// Draws the header of the list
	/// </summary>
	/// <param name="rect"></param>
	private void DrawHeader(Rect rect)
	{
		GUI.Label(rect, "Audio Clips");
	}

	/// <summary>
	/// Draws one element of the list (AudioClipValue)
	/// </summary>
	/// <param name="rect"></param>
	/// <param name="index"></param>
	/// <param name="active"></param>
	/// <param name="focused"></param>
	private void DrawElement(Rect rect, int index, bool active, bool focused)
	{
		float elementHeight = rect.height / sizeFactor;
		AudioClipValue item = AudioManager.clips[index];

		EditorGUI.BeginChangeCheck();
		EditorGUI.LabelField(new Rect(rect.x,rect.y + 1 * elementHeight,120,elementHeight),"Audio Clip Type");
		EditorGUI.LabelField(new Rect(rect.x,rect.y + 2 * elementHeight,120,elementHeight),"Audio Clip Volume");
		EditorGUI.LabelField(new Rect(rect.x,rect.y + 3 * elementHeight,120,elementHeight),"Audio Clip");

		item.key = (AudioClipName) EditorGUI.EnumPopup(new Rect(rect.x + 120, rect.y + 1 * elementHeight, rect.width-120, elementHeight), item.key);
		item.volume = EditorGUI.Slider(new Rect(rect.x + 120, rect.y + 2 * elementHeight, rect.width - 120, elementHeight), item.volume, 0f, 1f);
		item.audioClip = (AudioClip)EditorGUI.ObjectField(new Rect(rect.x + 120, rect.y + 3 * elementHeight, rect.width - 120, elementHeight), item.audioClip, typeof(AudioClip), true);

		GUIStyle headerStyle = EditorStyles.boldLabel;
		headerStyle.alignment = TextAnchor.MiddleCenter;
		EditorGUI.LabelField(new Rect(rect.x,rect.y,rect.width,elementHeight), Regex.Replace(Enum.GetName(typeof(AudioClipName), item.key), "([a-z])([A-Z])", "$1 $2"), headerStyle);
		if (EditorGUI.EndChangeCheck())
		{
			EditorUtility.SetDirty(target);
		}
	}

	private void AddItem(ReorderableList list)
	{
		AudioManager.clips.Add(new AudioClipValue());

		EditorUtility.SetDirty(target);
	}

	private void RemoveItem(ReorderableList list)
	{
		AudioManager.clips.RemoveAt(list.index);

		EditorUtility.SetDirty(target);
	}

	public override void OnInspectorGUI()
	{
		// Actually draw the list in the inspector
		reorderableList.DoLayoutList();
	}
}