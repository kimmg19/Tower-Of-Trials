using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{

    public static AudioManager instance;

    public Sound[] sounds;

    void Awake()
    {
        if (instance == null)
        {
            SceneManager.sceneLoaded += LoadsceneEvent;//sceneLoad이벤트에 LoadsceneEvent 함수 추가.
            instance = this;
            DontDestroyOnLoad(gameObject);
            foreach (Sound s in sounds)
            {
                s.source = gameObject.AddComponent<AudioSource>();
                s.source.outputAudioMixerGroup = s.mixer;
                s.source.clip = s.clip;
                s.source.volume = s.volume;
                s.source.pitch = s.pitch;
                s.source.loop = s.loop;
            }
        } else
        {
            Destroy(gameObject);
            return;
        }
    }

    //씬 전환시 사운드 변경하는-Unity엔진에서 자동으로 매개변수 전달.
    //mode는 씬 로드 방식. Single(새로, 이전 씬 제거),Additive(씬 위에 추가로)
    private void LoadsceneEvent(Scene scene, LoadSceneMode mode)
    {
        StopPreviousSceneAudio();
        if (scene.name == "LoadingScene")
        {            
            return;
        }
        else
        {
           Play(scene.name + "Bgm");           
        }

        
    }
    void StopPreviousSceneAudio()
    {
        foreach (Sound s in sounds)
        {
            if (s.source.isPlaying)
            {
                s.source.Stop();
                print("stop" + s.name);
            }
        }
    }

    public void Play(string sound)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        s.source.Play();
    }
    public void Stop(string sound)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        s.source.Stop();
    }

}
