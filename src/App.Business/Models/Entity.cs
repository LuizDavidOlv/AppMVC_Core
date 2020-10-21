using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Threading.Tasks;

namespace AppMvcBasica.Models
{
    public abstract class Entity //Quando a classe é abstrata, não pode ser instanciada, precisa ser herdada.
    {
        public Guid Id { get; set; }
        

        protected Entity()
        {
            Id = Guid.NewGuid();
        }
    }
}
