using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
[System.Serializable]
public class Sound
{
    public string soundName;
    public AudioClip clip;//소리 파일
    private AudioSource source;//소리 플래이어

    public float volumn;
    public bool loop;

    public void SetSource(AudioSource _source)
    {
        source = _source;
        source.clip = clip;
        source.loop = loop;
    }
    public void SetVolumn()
    {
        source.volume = volumn;
    }
    public void Play()
    {
        source.Play();
    }
    public void Stop()
    {
        source.Stop();
    }
    public void SetLoop()
    {
        source.loop = true;
    }
    public void SetLoopCancel()
    {
        source.loop = false;
    }
}

public class AudioManager : MonoBehaviour
{
    static public AudioManager instance;//오브젝트 파괴방지

    public PhotonView PV;

    [SerializeField] Sound[] sounds;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
    }

    private void Start()
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            GameObject soundObject = new GameObject("사운드 파일 이름 : " + i + "=" + sounds[i].soundName);
            sounds[i].SetSource(soundObject.AddComponent<AudioSource>());
            soundObject.transform.SetParent(this.transform);
        }
    }
    [PunRPC]
    public void Play(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if(_name == sounds[i].soundName)
            {
                sounds[i].Play();
                return;
            }
        }
    }
    public void Stop(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (_name == sounds[i].soundName)
            {
                sounds[i].Stop();
                return;
            }
        }
    }
    public void SetLoop(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (_name == sounds[i].soundName)
            {
                sounds[i].SetLoop();
                return;
            }
        }
    }
    public void SetLoopCancel(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (_name == sounds[i].soundName)
            {
                sounds[i].SetLoopCancel();
                return;
            }
        }
    }
    public void SetVolumn(string _name, float Volumn)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (_name == sounds[i].soundName)
            {
                sounds[i].volumn = Volumn;
                sounds[i].SetVolumn();
                return;
            }
        }
    }
}
