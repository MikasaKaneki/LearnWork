using System;
using System.Runtime.Serialization;

namespace GameFramework
{
    /// <summary>
    /// 游戏框架的异常类
    /// </summary>
    class GameFrameworkException : Exception
    {
        /// <summary>
        /// 初始化游戏框架异常类的新实例
        /// </summary>
        public GameFrameworkException() : base()
        {
        }

        /// <summary>
        /// 使用指定错误消息初始化游戏框架异常类的新实例
        /// </summary>
        /// <param name="message">描述错误的消息</param>
        public GameFrameworkException(string message) : base(message)
        {
        }

        /// <summary>
        /// 使用指定错误的消息和对作为此异常原因的内部异常的引用来初始化游戏框架异常类的新实例
        /// </summary>
        /// <param name="message">解释异常原因的错误消息。</param>
        /// <param name="innerException">导致当前异常的异常，如果innerException的参数不为空，则在处理内部的catch块中引发当前的异常</param>
        public GameFrameworkException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// 用序列化数据初始化游戏框架异常类的新实例
        /// </summary>
        /// <param name="info">存有有关所引发异常的序列化的数据对象</param>
        /// <param name="context">包含有关源或目标的上下文信息</param>
        public GameFrameworkException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}