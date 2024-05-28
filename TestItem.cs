using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringContainsTest
{
    public class TestItem : ICanContainsLookup
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string ContainsSearchText => Name;

        public override string ToString()
        {
            return $"Id: {Id} Name: {Name}";
        }
    }
}
