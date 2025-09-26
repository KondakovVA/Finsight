using System;
using Microsoft.EntityFrameworkCore;

namespace Finsight.Core.Dao.EF
{
    public class EfDataSource : IDataSource
    {
        private readonly EfContext _context;

        public EfDataSource(EfContext context)
        {
            ArgumentNullException.ThrowIfNull(context);
            _context = context;
        }

        public DbContext GetContext()
        {
            return _context;
        }
    }
}
