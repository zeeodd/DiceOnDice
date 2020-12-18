using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Services
{
    #region GameManager
    private static GameManager _gm;
    public static GameManager GameManager
    {
        get
        {
            Debug.Assert(_gm != null);
            return _gm;
        }
        set => _gm = value;
    }
    #endregion

    #region AudioManager
    private static AudioManager _am;
    public static AudioManager AudioManager
    {
        get
        {
            Debug.Assert(_am != null);
            return _am;
        }
        set => _am = value;
    }
    #endregion

    #region EventManager
    private static EventManager _em;
    public static EventManager EventManager
    {
        get
        {
            Debug.Assert(_em != null);
            return _em;
        }
        set => _em = value;
    }
    #endregion
}
