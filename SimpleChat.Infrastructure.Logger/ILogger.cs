using System;

namespace SimpleChat.Infrastructure.Logger
{
    public interface ILogger
    {
        void Info(string message);
        void Warn(string message);
        void Error(string message);
        void Error(Exception e, string message);
    }
}
