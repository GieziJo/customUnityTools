// ===============================
// Original author : Dr. Tim "Dr. T" Chamillard
// Modified by	   : J. Giezendanner
// SPECIAL NOTES   : original by Dr. Tim "Dr. T" Chamillard here: https://www.coursera.org/learn/more-programming-unity/supplement/n05pv/unity-audio-lecture-code
// ===============================
// Change History:
// J.Giezendanner: Modified to use enums and inspector instead of calling resources, added volume per clip option
//==================================

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// the required fields in the dictionary
/// </summary>
[Serializable]
public class AudioClipEntry{
    public AudioClip audioClip;
    public float volume = 1f;
}

/// <summary>
///  the exposed class in inspector
/// </summary>
[Serializable]
public class AudioClipValue : AudioClipEntry
{
    public AudioClipName key;
}


[AddComponentMenu("Giezi/AudioManager")]
public class AudioManager : MonoBehaviour {


    public static AudioManager sm;

    // the audioClips in the hierarchy
    [HideInInspector]
    public List<AudioClipValue> clips;
    private Dictionary<AudioClipName, List<AudioClipEntry>> audioClipDictionary;
    AudioSource audioSource;


    // Use this for initialization
    void Awake ()
    {
        if (sm == null)
        {
            audioSource = this.GetComponent<AudioSource>();
            DontDestroyOnLoad(this.gameObject);
            sm = this;
        }
        else if (sm != this)
            Destroy(gameObject);
        
        // initialise dictionary
        audioClipDictionary = new Dictionary<AudioClipName, List<AudioClipEntry>>();
        for (int i = 0; i < clips.Count; i++)
        {
            List<AudioClipEntry> clipList = null;
            if(audioClipDictionary.TryGetValue(clips[i].key,out clipList)){
                clipList.Add(clips[i]);
            }
            else
            {
                clipList = new List<AudioClipEntry>();
                clipList.Add(clips[i]);
                audioClipDictionary.Add(clips[i].key,clipList);
            }
        }
        // add audiosource
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.loop = false;
    }

    /// <summary>
    /// Randomly plays a clip from the list of clips for the specified entry
    /// </summary>
    /// <param name="entry">The key to be played</param>
    public void Play(AudioClipName entry)
    {
        List<AudioClipEntry> audioClips = null;
        if(audioClipDictionary.TryGetValue(entry, out audioClips))
        {
            AudioClipEntry chosenClip = audioClips[UnityEngine.Random.Range( 0, audioClips.Count )];
            audioSource.PlayOneShot(chosenClip.audioClip, chosenClip.volume);
        }else{
            Debug.Log("<color=red>No clip found with this entry</color>");
        }
    }
}
