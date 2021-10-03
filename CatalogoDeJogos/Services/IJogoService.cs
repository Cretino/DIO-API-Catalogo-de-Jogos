using CatalogoDeJogos.InputModel;
using CatalogoDeJogos.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CatalogoDeJogos.Services
{
    public interface IJogoService : IDisposable
    {
        Task<List<JogoViewModel>> Obter(int Pagina, int Quantidade);
        Task<JogoViewModel> Obter(Guid Id);
        Task<JogoViewModel> Inserir(JogoInputModel Jogo);
        Task Atualizar(Guid Id, JogoInputModel Jogo);
        Task Atualizar(Guid Id, double Preco);
        Task Atualizar(Guid Id, string Nome, bool IsProdutora);
        Task Remover(Guid Id);
    }
}
