/*
 * @Author       : RannarYang
 * @Date         : 2021-04-25 22:00:23
 * @LastEditors  : RannarYang
 * @LastEditTime : 2021-04-30 08:49:41
 * @FilePath     : \Client\Assets\Base\Singleton\MonoSingleton.cs
 */
using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    protected static T instance;

    public static T Instance
    {
        get { return instance; }
    }

    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = (T)this;
        }
        else
        {
            Debug.LogError("Get a second instance of this class" + this.GetType());
        }
    }
}
