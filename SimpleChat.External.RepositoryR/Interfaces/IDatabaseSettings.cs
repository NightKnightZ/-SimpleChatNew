using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleChat.Messaging.Database.Sqlite.Interfaces
{
    public interface IDatabaseSettings
    {
        string ConnectionString { get; set; }
    }
}
