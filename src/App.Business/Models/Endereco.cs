using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace AppMvcBasica.Models
{
    public class Endereco : Entity
    {
        // Colocar Id no final já faz com seja a chave estranjeira. Fará o relacionamento do endereço com o fornecedor.
        public Guid FornecedorId { get; set; }

        public string Logradouro { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Cep { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }

        //EF realation
        public Fornecedor Fornecedor { get; set; }

    }
}
