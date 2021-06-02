/*
 * @Author       : RannarYang
 * @Date         : 2021-04-25 21:52:25
 * @LastEditors  : RannarYang
 * @LastEditTime : 2021-04-26 23:23:28
 * @FilePath     : \Client\Assets\Base\Pool\Pool.cs
 */
using System.Collections.Generic;

public class Pool<T> where T :class, new()
{
    //池
    protected Stack<T> m_Pool = new Stack<T>();

    /// <summary>
    /// 获取在池子中的对象的数量
    /// </summary>
    /// <returns></returns>
    public int GetCount() {
        return this.m_Pool.Count;
    }
    //最大对象个数，<=0表示不限个数
    protected int m_MaxCount = 0;
    //没有回收的对象个数
    protected int m_NoRecycleCount = 0;

    public Pool(int maxcount)
    {
        m_MaxCount = maxcount;
        for (int i = 0; i < maxcount; i++)
        {
            m_Pool.Push(new T());
        }
    }

    /// <summary>
    /// 从池里面取类对象
    /// </summary>
    /// <param name="creatIfPoolEmpty">如果为空是否new出来</param>
    /// <returns></returns>
    public T Spawn(bool creatIfPoolEmpty)
    {
        if (m_Pool.Count > 0)
        {
            T rtn = m_Pool.Pop();
            if (rtn == null)
            {
                if (creatIfPoolEmpty)
                {
                    rtn = new T();
                }
            }
            m_NoRecycleCount++;
            return rtn;
        }
        else
        {
            if (creatIfPoolEmpty)
            {
                T rtn = new T();
                m_NoRecycleCount++;
                return rtn;
            }
        }

        return null;
    }

    /// <summary>
    /// 回收类对象
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public bool Recycle(T obj)
    {
        if (obj == null)
            return false;

        m_NoRecycleCount--;

        if (m_Pool.Count >= m_MaxCount && m_MaxCount > 0)
        {
            obj = null;
            return false;
        }

        m_Pool.Push(obj);
        return true;
    }
}
