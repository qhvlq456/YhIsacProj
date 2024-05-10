using System;

namespace YhProj.Game.Log
{
    public static class YhLogger
    {
        public delegate void LogWriteHandler(Level level, string message);

        public enum Level
        {
            Never = -99, // 출력하지 않음
            Debug = 1,
            Info = 0,
            Warn = -1,
            Error = -2,
            Any = 99 // 항상 출력
        }

        public static Level LogLevel { get; set; } = Level.Info;
        public static LogWriteHandler Write { get; set; }

        public static void Debug(string message)
        {
            if (Level.Debug > LogLevel) return;
            message = $"[Yh] {message}";
            UnityEngine.Debug.Log(message);
            Write?.Invoke(Level.Debug, message);
        }

        public static void Info(string message)
        {
            if (Level.Info > LogLevel) return;
            message = $"[Yh] {message}";
            UnityEngine.Debug.Log(message);
            Write?.Invoke(Level.Info, message);
        }

        public static void Warn(string message)
        {
            if (Level.Warn > LogLevel) return;
            message = $"[Yh] {message}";
            UnityEngine.Debug.LogWarning(message);
            Write?.Invoke(Level.Warn, message);
        }

        public static void Warn(Exception e)
        {
            if (Level.Warn > LogLevel) return;
            var message = $"[Yh] {e.Message}";
            UnityEngine.Debug.LogWarning($"{message}, {e}");
            Write?.Invoke(Level.Warn, message);
        }

        public static void Error(string message)
        {
            if (Level.Error > LogLevel) return;
            message = $"[Yh] {message}";
            UnityEngine.Debug.LogError(message);
            Write?.Invoke(Level.Error, message);
        }

        public static void Error(Exception e)
        {
            if (Level.Error > LogLevel) return;
            var message = $"[Yh] {e.Message}";
            UnityEngine.Debug.LogException(e);
            Write?.Invoke(Level.Error, message);
        }
    }
}

