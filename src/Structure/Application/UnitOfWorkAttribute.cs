using System;

namespace Structure.Application
{
    public class UnitOfWorkAttribute : Attribute
    {
        public bool Enabled { get; set; } = true;
    }
}
