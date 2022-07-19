using System.Collections;
using System.Collections.Concurrent;
using UnityEngine;

namespace UltimateFireworks.Interfaces
{
    internal interface ISoundLoader
    {
        ConcurrentDictionary<string, AudioClip[]> Sounds { get; set; }
        string DataPath { get; }
        bool IsLoading { get; }
    }
}