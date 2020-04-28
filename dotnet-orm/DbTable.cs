namespace LinqSharp.Cli
{
    public class DbTable
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public DbTableField[] TableFields { get; set; }
    }
}
