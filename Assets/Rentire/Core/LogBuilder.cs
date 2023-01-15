namespace Rentire.Core
{
    
    public class LogBuilder : ILogBuilder
    {
        private string message = string.Empty;
        private LogType logType;
        private LogColor logColor;

        private bool isBold = false;
        private bool isStated = false;
        public LogBuilder()
        {

        }
        public LogBuilder(string message, LogType logType = LogType.Info, LogColor color = LogColor.Black)
        {
            this.message = message;
            this.logType = logType;
            this.logColor = color;
        }
        public ILogBuilder Bold()
        {
            isBold = true;
            return this;
        }

        
        public void Build()
        {
            #if UNITY_EDITOR || DEBUG
            if (isStated)
            {
                message = $"************************* {message} *************************";
            }
            if (isBold)
                message = $"<b>{message}</b>";

            //Colorize log
            var color = logColor.ToString().ToLower();
            message = $"<color=\"{color}\"> {message} </color>";

            switch(logType)
            {
                case LogType.Info:
                    UnityEngine.Debug.Log(message);
                    break;
                case LogType.Warning:
                    UnityEngine.Debug.LogWarning(message);
                    break;
                case LogType.Error:
                    UnityEngine.Debug.LogError(message);
                    break;
            }

            #endif

        }

        public ILogBuilder SetMessage(string message)
        {
            this.message = message;
            return this;
        }

        public ILogBuilder SetLogType(LogType logType)
        {
            this.logType = logType;
            return this;
        }
        public ILogBuilder SetColor(LogColor color)
        {
            this.logColor = color;
            return this;
        }

        public ILogBuilder Stated()
        {
            isStated = true;
            return this;
        }
    }
    public enum LogType{
        Info,
        Error,
        Warning
    }

    public enum LogColor
    {
        Black,
        Red,
        Yellow,
        Green,
        Blue
    }
}