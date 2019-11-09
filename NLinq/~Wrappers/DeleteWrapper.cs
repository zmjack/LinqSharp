using Microsoft.EntityFrameworkCore;

namespace NLinq
{
    public class DeleteWrapper<TEntity>
        where TEntity : class
    {
        public WhereWrapper<TEntity> WhereWrapper { get; }

        public DeleteWrapper(WhereWrapper<TEntity> whereWrapper)
        {
            WhereWrapper = whereWrapper;
        }

        public string ToSql()
        {
            return $"DELETE FROM {WhereWrapper.TableName} WHERE {WhereWrapper.WhereString};";
        }

        public int Save() => WhereWrapper.DbContext.Database.ExecuteSqlCommand(ToSql());
    }
}
