using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using Northwnd;
using System.Threading;
using System.Threading.Tasks;

namespace LinqSharp.Data.Test
{
    public class ApplicationDbScope : SqlScope<MySqlConnection, MySqlCommand, MySqlParameter>
    {
        public ApplicationDbScope() : this(new MySqlConnection(ApplicationDbContext.CONNECT_STRING)) { }
        public ApplicationDbScope(MySqlConnection model) : base(model) { }
    }

}
