#if !COMPATIBLE
using MySqlConnector;
#else
using MySql.Data.MySqlClient;
#endif
using SQLib;

namespace LinqSharp.EFCore.Data.Test;

public class ApplicationDbScope : SqlScope<ApplicationDbScope, MySqlConnection, MySqlCommand, MySqlParameter>
{
    public const string CONNECT_STRING = "server=127.0.0.1;port=33306;user=root;pwd=root;database=linqsharp";
    public static ApplicationDbScope UseDefault() => new(new MySqlConnection(CONNECT_STRING));

    public ApplicationDbScope(MySqlConnection model) : base(model) { }
}
