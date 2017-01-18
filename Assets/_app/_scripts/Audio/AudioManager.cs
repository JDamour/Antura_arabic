﻿using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Audio;

namespace EA4S
{

    /// <summary>
    /// Handles audio requests throughout the application
    /// </summary>
    // conventions: needs renaming
    public class AudioManager : MonoBehaviour
    {
        const string LETTERS_PREFIX = "Letter/";
        const string WORDS_PREFIX = "";

        public static AudioManager I;

        [SerializeField, HideInInspector]
        public List<SfxConfiguration> sfxConfs = new List<SfxConfiguration>();

        [SerializeField, HideInInspector]
        public List<MusicConfiguration> musicConfs = new List<MusicConfiguration>();

        public AudioMixerGroup musicGroup;
        public AudioMixerGroup sfxGroup;
        public AudioMixerGroup lettersGroup;
        public AudioMixerGroup keeperGroup;

        System.Action OnNotifyEndAudio;
        bool hasToNotifyEndAudio = false;

        bool musicEnabled = true;
        public bool MusicEnabled { get { return musicEnabled; } }
        Music currentMusic;

        Dictionary<string, AudioClip> audioCache = new Dictionary<string, AudioClip>();

        void Awake()
        {
            I = this;

            musicEnabled = true;
        }

        public void OnAppPause(bool pauseStatus)
        {
            // app is pausing
            if (pauseStatus)
            {
                if (Fabric.EventManager.Instance != null)
                    Fabric.EventManager.Instance.PostEvent("MusicTrigger", Fabric.EventAction.StopAll);
            }
            //app is resuming
            if (!pauseStatus)
            {
                if (musicEnabled)
                {
                    PlayMusic(currentMusic);
                }
            }
        }

        public void NotifyEndAudio(Fabric.EventNotificationType type, string boh, object info, GameObject gameObject)
        {
            // Debug.Log ("OnNotify:" + type + "GameObject:" + gameObject.name);
            if (info != null)
            {
                if (type == Fabric.EventNotificationType.OnAudioComponentStopped)
                {
                    //Debug.Log("NotifyEndAudio OnAudioComponentStopped()");
                    hasToNotifyEndAudio = true;
                }
            }
        }

        #region Music
        public void ToggleMusic()
        {
            musicEnabled = !musicEnabled;
            if (musicEnabled)
            {
                PlayMusic(currentMusic);
            }
            else
            {
                if (Fabric.EventManager.Instance != null)
                    Fabric.EventManager.Instance.PostEvent("MusicTrigger", Fabric.EventAction.StopAll);
            }
        }

        public void PlayMusic(Music music)
        {
            currentMusic = music;
            var eventName = AudioConfig.GetMusicEventName(music);
            if (eventName == "")
            {
                StopMusic();
            }
            else
            {
                if (musicEnabled)
                {
                    Fabric.EventManager.Instance.PostEvent("MusicTrigger", Fabric.EventAction.SetSwitch, eventName);
                    Fabric.EventManager.Instance.PostEvent("MusicTrigger");
                    //Fabric.EventManager.Instance.PostEvent("Music/" + eventName);
                }
            }
        }

        public void StopDialogue(bool clearPreviousCallback)
        {
            if (!clearPreviousCallback && OnNotifyEndAudio != null)
                OnNotifyEndAudio();

            OnNotifyEndAudio = null;

            if (Fabric.EventManager.Instance != null)
                Fabric.EventManager.Instance.PostEvent("KeeperDialog", Fabric.EventAction.StopAll);
        }

        public void StopMusic()
        {
            currentMusic = Music.Silence;
            if (Fabric.EventManager.Instance != null)
                Fabric.EventManager.Instance.PostEvent("MusicTrigger", Fabric.EventAction.StopAll);
        }

        void FadeOutMusic(string n)
        {
            Fabric.Component component = Fabric.FabricManager.Instance.GetComponentByName(n);
            if (component != null)
            {
                component.FadeOut(0.1f, 0.5f);
            }
        }
        #endregion

        #region Sfx
        /// <summary>
        /// Play a soundFX
        /// </summary>
        /// <param name="sfx">Sfx.</param>
        public void PlaySfx(Sfx sfx)
        {
            PlaySound(AudioConfig.GetSfxEventName(sfx));
        }

        public void StopSfx(Sfx sfx)
        {
            StopSound(AudioConfig.GetSfxEventName(sfx));
        }
        #endregion

        #region generic sound
        void PlaySound(string eventName)
        {
            Fabric.EventManager.Instance.PostEvent(eventName);
        }

        void PlaySound(string eventName, GameObject GO)
        {
            Fabric.EventManager.Instance.PostEvent(eventName, GO);
        }
        #endregion

        #region Letters, WOrds and Phrases
        public void PlayLetter(string letterId)
        {
            Fabric.EventManager.Instance.PostEvent(LETTERS_PREFIX + letterId);
        }

