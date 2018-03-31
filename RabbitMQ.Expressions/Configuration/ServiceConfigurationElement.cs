using System;
using System.Configuration;

namespace RabbitMQ.Expressions.Configuration
{
    /// <summary>
    /// 服务端队列配置
    /// </summary>
    public class ServiceConfigurationElement : ConfigurationElement
    {
        /// <summary>
        /// 队列名称
        /// </summary>
        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get => $"{base["name"]}";
        }

        /// <summary>
        /// 程序集
        /// </summary>
        [ConfigurationProperty("assemblyName", DefaultValue = "")]
        public string AssemblyName
        {
            get
            {
                var assemblyName = $"{base["assemblyName"]}";
                if (string.IsNullOrWhiteSpace(assemblyName))
                {
                    assemblyName = NameSpace;
                }
                return assemblyName;
            }
        }

        /// <summary>
        /// 命名空间
        /// </summary>
        [ConfigurationProperty("nameSpace", IsRequired = true)]
        public string NameSpace
        {
            get => $"{base["nameSpace"]}";
            set => base["nameSpace"] = value;
        }

        /// <summary>
        /// 类名
        /// </summary>
        [ConfigurationProperty("className", IsRequired = true)]
        public string ClassName
        {
            get => $"{base["className"]}";
        }

        /// <summary>
        /// 是否持久化
        /// </summary>
        [ConfigurationProperty("durable", DefaultValue = true)]
        public bool Durable
        {
            set => base["durable"] = value;
            get => Convert.ToBoolean(base["durable"]);
        }

        /// <summary>
        /// 可以等待返回的队列数
        /// </summary>
        [ConfigurationProperty("prefetchcount", DefaultValue = (ushort)1)]
        public ushort PrefetchCount
        {
            get => Convert.ToUInt16(base["prefetchcount"]);
        }
    }
}