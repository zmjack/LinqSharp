using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LinqSharp.EFCore.Data
{
    public class YearMonthModel
    {
        [Key]
        public Guid Id { get; set; }

        public int Year { get; set; }

        public int Month { get; set; }

        public int Day { get; set; }

    }
}
