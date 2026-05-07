using UnityEngine;

public class SoundSettings : MonoBehaviour
{
    private static bool isMuted = false;
    
    public void ToggleSoundButton()
    {
        isMuted = !isMuted;

        if (isMuted)
        {
            AudioListener.volume = 0f;
            Debug.Log("Sound: OFF");
        }
        else
        {
            AudioListener.volume = 1f;
            Debug.Log("Sound: ON");
        }
    }
}