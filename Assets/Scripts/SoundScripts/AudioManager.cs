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
            SceneManager.sceneLoaded += LoadsceneEvent;//sceneLoad�̺�Ʈ�� LoadsceneEvent �Լ� �߰�.
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

    //�� ��ȯ�� ���� �����ϴ�-Unity�������� �ڵ����� �Ű����� ����.
    //mode�� �� �ε� ���. Single(����, ���� �� ����),Additive(�� ���� �߰���)
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
