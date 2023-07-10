using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using UltimateFireworks.Interfaces;
using UnityEngine;
using UnityEngine.Networking;
using Zenject;

namespace UltimateFireworks.Models
{
    internal class SoundLoader : MonoBehaviour, ISoundLoader
    {
        public ConcurrentDictionary<string, AudioClip[]> Sounds { get; set; } = new ConcurrentDictionary<string, AudioClip[]>();
        public bool IsLoading { get; private set; } = false;
        private readonly string s_dataPath = Path.Combine(Environment.CurrentDirectory, "UserData", "UltimateFireworks", "Sound");
        public string DataPath => this.s_dataPath;

        public void Awake()
        {
            this.StartCoroutine(this.LoadSounds());
        }

        private IEnumerator LoadSounds()
        {
            this.IsLoading = true;
            this.Sounds.Clear();

            if (!Directory.Exists(this.DataPath)) {
                Directory.CreateDirectory(this.DataPath);
            }

            foreach (var songDirectory in Directory.EnumerateDirectories(this.DataPath, "*", SearchOption.TopDirectoryOnly)) {
                var sounds = new List<AudioClip>();
                foreach (var songPath in Directory.EnumerateFiles(songDirectory, "*.wav", SearchOption.TopDirectoryOnly)) {
                    var song = UnityWebRequestMultimedia.GetAudioClip(songPath, AudioType.WAV);
                    yield return song.SendWebRequest();
                    if (!string.IsNullOrEmpty(song.error)) {
                        continue;
                    }
                    else {
                        var clip = DownloadHandlerAudioClip.GetContent(song);
                        clip.name = Path.GetFileName(songPath);
                        sounds.Add(clip);
                        yield return new WaitWhile(() => !clip);
                    }
                }
                this.Sounds.AddOrUpdate(songDirectory, sounds.ToArray(), (s, d) => sounds.ToArray());
            }
            this.IsLoading = false;
        }
    }
}
