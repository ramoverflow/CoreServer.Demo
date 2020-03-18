using System;

namespace CoreServer.Entity.MySqlDb
{
    public class Product
    {
        public Guid Id { get; set; }
        
        public string Name { get; set; }
    }
}