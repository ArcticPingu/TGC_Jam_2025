using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public float time;
    void Start()
    {
        PlayMusic();
    }

    void Update()
    {
        if (time < 0)
        {
            PlayMusic();
        }
        else
        {
            time += Time.deltaTime;
        }
    }

    private void PlayMusic()
    {
        time = Random.Range(55f, 80f);
        GetComponent<AudioSource>().Play();
    }

}
