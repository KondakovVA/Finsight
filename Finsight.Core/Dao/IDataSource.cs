using Microsoft.EntityFrameworkCore;

namespace Finsight.Core.Dao
{
    public interface IDataSource
    {
        DbContext GetContext();
    }
}
