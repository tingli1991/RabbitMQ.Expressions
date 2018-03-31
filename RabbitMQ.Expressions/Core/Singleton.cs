using System.Collections.Generic;

namespace RabbitMQ.Expressions.Core
{
    /// <summary>
    /// 单利模式
    /// </summary>
    /// <typeparam name="T">泛型类</typeparam>
    class Singleton
    {
        private static object _lock = new object();
        private static Dictionary<string, object> instanceDic = new Dictionary<string, object>();

        /// <summary>
        /// 创建对象实例
        /// </summary>
        /// <param name="assemblyName">类型所在程序集名称</param>
        /// <param name="nameSpace">类型所在命名空间</param>
        /// <param name="className">类型名</param>
        /// <returns></returns>
        public static object GetInstance(string filePath, string assemblyName, string nameSpace, string className)
        {
            object instance = null;
            var fullName = ReflectionHelper.GetFullName(nameSpace, className);
            if (!instanceDic.ContainsKey(fullName))
            {
                lock (_lock)
                {
                    if (!instanceDic.ContainsKey(fullName))
                    {
                        instance = ReflectionHelper.CreateInstance(filePath, assemblyName, nameSpace, className);
                        instanceDic.Add(fullName, instance);
                    }
                }
            }
            else
            {
                instance = instanceDic[fullName];
            }
            return instance;
        }
    }
}