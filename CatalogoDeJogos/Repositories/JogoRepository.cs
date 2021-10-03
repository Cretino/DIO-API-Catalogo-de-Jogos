using CatalogoDeJogos.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CatalogoDeJogos.Repositories
{
    public class JogoRepository : IJogoRepository
    {
        private static Guid GetNewId;

        private static Dictionary<Guid, Jogo> ListaDeJogos = new Dictionary<Guid, Jogo>()
        {
            { (GetNewId = Guid.NewGuid()), new Jogo { Id = GetNewId, Nome = "Meu Jogo Teste 1", Produtora = "Produtora Teste 1", Preco = 100 } },
            { (GetNewId = Guid.NewGuid()), new Jogo { Id = GetNewId, Nome = "Meu Jogo Teste 2", Produtora = "Produtora Teste 1", Preco = 50 } },
            { (GetNewId = Guid.NewGuid()), new Jogo { Id = GetNewId, Nome = "Meu Jogo Teste 3", Produtora = "Produtora Teste 2", Preco = 25 } }
        };

        public Task Atualizar(Jogo JogoExistente)
        {
            ListaDeJogos[JogoExistente.Id] = JogoExistente;

            return Task.CompletedTask;
        }

        public void Dispose()
        {
        }

        public Task Inserir(Jogo NovoJogo)
        {
            ListaDeJogos.Add(NovoJogo.Id, NovoJogo);

            return Task.CompletedTask;
        }

        public Task<List<Jogo>> Obter(int Pagina, int Quantidade)
        {
            return Task.FromResult(ListaDeJogos.Values.Skip((Pagina - 1) * Quantidade).Take(Quantidade).ToList());
        }

        public Task<Jogo> Obter(Guid Id)
        {
            // Se o jogo não for encontrado, retornamos um tipo Jogo sem nenhuma informação.
            if (!ListaDeJogos.ContainsKey(Id))
            {
                return Task.FromResult(new Jogo
                {
                    Id = Guid.Empty,
                    Nome = string.Empty,
                    Produtora = string.Empty,
                    Preco = double.NaN
                });
            }

            return Task.FromResult(ListaDeJogos[Id]);
        }

        public Task<List<Jogo>> Obter(string Nome, string Produtora)
        {
            return Task.FromResult(ListaDeJogos.Values.Where(JogoObtido => JogoObtido.Nome.Equals(Nome) && JogoObtido.Produtora.Equals(Produtora)).ToList());
        }

        public Task Remover(Guid Id)
        {
            ListaDeJogos.Remove(Id);

            return Task.CompletedTask;
        }
    }
}
