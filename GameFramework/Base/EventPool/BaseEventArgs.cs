namespace GameFramework
{
    public abstract class BaseEventArgs : GameFrameworkEventArgs, IReference
    {
        /// <summary>
        /// 获取类型的编号
        /// </summary>
        public abstract int id { get; }

        /// <summary>
        /// 清理引用
        /// </summary>
        public abstract void Clear();
    }
}