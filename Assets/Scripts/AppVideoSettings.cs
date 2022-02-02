using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Control the video resolution of the Unity Game
/// </summary>
public class AppVideoSettings : MonoBehaviour
{
    public bool isFullScreen { get; private set; } = true;
    public int Monitor { get; private set; } = 0;
    public int ScreenWidth { get; private set; } = 0;
    public int ScreenHeight { get; private set; } = 0;
    public int screenRR { get; private set; } = 0;

    //void Start()
    //{
    //    Monitor = PlayerPrefs.GetInt("UnitySelectMonitor", 0);
    //    isFullScreen = Screen.fullScreen;
    //}

    public Resolution GetScreenResoution()
    {
        return Screen.currentResolution;
    }

    public int GetScreenResoutionIndex(int width, int height, int refreshRate)
    {
        Resolution[] resolutions = GetScreenResolutions();
        int index = 0;
        bool found = false;
        for(index = 0; index < resolutions.Length; index++)
        {
            if(resolutions[index].width == width &&
                resolutions[index].height == height &&
                resolutions[index].refreshRate == refreshRate)
            {
                found = true;
                break;
            }
        }

        if (!found)
            index = resolutions.Length - 1;

        return index;
    }

    public void SetScreenResolution(Resolution res)
    {
        FullScreenMode fsm = FullScreenMode.FullScreenWindow;
        if (!isFullScreen)
            fsm = FullScreenMode.Windowed;

        Screen.SetResolution(res.width, res.height, fsm, res.refreshRate);
    }

    public void SetScreenResolution(int index)
    {
        SetScreenResolution(GetScreenResolutions()[index]);
    }

    public void SetFullScreen(bool val)
    {
        isFullScreen = val;
        SetScreenResolution(Screen.currentResolution);
    }

    private Resolution[] GetScreenResolutions()
    {
        return Screen.resolutions;
    }

    public string[] GetScreenResoultions_String()
    {
        Resolution[] res = GetScreenResolutions();
        string[] res_strings = new string[res.Length];
        for(int i = 0; i < res.Length; i++)
        {
            res_strings[i] = res[i].width + " x " + res[i].height + ", " + res[i].refreshRate + "Hz";
        }

        return res_strings;
    }

    public string[] GetDisplays()
    {
        Display[] displays = Display.displays;
        string[] display_strings = new string[displays.Length];
        for(int i = 0; i < displays.Length; i++)
        {
            display_strings[i] = "Display " + (i + 1);
        }

        return display_strings;
    }

    private IEnumerator ChangeDisplayAsync(int display)
    {
        Monitor = display;
        if (Monitor >= Display.displays.Length)
        {
            Monitor = 0;
        }
        PlayerPrefs.SetInt("UnitySelectMonitor", Monitor);
        yield return null;
        Application.Quit();//Currently need to reboot the app for this to apply...
    }

    public void Apply(int resIndex, int monitorDisplay, bool fullScreen)
    {
        //Debug.Log(monitorDisplay);

        if (monitorDisplay != Monitor)
            StartCoroutine(ChangeDisplayAsync(monitorDisplay));

        isFullScreen = fullScreen;
        SetScreenResolution(resIndex);
    }

    public void ChangeDisplay()
    {
        if (1 != Monitor)
            StartCoroutine(ChangeDisplayAsync(1));
        isFullScreen = true;
        SetScreenResolution(16);
    }
}
