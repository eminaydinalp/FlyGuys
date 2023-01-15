namespace Rentire.Core {
    using System.Diagnostics;
    using Debug = UnityEngine.Debug;

    public static class Log {
        [Conditional ("UNITY_EDITOR"), Conditional ("DEBUG")]
        public static void Info (string message) {
            var callerMethodName = new StackFrame (1)?.GetMethod ()?.ReflectedType?.Name;
            Debug.Log ((callerMethodName ?? "Not Found") + ": " + message);
        }

        [Conditional("UNITY_EDITOR"), Conditional("DEBUG")]
        public static void Info(string message, params object[] parameters)
        {
            var callerMethodName = new StackFrame(1)?.GetMethod()?.ReflectedType?.Name;
            Debug.Log((callerMethodName ?? "Not Found") + ": " + string.Format( message, parameters));
        }

        [Conditional ("UNITY_EDITOR"), Conditional ("DEBUG")]
        public static void InfoBold (string message) {
            Debug.Log ("<b>" + message + "</b>");
        }

        [Conditional ("UNITY_EDITOR"), Conditional ("DEBUG")]
        public static void InfoStated (string message) {
            Debug.Log ($"<b> **************************** {message} **************************** </b>");
        }

        [Conditional ("UNITY_EDITOR"), Conditional ("DEBUG")]
        public static void InfoFormat (string template, params object[] args) {
            var message = string.Format (template, args);
            var callerMethodName = new StackFrame (1)?.GetMethod ()?.ReflectedType?.Name;
            Debug.Log ((callerMethodName ?? "Not Found") + ": " + message);
        }

        public static void Warning (string message) {
            var callerMethodName = new StackFrame (1)?.GetMethod ()?.ReflectedType?.Name;
            Debug.LogWarning ((callerMethodName ?? "Not Found") + ": " + message);
        }

        public static void Error (string message) {
            var callerMethodName = new StackFrame (1)?.GetMethod ()?.ReflectedType?.Name;
            Debug.LogError ((callerMethodName ?? "Not Found") + ": " + message);
        }

        public static void Error(string message, params object[] parameters)
        {
            var callerMethodName = new StackFrame(1)?.GetMethod()?.ReflectedType?.Name;
            Debug.LogError((callerMethodName ?? "Not Found") + ": " + string.Format(message, parameters));
        }

    }
}