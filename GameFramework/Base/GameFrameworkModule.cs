using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameFramework
{
    internal abstract class GameFrameworkModule
    {
        /// <summary>
        /// 获取游戏框架模块的优先级
        /// </summary>
        /// <remarks>优先级高的模块会先轮询，并且关闭操作会后进行</remarks>
        internal virtual int Priority
        {
            get { return 0; }
        }

        /// <summary>
        /// 游戏框架的模块轮询
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间 单位是秒</param>
        /// <param name="realElapseSeconds">真实流逝的时间 单位是秒</param>
        internal abstract void Update(float elapseSeconds, float realElapseSeconds);

        /// <summary>
        /// 关闭并且清理游戏框架模块
        /// </summary>
        internal abstract void Shutdown();
    }
}