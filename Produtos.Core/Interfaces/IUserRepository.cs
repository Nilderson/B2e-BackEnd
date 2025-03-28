using Produtos.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Produtos.Core.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByLogin(string login);
        Task Add(User usuario);
    }
}
