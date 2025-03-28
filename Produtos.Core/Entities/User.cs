using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Produtos.Core.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Senha { get; set; }
        public DateTime DataCreate{ get; set; } = DateTime.UtcNow;
    }
}
