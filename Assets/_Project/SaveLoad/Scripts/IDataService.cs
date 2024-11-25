using System.Collections.Generic;

namespace Paerux.Persistence
{
    public interface IDataService
    {
        bool Save(string relativePath, ISaveData data, bool encrypt = false);
        ISaveData Load(string relativePath, bool decrypt = false);
        bool Delete(string relativePath);
        IEnumerable<ISaveData> ListSaves();
    }
}
