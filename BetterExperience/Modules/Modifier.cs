using System.Collections.Generic;
using System.Reflection;

namespace BetterExperience.Modules
{
    /// <summary>
    /// 反射修改器，提供字段设置和缓存功能
    /// </summary>
    public class Modifier
    {
        private readonly Dictionary<string, FieldInfo> _fieldCache = new Dictionary<string, FieldInfo>();

        /// <summary>
        /// 通过反射设置对象字段的值（带缓存）
        /// 调用者需确保参数有效
        /// </summary>
        public void SetValue(object obj, string fieldName, object value)
        {
            var type = obj.GetType();
            var key = type.FullName + "." + fieldName;

            if (!_fieldCache.TryGetValue(key, out var fieldInfo))
            {
                fieldInfo = type.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
                _fieldCache[key] = fieldInfo;
            }

            if (fieldInfo != null) fieldInfo.SetValue(obj, value);
        }

        /// <summary>
        /// 清空反射缓存
        /// </summary>
        public void Clear()
        {
            _fieldCache.Clear();
        }
    }
}
