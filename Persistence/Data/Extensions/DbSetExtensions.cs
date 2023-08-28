using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Data.Extensions
{
    public static class DbSetExtensions
    {
        public static void RemoveIf<T>(this DbSet<T> theDbSet, Expression<Func<T, bool>> thePredicate) where T : class
        {
            var entities = theDbSet.Where(thePredicate);
            theDbSet.RemoveRange(entities);
        }
    }
}
