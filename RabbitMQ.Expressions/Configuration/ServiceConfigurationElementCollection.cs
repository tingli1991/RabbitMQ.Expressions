using System;
using System.Configuration;
using System.IO;

namespace RabbitMQ.Expressions.Configuration
{
    /// <summary>
    /// 队列配置集合(服务端)
    /// </summary>
    [ConfigurationCollection(typeof(ServiceConfigurationElementCollection), AddItemName = "service")]
    public class ServiceConfigurationElementCollection : ConfigurationElementCollection
    {
        /// <summary>
        /// 插件存放路径
        /// </summary>
        [ConfigurationProperty("pluginPath", DefaultValue = "")]
        public string PluginPath
        {
            get
            {
                var pluginPath = $"{base["pluginPath"]}";
                if (!AbsolutePath)
                {
                    var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                    pluginPath = Path.Combine(baseDirectory, pluginPath);
                }
                return pluginPath;
            }
        }

        /// <summary>
        /// 是否是绝对路径
        /// </summary>
        [ConfigurationProperty("absolutePath", DefaultValue = false)]
        public bool AbsolutePath
        {
            get => Convert.ToBoolean(base["absolutePath"]);
        }
        
        /// <summary>
        /// 创建新的队列配置节点
        /// </summary>
        /// <returns></returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new ServiceConfigurationElement();
        }

        /// <summary>
        /// 获取队列配置Key
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ServiceConfigurationElement)element).Name;
        }

        /// <summary>
        /// 根据配置key索引相应配置
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public new ServiceConfigurationElement this[string name]
        {
            get
            {
                return (ServiceConfigurationElement)BaseGet(name);
            }
        }
    }
}
