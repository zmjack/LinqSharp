using System;
using System.Collections.Generic;
using System.Text;

namespace LinqSharp.Cli
{
    public class DbTable
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public DbTableField[] TableFields { get; set; }
    }
}