        public void PlayWord(string wordId)
        {
            //Debug.Log("PlayWord: " + wordId);
            Fabric.EventManager.Instance.PostEvent("Words", Fabric.EventAction.SetAudioClipReference, "Words/" + wordId);
            Fabric.EventManager.Instance.PostEvent("Words");
            //Fabric.EventManager.Instance.PostEvent(WORDS_PREFIX + wordId);
        }

        public void PlayPhrase(string phraseId)
        {
            //Debug.Log("PlayWord: " + wordId);
            Fabric.EventManager.Instance.PostEvent("Words", Fabric.EventAction.SetAudioClipReference, "Phrases/" + phraseId);
            Fabric.EventManager.Instance.PostEvent("Words");
            //Fabric.EventManager.Instance.PostEvent(WORDS_PREFIX + wordId);
        }
        #endregion

        void StopSound(string eventName)
        {
            if (Fabric.EventManager.Instance != null)
                Fabric.EventManager.Instance.PostEvent(eventName, Fabric.EventAction.StopAll);
        }



        #region Dialog
        public void PlayDialog(string localizationData_id)
        {
            PlayDialog(LocalizationManager.GetLocalizationData(localizationData_id));
        }

        public void PlayDialog(Db.LocalizationDataId id)
        {
            PlayDialog(LocalizationManager.GetLocalizationData(id));
        }

        public void PlayDialog(Db.LocalizationData data, bool clearPreviousCallback = false)
        {
            if (!clearPreviousCallback && OnNotifyEndAudio != null)
                OnNotifyEndAudio();

            OnNotifyEndAudio = null;

            if (data.AudioFile != "")
            {
                //Debug.Log("PlayDialog: " + data.id + " - " + Fabric.EventManager.GetIDFromEventName(string_id));
                Fabric.EventManager.Instance.PostEvent("KeeperDialog", Fabric.EventAction.SetAudioClipReference, "Dialogs/" + data.AudioFile);
                Fabric.EventManager.Instance.PostEvent("KeeperDialog");
            }
        }

        public void PlayDialog(string localizationData_id, System.Action callback)
        {
            PlayDialog(LocalizationManager.GetLocalizationData(localizationData_id), callback);
        }

        public void PlayDialog(Db.LocalizationDataId id, System.Action callback)
        {
            PlayDialog(LocalizationManager.GetLocalizationData(id), callback);
        }

        public void PlayDialog(Db.LocalizationData data, System.Action callback, bool clearPreviousCallback = false)
        {
            if (!clearPreviousCallback && OnNotifyEndAudio != null)
                OnNotifyEndAudio();

            OnNotifyEndAudio = null;

            if (data.AudioFile != "")
            {
                // Debug.Log("PlayDialog with Callback: " + data.id + " - " + Fabric.EventManager.GetIDFromEventName(string_id));

                OnNotifyEndAudio = callback;
                Fabric.EventManager.Instance.PostEvent("KeeperDialog", Fabric.EventAction.SetAudioClipReference, "Dialogs/" + data.AudioFile);
                Fabric.EventManager.Instance.PostEventNotify("KeeperDialog", NotifyEndAudio);
            }
            else
            {
                if (callback != null)
                    callback();
            }
        }
        #endregion

        #region Audio clip utilities

        public AudioClip GetAudioClip(Db.LocalizationData data)
        {
            return GetCachedResource("AudioArabic/Dialogs/" + data.AudioFile);
        }


        public AudioClip GetAudioClip(ILivingLetterData data)
        {
            if (data.DataType == LivingLetterDataType.Letter)
                return GetCachedResource("AudioArabic/Letters/" + data.Id);
            else if (data.DataType == LivingLetterDataType.Word || data.DataType == LivingLetterDataType.Image)
            {
                return GetCachedResource("AudioArabic/Words/" + data.Id);
            }
            else if (data.DataType == LivingLetterDataType.Phrase)
            {
                return GetCachedResource("AudioArabic/Phrases/" + data.Id);
            }

            return null;
        }

        public AudioClip GetAudioClip(Sfx sfx)
        {
            // TODO use a dictionary
            SfxConfiguration conf = sfxConfs.Find((a) => { return a.sfx == (Sfx)sfx; });

            if (conf == null)
                return null;

            return conf.clips.GetRandom();
        }

        AudioClip GetCachedResource(string resource)
        {
            AudioClip clip = null;

            if (audioCache.TryGetValue(resource, out clip))
                return clip;

            clip = Resources.Load(resource) as AudioClip;

            audioCache[resource] = clip;
            return clip;
        }

        public void ClearCache()
        {
            foreach (var r in audioCache)
                Resources.UnloadAsset(r.Value);
            audioCache.Clear();
        }
        #endregion


        void Update()
        {
            if (hasToNotifyEndAudio)
            {
                hasToNotifyEndAudio = false;
                if (OnNotifyEndAudio != null)
                {
                    var oldCallback = OnNotifyEndAudio;
                    OnNotifyEndAudio = null;
                    oldCallback();
                }
            }
        }
    }
}