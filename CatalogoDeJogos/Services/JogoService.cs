using CatalogoDeJogos.InputModel;
using CatalogoDeJogos.Repositories;
using CatalogoDeJogos.ViewModel;
using CatalogoDeJogos.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CatalogoDeJogos.Exceptions;

namespace CatalogoDeJogos.Services
{
    public class JogoService : IJogoService
    {
        private readonly IJogoRepository _jogoRepository;

        public JogoService(IJogoRepository jogoRepository)
        {
            _jogoRepository = jogoRepository;
        }

        public async Task Atualizar(Guid Id, JogoInputModel Jogo)
        {
            var JogoObtido = await _jogoRepository.Obter(Id);

            if (JogoObtido.Id == Guid.Empty)
                throw new JogoNaoCadastradoException();

            JogoObtido.Nome = Jogo.Nome;
            JogoObtido.Produtora = Jogo.Produtora;
            JogoObtido.Preco = Jogo.Preco;

            var ListaDeJogos = await _jogoRepository.Obter(JogoObtido.Nome, JogoObtido.Produtora);

            if (ListaDeJogos.Count > 0)
                throw new JogoJaCadastradoException();

            await _jogoRepository.Atualizar(JogoObtido);
        }

        public async Task Atualizar(Guid Id, double Preco)
        {
            var JogoObtido = await _jogoRepository.Obter(Id);

            if (JogoObtido.Id == Guid.Empty)
                throw new JogoNaoCadastradoException();

            JogoObtido.Preco = Preco;

            await _jogoRepository.Atualizar(JogoObtido);
        }

        public async Task Atualizar(Guid Id, string Nome, bool IsProdutora)
        {
            var JogoObtido = await _jogoRepository.Obter(Id);

            if (JogoObtido.Id == Guid.Empty)
                throw new JogoNaoCadastradoException();

            if (IsProdutora == true)
                JogoObtido.Produtora = Nome;
            else
                JogoObtido.Nome = Nome;

            var ListaDeJogos = await _jogoRepository.Obter(JogoObtido.Nome, JogoObtido.Produtora);

            if (ListaDeJogos.Count > 0)
                throw new JogoJaCadastradoException();

            await _jogoRepository.Atualizar(JogoObtido);
        }

        public async Task<JogoViewModel> Inserir(JogoInputModel Jogo)
        {
            var JogoObtido = await _jogoRepository.Obter(Jogo.Nome, Jogo.Produtora);

            if (JogoObtido.Count > 0)
                throw new JogoJaCadastradoException();

            var NovoJogo = new Jogo
            {
                Id = Guid.NewGuid(),
                Nome = Jogo.Nome,
                Produtora = Jogo.Produtora,
                Preco = Jogo.Preco
            };

            await _jogoRepository.Inserir(NovoJogo);

            return new JogoViewModel
            {
                Id = NovoJogo.Id,
                Nome = NovoJogo.Nome,
                Produtora = NovoJogo.Produtora,
                Preco = NovoJogo.Preco
            };
        }

        public async Task<List<JogoViewModel>> Obter(int Pagina, int Quantidade)
        {
            var lista = await _jogoRepository.Obter(Pagina, Quantidade);

            return lista.Select(JogoObtido => new JogoViewModel
            {
                Id = JogoObtido.Id,
                Nome = JogoObtido.Nome,
                Produtora = JogoObtido.Produtora,
                Preco = JogoObtido.Preco
            }).ToList();
        }

        public async Task<JogoViewModel> Obter(Guid Id)
        {
            var JogoObtido = await _jogoRepository.Obter(Id);

            // Se o jogo não tem 'Id', é por que ele não existe.
            if (JogoObtido.Id == Guid.Empty)
                return null;

            return new JogoViewModel
            {
                Id = JogoObtido.Id,
                Nome = JogoObtido.Nome,
                Produtora = JogoObtido.Produtora,
                Preco = JogoObtido.Preco
            };
        }

        public async Task Remover(Guid Id)
        {
            var JogoObtido = await _jogoRepository.Obter(Id);

            // Se o jogo não tem 'Id', é por que ele não existe.
            if (JogoObtido.Id == Guid.Empty)
                throw new JogoNaoCadastradoException();

            await _jogoRepository.Remover(Id);
        }

        public void Dispose()
        {
            _jogoRepository?.Dispose();
        }
    }
}
