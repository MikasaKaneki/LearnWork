using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace GameFramework
{
    /// <summary>
    /// 事件池
    /// </summary>
    /// <typeparam name="T">事件类型</typeparam>
    internal sealed partial class EventPool<T> where T : BaseEventArgs
    {
        private readonly Dictionary<int, EventHandler<T>> m_EventHandlers;

        private readonly Queue<Event> m_Events;

        private readonly EventPoolMode m_EventPoolMode;

        /// <summary>
        /// 初始化事件池的新实例
        /// </summary>
        /// <param name="mode">事件池模式</param>
        public EventPool(EventPoolMode mode)
        {
            m_EventHandlers = new Dictionary<int, EventHandler<T>>();
            m_Events = new Queue<Event>();
            m_EventPoolMode = mode;
        }

        /// <summary>
        /// 获取事件的数量
        /// </summary>
        public int Count
        {
            get { return m_Events.Count; }
        }

        /// <summary>
        /// 事件池轮询
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝的时间 单位秒</param>
        /// <param name="realElapseSeconds">真实的流逝时间 单位秒</param>
        public void Update(float elapseSeconds, float realElapseSeconds)
        {
            while (m_Events.Count > 0)
            {
                Event e = null;
                lock (m_Events)
                {
                    e = m_Events.Dequeue();
                }
                HandledEvent(e.Sender, e.EventArgs);
            }
        }

        /// <summary>
        /// 关闭并清理事件池
        /// </summary>
        public void Shutdown()
        {
            Clear();
            m_EventHandlers.Clear();
        }

        /// <summary>
        /// 清理事件
        /// </summary>
        private void Clear()
        {
            lock (m_Events)
            {
                m_Events.Clear();
            }
        }

        /// <summary>
        /// 检查订阅事件处理函数
        /// </summary>
        /// <param name="id">时间类型编号</param>
        /// <param name="handler">要检查的事件处理函数</param>
        /// <returns>是否存在事件处理函数</returns>
        public bool Check(int id, EventHandler<T> handler)
        {
            if (handler == null)
            {
                throw new GameFrameworkException("Event handler is invaild.");
            }
            EventHandler<T> handlers = null;
            if (!m_EventHandlers.TryGetValue(id, out handlers))
            {
                return false;
            }
            if (handlers == null)
            {
                return false;
            }
            foreach (EventHandler<T> i in handlers.GetInvocationList())
            {
                if (i == handler)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 订阅事件的处理函数
        /// </summary>
        /// <param name="id">事件类型编号</param>
        /// <param name="handler">要订阅的事件处理函数</param>
        public void Subscribe(int id, EventHandler<T> handler)
        {
            if (handler == null)
            {
                throw new GameFrameworkException("Event handler is invalid.");
            }
            EventHandler<T> eventHandler = null;
            if (!m_EventHandlers.TryGetValue(id, out eventHandler) || eventHandler == null)
            {
                m_EventHandlers[id] = handler;
            }
            else if ((m_EventPoolMode & EventPoolMode.AllowMultiHandler) == 0)
            {
                throw new GameFrameworkException(string.Format("Event '{0}' not allow multi handler.", id.ToString()));
            }
            else if ((m_EventPoolMode & EventPoolMode.AllowDuplicateHandler) == 0 && Check(id, handler))
            {
                throw new GameFrameworkException(string.Format("Event '{0}' not allow duplicate handler.",
                    id.ToString()));
            }
            else
            {
                eventHandler += handler;
                m_EventHandlers[id] = eventHandler;
            }
        }

        /// <summary>
        /// 取消订阅事件处理函数
        /// </summary>
        /// <param name="id">事件类型编号</param>
        /// <param name="handler">要取消订阅的时间处理函数</param>
        public void Unsubscribe(int id, EventHandler<T> handler)
        {
            if (handler == null)
            {
                throw new GameFrameworkException("Event handler is invalid.");
            }

            if (m_EventHandlers.ContainsKey(id))
            {
                m_EventHandlers[id] -= handler;
            }
        }

        /// <summary>
        /// 抛出事件，这个线程是安全的，即使不在主线程中抛出，也可保证在主线程中回调事件处理函数，但事件会在抛出后的下一帧分发。
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">事件参数</param>
        public void Fire(object sender, T e)
        {
            Event eventNode = new Event(sender, e);
            lock (m_Events)
            {
                m_Events.Enqueue(eventNode);
            }
        }

        /// <summary>
        /// 抛出事件立即模式，这个操作不是线程安全的，事件会立即分发。
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">事件参数</param>
        public void FireNow(object sender, T e)
        {
            HandledEvent(sender, e);
        }

        /// <summary>
        /// 处理时间结点
        /// </summary>
        /// <param name="sender">事件源</param>
        /// <param name="e">事件参数</param>
        private void HandledEvent(object sender, T e)
        {
            EventHandler<T> handlers = null;
            if (m_EventHandlers.TryGetValue(e.id, out handlers))
            {
                if (handlers != null)
                {
                    handlers(sender, e);
                }
            }

            ReferencePool.Release(e.GetType(), e);
            if (handlers == null && (m_EventPoolMode & EventPoolMode.AllowNoHandler) == 0)
            {
                throw new GameFrameworkException(string.Format("Event '{0}' not allow no handler.", e.id.ToString()));
            }
        }
    }
}