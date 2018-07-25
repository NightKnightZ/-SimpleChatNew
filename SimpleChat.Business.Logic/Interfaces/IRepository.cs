using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleChat.Messaging.Interfaces
{
    public interface IRepository<T>
    {
        void Add(T item);
        T GetItem(int id);
        void Update(T item);
        void DeleteItem(int id);
        void Save();
    }
}
