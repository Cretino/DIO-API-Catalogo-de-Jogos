using CatalogoDeJogos.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CatalogoDeJogos.Repositories
{
    public interface IJogoRepository : IDisposable
    {
        Task<List<Jogo>> Obter(int Pagina, int Quantidade);
        Task<Jogo> Obter(Guid Id);
        Task<List<Jogo>> Obter(string Nome, string Produtora);
        Task Inserir(Jogo NovoJogo);
        Task Atualizar(Jogo JogoExistente);
        Task Remover(Guid Id);
    }
}
