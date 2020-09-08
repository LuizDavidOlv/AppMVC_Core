using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace AppMvcBasica.Models
{
    public class Fornecedor : Entity
    {
        public string Nome { get; set; }

        public string Documento { get; set; }

        public TipoFornecedor TipoFornecedor { get; set; }

        public Endereco Endereco { get; set; }

        public bool Ativo { get; set; }


        /*
        EF Relations
        É preciso estabelecer um tipo de relacionamento visível pra o Entity Framework.
        O Entity Framework pecisa entender, via declaração de propriedade, que o fornecedor tem realação de um para muitos com produto
        */
        public IEnumerable<Produto> Produtos { get; set; }
    }
}
