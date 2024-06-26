using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    AudioSource m_MusicAudio;
    AudioSource m_SoundAudio;

    private float SoundVolume
    {
        get { return PlayerPrefs.GetFloat("SoundVolume", 1.0f); }
        set
        {
            m_SoundAudio.volume = value;
            PlayerPrefs.SetFloat("SoundVolume", value);
        }
    }

    private float MusicVolume
    {
        get { return PlayerPrefs.GetFloat("MusicVolume", 1.0f); }
        set
        {
            m_MusicAudio.volume = value;
            PlayerPrefs.SetFloat("MusicVolume", value);
        }
    }

    private void Awake()
    {
        m_MusicAudio = this.gameObject.AddComponent<AudioSource>();
        m_MusicAudio.playOnAwake = false;
        m_MusicAudio.loop = true;

        m_SoundAudio = this.gameObject.AddComponent<AudioSource>();
        m_SoundAudio.loop = false;
    }

    //��������
    public void PlayMusic(string name)
    {
        if(this.MusicVolume < 0.1f)
        {
            return;
        }
        string oldName = "";
        if(m_MusicAudio.clip != null)
        {
            oldName = m_MusicAudio.clip.name;
        }
        Debug.Log("oldName: " +  oldName + ",   newName: " + name);
        //��ͬ�����ֲ��ظ�����
        if (oldName == name)
            return;

        Manager.Resource.LoadMusic(name, (UnityEngine.Object obj) =>
        {
            m_MusicAudio.clip = obj as AudioClip;
            m_MusicAudio.Play();
        });
    }

    //��ͣ����
    public void PauseMusic()
    {
        m_MusicAudio.Pause();
    }

    //��������
    public void OnUnPauseMusic()
    {
        m_MusicAudio.UnPause();
    }

    //ֹͣ����
    public void StopMusic()
    {
        m_MusicAudio.Stop();
    }

    //������Ч
    public void PlaySound(string name)
    {
        if (this.SoundVolume < 0.1f)
            return;

        Manager.Resource.LoadSound(name, (UnityEngine.Object obj) =>
        {
            m_SoundAudio.PlayOneShot(obj as AudioClip);
        });
    }

    //���ñ�����������
    public void SetMusicVolume(float value)
    {
        this.MusicVolume = value;
    }

    //������Ч����
    public void SetSoundVolume(float value)
    {
        this.SoundVolume = value;
    }

}
