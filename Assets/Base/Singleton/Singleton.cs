/*
 * @Author       : RannarYang
 * @Date         : 2021-04-25 22:00:23
 * @LastEditors  : RannarYang
 * @LastEditTime : 2021-04-26 14:55:00
 * @FilePath     : \Client\Assets\Base\Singleton\Singleton.cs
 */
public class Singleton<T> where T : new()
{
    private static T m_Instance;
    public static T Instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = new T();
            }

            return m_Instance;
        }
    }

}
