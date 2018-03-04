using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CrackleAudio
{
    public class SoundController : SceneSingleton<SoundController>
    {
        [SerializeField] SoundGroup[] soundGroups;
        [SerializeField] SoundGroup[] musics;

        IDictionary<string, SoundGroup> soundMap = new Dictionary<string, SoundGroup>();
        IDictionary<string, SoundGroup> musicMap = new Dictionary<string, SoundGroup>();
        Pool<AudioSource> sources;
        AudioSource musicSource;

        void Start()
        {
            sources = new Pool<AudioSource>(() => gameObject.AddComponent<AudioSource>());
            musicSource = sources.Get();
            musicSource.loop = true;
            MapGroups(soundGroups, soundMap);
            MapGroups(musics, musicMap);
        }

        void MapGroups(SoundGroup[] groups, IDictionary<string, SoundGroup> map)
        {
            foreach (var group in groups)
            {
                map.Add(group.Name, group);
            }
        }

        public static int PlaySound(string groupName, float volume = 1f)
        {
            return Instance.PlaySoundImpl(groupName, volume);
        }

        int PlaySoundImpl(string groupName, float volume)
        {
            var source = sources.Get();
            source.volume = volume;
            source.clip = soundMap[groupName].GetRandomSound();
            source.Play();
            StartCoroutine(PutSourceBackInPoolOnClipEnd(source));

            return source.GetInstanceID();
        }

        IEnumerator PutSourceBackInPoolOnClipEnd(AudioSource source)
        {
            yield return new WaitForSeconds(source.clip.length);
            sources.Put(source);
        }

        public static int AddEnvironmentalSound(string soundName)
        {
            throw new System.NotImplementedException();
        }

        public static void PlayMusic(string songName, float volume)
        {
            Instance.PlayMusicImpl(songName, volume);
        }

        void PlayMusicImpl(string songName, float volume)
        {
            musicSource.volume = volume;
            musicSource.clip = musicMap[songName].GetRandomSound();
            musicSource.Play();
        }
    }
}