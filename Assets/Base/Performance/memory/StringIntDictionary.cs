using System;
using System.Collections.Generic;

[Serializable]
public class StringIntDictionary : SerializableDictionary<string, int> {}

[Serializable]
public class StringResourceItemDictionary : SerializableDictionary<string, ResourceItem> {}

[Serializable]
public class StringAssetBundleItemDictionary : SerializableDictionary<string, AssetBundleItem> {}

[Serializable]
public class ResourceObjArrayStorage : SerializableDictionary.Storage<ResourceObj[]> {}

[Serializable]
public class StringResourceObjArrayDictionary : SerializableDictionary<string, ResourceObj[], ResourceObjArrayStorage> {}

