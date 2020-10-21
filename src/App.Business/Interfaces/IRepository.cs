using AppMvcBasica.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

// Conversa com a camada de acesso a dados através dessa interface que será injetada por dependência fazendo relação a uma classe de repositório.

namespace DevIO.Business.Interfaces
{
    //Interface IDisposable obriga que o repositório faça o dispose para liberar memória
    public interface IRepository<TEntity>: IDisposable  where TEntity :Entity //Só pode ser utilizada ser for uma classe filha de Entity
    {
        Task Adicionar(TEntity entity);
        Task<TEntity> ObterPorId(Guid id);
        Task<List<TEntity>> ObterTodos();
        Task Atualizar(TEntity entity);
        Task Remover(Guid id);
        Task<IEnumerable<TEntity>> Buscar(Expression<Func<TEntity, bool>> predicate);//Buscar qualquer entidade por qualquer parâmetro
        Task<int> SaveChanges();
    }
}
