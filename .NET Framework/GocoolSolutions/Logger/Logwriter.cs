using System.Collections.Generic;
using NLog;
/// <summary>
/// 存在一个字典，key:模块名，value：模块对应的logger
/// 写入日志时，若模块不存在，则创建对应的logger并放到字典中
/// 我们从字典获取logger，而不是每次写日志的时候都new出一个logger
/// 提高日志写入的效率
/// </summary>
namespace GocoolSolutions.MyLoggers
{
    public static class Logwriter
    {
        //读写器名字--读写器
        private static Dictionary<string, Logger> existingLoggers = new Dictionary<string, Logger>();
        //用于锁字典
        private static object locker = new object();
        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="msg">日志内容</param>
        /// <param name="logger">读写器名</param>
        /// <param name="logLevel">错误等级</param>
        public static void WriteLog(string msg, string logger = "default", LogLevel logLevel = LogLevel.Info)
        {
            if (!existingLoggers.ContainsKey(logger))
            {
                lock (locker)
                {
                    if (!existingLoggers.ContainsKey(logger))
                    {
                        existingLoggers[logger] = LogManager.GetLogger(logger);
                    }
                }
            }
            switch (logLevel)
            {
                case LogLevel.Trace:
                    existingLoggers[logger].Trace(msg);
                    break;
                case LogLevel.Debug:
                    existingLoggers[logger].Debug(msg);
                    break;
                case LogLevel.Info:
                    existingLoggers[logger].Info(msg);
                    break;
                case LogLevel.Warn:
                    existingLoggers[logger].Warn(msg);
                    break;
                case LogLevel.Error:
                    existingLoggers[logger].Error(msg);
                    break;
                case LogLevel.Fatal:
                    existingLoggers[logger].Fatal(msg);
                    break;
            }
        }
    }

    /// <summary>
    /// 日志消息的等级
    /// </summary>
    public enum LogLevel
    {
        Trace,
        Debug,
        Info,
        Warn,
        Error,
        Fatal
    }
}
