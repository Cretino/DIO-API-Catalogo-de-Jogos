using CatalogoDeJogos.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace CatalogoDeJogos.Repositories
{
    public class JogoSqlServerRepository : IJogoRepository
    {
        private readonly SqlConnection SqlServerConnection;

        public JogoSqlServerRepository(IConfiguration Configuration)
        {
            SqlServerConnection = new SqlConnection(Configuration.GetConnectionString("Default"));
        }

        public async Task Atualizar(Jogo JogoExistente)
        {
            var Comandos = $"UPDATE Jogos SET Nome = '{JogoExistente.Nome}', Produtora = '{JogoExistente.Produtora}', Preco = '{JogoExistente.Preco.ToString().Replace(",", ".")}' WHERE Id = '{JogoExistente.Id}'";

            await SqlServerConnection.OpenAsync();
            SqlCommand Command = new SqlCommand(Comandos, SqlServerConnection);

            Command.ExecuteNonQuery();
            await SqlServerConnection.CloseAsync();
        }

        public void Dispose()
        {
            SqlServerConnection?.Close();
            SqlServerConnection?.Dispose();
        }

        public async Task Inserir(Jogo NovoJogo)
        {
            var Comandos = $"INSERT INTO Jogos(Id, Nome, Produtora, Preco) VALUES('{NovoJogo.Id}', '{NovoJogo.Nome}', '{NovoJogo.Produtora}', '{NovoJogo.Preco.ToString().Replace(",", ".")}')";

            await SqlServerConnection.OpenAsync();

            SqlCommand Command = new SqlCommand(Comandos, SqlServerConnection);

            Command.ExecuteNonQuery();
            await SqlServerConnection.CloseAsync();
        }

        public async Task<List<Jogo>> Obter(int Pagina, int Quantidade)
        {
            var ListaDeJogos = new List<Jogo>();
            var Comandos = $"SELECT * FROM Jogos ORDER BY Id OFFSET {((Pagina - 1) * Quantidade)} ROWS FETCH NEXT {Quantidade} ROWS ONLY";

            await SqlServerConnection.OpenAsync();

            SqlCommand Command = new SqlCommand(Comandos, SqlServerConnection);
            SqlDataReader Reader = await Command.ExecuteReaderAsync();

            while(Reader.Read())
            {
                ListaDeJogos.Add(new Jogo
                {
                    Id = (Guid)Reader["Id"],
                    Nome = (string)Reader["Nome"],
                    Produtora = (string)Reader["Produtora"],
                    Preco = Math.Round(Convert.ToDouble(Reader["Preco"]), 2, MidpointRounding.AwayFromZero)
                });
            }

            await SqlServerConnection.CloseAsync();

            return ListaDeJogos;
        }

        public async Task<Jogo> Obter(Guid Id)
        {
            Jogo JogoObtido = new Jogo
            {
                Id = Guid.Empty,
                Nome = string.Empty,
                Produtora = string.Empty,
                Preco = double.NaN
            };

            var Comandos = $"SELECT * FROM Jogos WHERE Id = '{Id}'";

            await SqlServerConnection.OpenAsync();

            SqlCommand Command = new SqlCommand(Comandos, SqlServerConnection);
            SqlDataReader Reader = await Command.ExecuteReaderAsync();

            while(Reader.Read())
            {
                JogoObtido = new Jogo
                {
                    Id = (Guid)Reader["Id"],
                    Nome = (string)Reader["Nome"],
                    Produtora = (string)Reader["Produtora"],
                    Preco = Math.Round(Convert.ToDouble(Reader["Preco"]), 2, MidpointRounding.AwayFromZero)
                };
            }

            await SqlServerConnection.CloseAsync();

            return JogoObtido;
        }

        public async Task<List<Jogo>> Obter(string Nome, string Produtora)
        {
            var ListaDeJogos = new List<Jogo>();
            var Comandos = $"SELECT * FROM Jogos WHERE Nome = '{Nome}' AND Produtora = '{Produtora}'";

            await SqlServerConnection.OpenAsync();

            SqlCommand Command = new SqlCommand(Comandos, SqlServerConnection);
            SqlDataReader Reader = await Command.ExecuteReaderAsync();

            while (Reader.Read())
            {
                ListaDeJogos.Add(new Jogo
                {
                    Id = (Guid)Reader["Id"],
                    Nome = (string)Reader["Nome"],
                    Produtora = (string)Reader["Produtora"],
                    Preco = Math.Round(Convert.ToDouble(Reader["Preco"]), 2, MidpointRounding.AwayFromZero)
                });
            }

            await SqlServerConnection.CloseAsync();

            return ListaDeJogos;
        }

        public async Task Remover(Guid Id)
        {
            var Comandos = $"DELETE FROM Jogos WHERE Id = '{Id}'";

            await SqlServerConnection.OpenAsync();

            SqlCommand Command = new SqlCommand(Comandos, SqlServerConnection);

            Command.ExecuteNonQuery();
            await SqlServerConnection.CloseAsync();
        }
    }
}
