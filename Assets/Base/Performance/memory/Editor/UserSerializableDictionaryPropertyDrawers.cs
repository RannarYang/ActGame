using UnityEditor;
[CustomPropertyDrawer(typeof(StringIntDictionary))]
[CustomPropertyDrawer(typeof(StringResourceItemDictionary))]
[CustomPropertyDrawer(typeof(StringAssetBundleItemDictionary))]
[CustomPropertyDrawer(typeof(StringResourceObjArrayDictionary))]
public class CustomSerializableDictionaryPropertyDrawer : SerializableDictionaryPropertyDrawer {}

[CustomPropertyDrawer(typeof(ResourceObjArrayStorage))]
public class CustomAnySerializableDictionaryStoragePropertyDrawer: SerializableDictionaryStoragePropertyDrawer {}
