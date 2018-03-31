using RabbitMQ.Expressions.Configuration;
using RabbitMQ.Expressions.Core;
using RabbitMQ.Expressions.Interface;
using RabbitMQ.Expressions.Service;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace RabbitMQ.Expressions
{
    /// <summary>
    /// RabbitMQ服务管理者
    /// </summary>
    public class HandlerManager
    {
        /// <summary>
        /// 禁止使用new创建对象
        /// </summary>
        private HandlerManager() { }

        /// <summary>
        /// 异常处理事件
        /// </summary>
        public event OnExceptionHandler Exception;

        /// <summary>
        /// 对象锁
        /// </summary>
        protected static object _lock = new object();

        /// <summary>
        /// 自定义配置文件节点名称
        /// </summary>
        protected const string SECTION_NAME = "rabbitmqSettings";

        /// <summary>
        /// 实例
        /// </summary>
        public static HandlerManager Instance = new HandlerManager();

        /// <summary>
        /// 实例对象
        /// </summary>
        private static Dictionary<string, BaseHandler> instanceDic = new Dictionary<string, BaseHandler>();

        /// <summary>
        /// 配置信息
        /// </summary>
        protected static RabbitMQConfigurationSection configSection = (RabbitMQConfigurationSection)ConfigurationManager.GetSection(SECTION_NAME);

        /// <summary>
        /// 启动
        /// </summary>
        public void OnStart()
        {
            try
            {
                if (configSection == null)
                {
                    throw new TypeInitializationException("rabbitmqSettings", null);
                }

                var services = configSection.Services;
                foreach (ServiceConfigurationElement service in services)
                {
                    var fullName = ReflectionHelper.GetFullName(service.NameSpace, service.ClassName);
                    if (!instanceDic.ContainsKey(fullName))
                    {
                        lock (_lock)
                        {
                            if (!instanceDic.ContainsKey(fullName))
                            {
                                var instance = (BaseHandler)Singleton.GetInstance(services.PluginPath, service.AssemblyName, service.NameSpace, service.ClassName);
                                instance.OnStart(configSection.ConnectionStrings, service);
                                instanceDic.Add(fullName, instance);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //执行异常处理事件
                OnException(this, ex);
            }
        }

        /// <summary>
        /// 重启服务
        /// </summary>
        public void ReStart()
        {
            if (instanceDic == null || !instanceDic.Any())
            {
                //没有则不需要停止
                return;
            }

            foreach (var instance in instanceDic)
            {
                var service = instance.Value;
                service.ReStart();
            }
        }

        /// <summary>
        /// 停止服务
        /// </summary>
        public void OnStop()
        {
            if (instanceDic == null || !instanceDic.Any())
            {
                //没有则不需要停止
                return;
            }

            foreach (var instance in instanceDic)
            {
                var service = instance.Value;
                service.OnStop();
            }
        }

        /// <summary>
        /// 异常处理类
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="ex"></param>
        public virtual void OnException(object sender, Exception ex)
        {
            Exception?.Invoke(sender, ex);
        }
    }
}