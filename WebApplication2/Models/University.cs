using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2.Models
{
    public class University
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public User[] Users { get; set; }
    }
}
