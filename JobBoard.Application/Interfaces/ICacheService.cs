using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobBoard.Application.Interfaces
{
    public interface ICacheService
    {
        T? GetData<T>(string key);
        bool SetData<T>(string key, T value, TimeSpan expiration);
        bool RemoveData(string key);
    }
}
