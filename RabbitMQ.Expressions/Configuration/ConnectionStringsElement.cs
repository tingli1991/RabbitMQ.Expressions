using System;
using System.Configuration;

namespace RabbitMQ.Expressions.Configuration
{
    /// <summary>
    /// 连接字符串
    /// </summary>
    public class ConnectionStringsElement : ConfigurationElement
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [ConfigurationProperty("username", DefaultValue = "guest")]
        public string UserName
        {
            get => $"{base["username"]}";
        }

        /// <summary>
        /// 密码
        /// </summary>
        [ConfigurationProperty("password", DefaultValue = "guest")]
        public string Password
        {
            get => $"{base["password"]}";
        }

        /// <summary>
        /// 主机地址
        /// </summary>
        [ConfigurationProperty("host", DefaultValue = "127.0.0.1")]
        public string Host
        {
            get => $"{base["host"]}";
        }

        /// <summary>
        /// 监听端口
        /// </summary>
        [ConfigurationProperty("port", DefaultValue = 5672)]
        public int Port
        {
            get => Convert.ToInt32(base["port"]);
        }

        /// <summary>
        /// 连接超时时间（单位：秒，默认：30秒）
        /// </summary>
        [ConfigurationProperty("timeout", DefaultValue = (ushort)30)]
        public ushort TimeOut
        {
            get => Convert.ToUInt16(base["timeout"]);
        }
    }
}