namespace Rentire.Core
{

	using System;
	public interface ILogBuilder
	{
		ILogBuilder Bold();
		ILogBuilder Stated();
        ILogBuilder SetMessage(string message);
        ILogBuilder SetLogType(LogType logType);
        ILogBuilder SetColor(LogColor color);
		void Build();
	}
}