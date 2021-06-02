/*
 * @Author       : RannarYang
 * @Date         : 2021-04-26 13:53:18
 * @LastEditors  : RannarYang
 * @LastEditTime : 2021-04-26 14:50:48
 * @FilePath     : \Client\Assets\Base\Utils\IdGenerator.cs
 */
using System.Collections.Generic;
public class IdGenerator: Singleton<IdGenerator> {
    private Dictionary<int, int> idDic = new Dictionary<int, int>();
    private const int Default_ID_Begin = 1;
    /**获取ID */
    public int GetID(int idType) {
        if(!this.idDic.ContainsKey(idType)) {
            int res = Default_ID_Begin;
            this.idDic.Add(idType, res);
            return res;
        } else {
            int lastId;
            this.idDic.TryGetValue(idType, out lastId);
            lastId++;
            this.idDic[idType] = lastId;
            return lastId;
        }
    }

    /**删除ID */
    public void RemoveID(int idType) {
        this.idDic.Remove(idType);
    }

    /**清除所有 */
    public void ClearAll() {
        this.idDic.Clear();
    }
}
