using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static class SS_GameData {

    private static bool[] objectiveStatus = new bool[10];
    private static bool[] exploreStatus = new bool[10];
    private static bool openCutscene = true;
    private static bool camEnabled = true;
    private static bool moveEnabled = true;
    private static bool mouseEnabled = true;
    private static bool firstItemMouseOver = true;
    private static string buttonTag = "iButton";
    private static string containerTag = "iContainer";
    private static string itemTag = "item";
    private static float bgVol = 1.0f;
    private static float sfxVol = 0.5f;
    private static float vfxVol = 1.0f;

    public static bool[] objective
    {
        get
        {
            return objectiveStatus;
        }
        set
        {
            objectiveStatus = value;
        }
    }

    public static bool[] explore
    {
        get
        {
            return exploreStatus;
        }
        set
        {
            exploreStatus = value;
        }
    }

    /// <summary>
    /// Return/Set the global background game volume.
    /// </summary>
    public static float BackgroundVolume
    {
        get
        {
            return bgVol;
        }
        set
        {
            bgVol = value;
        }
    }

    /// <summary>
    /// Return/Set the global sound effect volume
    /// </summary>
    public static float SFXVolume
    {
        get
        {
            return sfxVol;
        }
        set
        {
            sfxVol = value;
        }
    }

    /// <summary>
    /// Return/Set the global voice sound volume
    /// </summary>
    public static float VoiceSoundVolume
    {
        get
        {
            return vfxVol;
        }
        set
        {
            vfxVol = value;
        }
    }

    /// <summary>
    /// Return the designated interactable system button tag
    /// </summary>
    public static string GetButtonTag
    {
        get
        {
            return buttonTag;
        }
    }

    /// <summary>
    /// Return the designated interactable system container tag
    /// </summary>
    public static string GetContainerTag
    {
        get
        {
            return containerTag;
        }
    }

    public static string GetItemTag
    {
        get
        {
            return itemTag;
        }
    }

    /// <summary>
    /// Global check for camera control
    /// </summary>
    public static bool isCameraEnabled
    {
        get
        {
            return camEnabled;
        }
        set
        {
            camEnabled = value;
        }
    }

    /// <summary>
    /// Global check for movement control
    /// </summary>
    public static bool isMovementEnabled
    {
        get
        {
            return moveEnabled;
        }
        set
        {
            moveEnabled = value;
        }
    }

    /// <summary>
    /// Global check for mouse control
    /// </summary>
    public static bool isMouseEnabled
    {
        get
        {
            return mouseEnabled;
        }
        set
        {
            mouseEnabled = value;
        }
    }

    public static bool playLongCutscene
    {
        get
        {
            return openCutscene;
        }
        set
        {
            openCutscene = value;
        }
    }
}
