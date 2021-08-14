using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TokenBasedAuth_NetCore.Entities.Trivia;

namespace TokenBasedAuth_NetCore.Services.Trivia
{
    public interface ITriviaService
    {
        IEnumerable<Category> GetAll();

    }
}
