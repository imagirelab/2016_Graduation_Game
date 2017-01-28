using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(RawImage))]
[RequireComponent(typeof(AudioSource))]
public class Movie_On_UI : MonoBehaviour
{
    //[SerializeField]
    //string m_moviePath;

    public MovieTexture m_movieTexture;

    RawImage m_rawImage = null;
    AudioSource m_audioSource = null;

    public bool IsPlaying
    {
        get { return m_movieTexture.isPlaying; }
    }

    public void Play()
    {
        if (IsPlaying)
        {
            Stop();
        }

        if (m_movieTexture == null)
        {
            Debug.LogError("movie is nothing:"/* + m_moviePath*/);
            return;
        }

        m_movieTexture.loop = false;

        m_rawImage.material.mainTexture = m_movieTexture;
        m_audioSource.clip = m_movieTexture.audioClip;

        m_movieTexture.Play();
        m_audioSource.Play();
    }

    public void Stop()
    {
        if (!IsPlaying)
        {
            return;
        }

        m_movieTexture.Stop();
        m_audioSource.Stop();
    }
    
    void OnEnable()
    {
        //m_movieTexture = (MovieTexture)Resources.Load<MovieTexture>(m_moviePath);

        m_rawImage = this.GetComponent<RawImage>();
        m_audioSource = this.GetComponent<AudioSource>();

        Play(); // テスト用
    }

        void OnDisable()
    {
        m_movieTexture.Stop();
        //m_movieTexture = null;
    }
}
