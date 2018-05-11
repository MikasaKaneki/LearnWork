﻿using System;
using System.Collections.Generic;

namespace GameFramework
{
    /// <summary>
    ///游戏框架的入口
    /// </summary>
    public static class GameFrameworkEntry
    {
        private const string GameFrameworkVersion = "3.1.1";

        private static readonly LinkedList<GameFrameworkModule> s_GameFrameworkModules =
            new LinkedList<GameFrameworkModule>();

        public static string Version
        {
            get { return GameFrameworkVersion; }
        }

        /// <summary>
        /// 所有游戏框架模块轮询。
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
        /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
        public static void Update(float elapseSeconds, float realElapseSeconds)
        {
            foreach (GameFrameworkModule module in s_GameFrameworkModules)
            {
                module.Update(elapseSeconds, realElapseSeconds);
            }
        }

        /// <summary>
        /// 关闭并清理所有的游戏框架模块
        /// </summary>
        public static void Shutdown()
        {
            for (LinkedListNode<GameFrameworkModule> current = s_GameFrameworkModules.Last;
                current != null;
                current = current.Previous)
            {
                current.Value.Shutdown();
            }
            s_GameFrameworkModules.Clear();
            //TODO 清理反射池和清空log辅助工具
        }

        /// <summary>
        /// 获取游戏框架模块
        /// </summary>
        /// <typeparam name="T">要获取的游戏框架模块类型</typeparam>
        /// <returns>要获取的游戏框架模块</returns>
        /// <remarks> 如果要获取的框架模块不存在的话，则自动创建该游戏框架模块</remarks>
        public static T GetModule<T>() where T : class
        {
            Type interfaceType = typeof(T);
            if (!interfaceType.IsInterface)
            {
                throw new GameFrameworkException(String.Format("You must get module by interface,but '{0}' is not.",
                    interfaceType.FullName));
            }

            if (!interfaceType.FullName.StartsWith("GameFramework."))
            {
                throw new GameFrameworkException(string.Format("You must get a game framework module,but '{0}' is not.",
                    interfaceType.FullName));
            }


            string moduleName = string.Format("{0}.{1}", interfaceType.Namespace, interfaceType.Name.Substring(1));

            Type moduleType = Type.GetType(moduleName);
            if (moduleType == null)
            {
                throw new GameFrameworkException(string.Format("Can not find Game Framework module type '{0}'.",
                    moduleName));
            }
            return GetModule(moduleType) as T;
        }

        /// <summary>
        /// 获取游戏框架模块。
        /// </summary>
        /// <param name="moduleType">要获取的游戏框架模块类型</param>
        /// <returns>要获取游戏框架</returns>
        /// <remarks>如果要获取的游戏框架模块不存在，则自动创建该游戏框架模块</remarks>
        public static GameFrameworkModule GetModule(Type moduleType)
        {
            foreach (GameFrameworkModule module in s_GameFrameworkModules)
            {
                if (module.GetType() == moduleType)
                {
                    return module;
                }
            }
            return CreateModule(moduleType);
        }

        /// <summary>
        /// 创建游戏模块框架
        /// </summary>
        /// <param name="moduleType">要创建的游戏模块类型</param>
        /// <returns>要创建的游戏框架模块</returns>
        private static GameFrameworkModule CreateModule(Type moduleType)
        {
            GameFrameworkModule module = (GameFrameworkModule) Activator.CreateInstance(moduleType);
            if (module == null)
            {
                throw new GameFrameworkException(
                    string.Format("Can not create module '{0}'", module.GetType().FullName));
            }
            LinkedListNode<GameFrameworkModule> current = s_GameFrameworkModules.First;
            while (current != null)
            {
                if (module.Priority > current.Value.Priority)
                {
                    break;
                }
                current = current.Next;
            }
            if (current != null)
            {
                s_GameFrameworkModules.AddBefore(current, module);
            }
            else
            {
                s_GameFrameworkModules.AddLast(module);
            }
            return module;
        }
    }
}