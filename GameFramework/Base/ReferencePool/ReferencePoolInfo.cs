using System;
using System.Collections.Generic;

namespace GameFramework
{
    /// <summary>
    /// 引用池信息
    /// </summary>
    public sealed class ReferencePoolInfo
    {
        private readonly string m_TypeName; //引用池的名称
        private readonly int m_UnusedReferenceCount; //未使用的引用数量
        private readonly int m_UsingReferenceCount; //正在使用的引用数量
        private readonly int m_AcquireReferenceCount; //获取引用数量
        private readonly int m_ReleaseReferenceCount; //归还引用数量
        private readonly int m_AddReferenceCount; // 增加引用数量
        private readonly int m_RemoveReferenceCount; //移除引用数量

        /// <summary>
        /// 初始化引用池信息的新实例
        /// </summary>
        /// <param name="typeName">引用池类型名称</param>
        /// <param name="unusedReferenceCount">未使用引用数量</param>
        /// <param name="usingReferenceCount">正在使用的引用数量</param>
        /// <param name="acquireReferenceCount">获取引用数量</param>
        /// <param name="releaseReferenceCount">归还引用数量</param>
        /// <param name="addReferenceCount">增加引用数量</param>
        /// <param name="removeReferenceCount">移除引用数量</param>
        public ReferencePoolInfo(string typeName, int unusedReferenceCount, int usingReferenceCount,
            int acquireReferenceCount, int releaseReferenceCount, int addReferenceCount, int removeReferenceCount)
        {
            m_TypeName = typeName;
            m_UnusedReferenceCount = unusedReferenceCount;
            m_UsingReferenceCount = usingReferenceCount;
            m_AcquireReferenceCount = acquireReferenceCount;
            m_ReleaseReferenceCount = releaseReferenceCount;
            m_AddReferenceCount = addReferenceCount;
            m_RemoveReferenceCount = removeReferenceCount;
        }

        /// <summary>
        /// 获取引用池类型的名称
        /// </summary>
        public string TypeName
        {
            get { return m_TypeName; }
        }

        /// <summary>
        /// 获取未使用的引用数量
        /// </summary>
        public int UnusedReferenceCount
        {
            get { return m_UnusedReferenceCount; }
        }

        /// <summary>
        /// 获取正在使用的引用数量
        /// </summary>
        public int UsingReferenceCount
        {
            get { return m_UsingReferenceCount; }
        }

        /// <summary>
        /// 获取获取引用数量。
        /// </summary>
        public int AquireReferenceCount
        {
            get { return m_AcquireReferenceCount; }
        }
    }
}