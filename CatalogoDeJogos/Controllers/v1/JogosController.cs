using CatalogoDeJogos.Exceptions;
using CatalogoDeJogos.InputModel;
using CatalogoDeJogos.Services;
using CatalogoDeJogos.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CatalogoDeJogos.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class JogosController : ControllerBase
    {
        private readonly IJogoService _jogoService;

        public JogosController(IJogoService jogoService)
        {
            _jogoService = jogoService;
        }

        /// <summary>
        /// Buscar todos os jogos cadastrados de forma paginada.
        /// </summary>
        /// <remarks>
        /// Não é possível retornar os jogos sem paginação.
        /// </remarks>
        /// <param name="Pagina">Indica qual página que será consultada. Mínimo 1</param>
        /// <param name="Quantidade">Indica qual a quantidade de registros por página. Valores de (1~50)</param>
        /// <response code="200">Retorna a lista de jogos.</response>
        /// <response code="204">Se não houver jogos cadastrados.</response>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<JogoViewModel>>> Obter([FromQuery, Range(1, int.MaxValue)] int Pagina = 1, [FromQuery, Range(1, 50)] int Quantidade = 5)
        {
            var lista = await _jogoService.Obter(Pagina, Quantidade);

            if (lista.Count() == 0)
                return NoContent();

            return Ok(lista);
        }

        /// <summary>
        /// Buscar um jogo pelo seu Id.
        /// </summary>
        /// <param name="Id">Id do jogo que deseja buscar.</param>
        /// <response code="200">Retorna o jogo filtrado.</response>
        /// <response code="204">Caso não haja jogo com este id.</response>
        [HttpGet("{Id:guid}")]
        public async Task<ActionResult<JogoViewModel>> Obter([FromRoute] Guid Id)
        {
            var jogo = await _jogoService.Obter(Id);

            if (jogo == null)
                return NoContent();

            return Ok(jogo);
        }

        /// <summary>
        /// Inserir um jogo no catálogo.
        /// </summary>
        /// <param name="Jogo">Dados do jogo a ser inserido.</param>
        /// <response code="200">Caso o jogo seja inserido com sucesso.</response>
        /// <response code="422">Caso já exista um jogo com mesmo nome para a mesma produtora.</response>
        [HttpPost]
        public async Task<ActionResult<JogoViewModel>> InserirJogo([FromBody] JogoInputModel Jogo)
        {
            try
            {
                var result = await _jogoService.Inserir(Jogo);

                return Ok(result);
            }
            catch (JogoJaCadastradoException ex)
            {
                return UnprocessableEntity(ex.Message);
            }
        }

        /// <summary>
        /// Atualizar um jogo no catálogo.
        /// </summary>
        /// <param name="Id">Id do jogo a ser atualizado.</param>
        /// <param name="Jogo">Novos dados para atualizar o jogo indicado.</param>
        /// <response code="200">Caso o jogo seja atualizado com sucesso.</response>
        /// <response code="404">Caso não exista um jogo com este Id.</response>
        /// <response code="422">Caso já exista um jogo com mesmo nome para a mesma produtora.</response>
        [HttpPut("{Id:guid}")]
        public async Task<ActionResult> AtualizarJogo([FromRoute] Guid Id, [FromBody] JogoInputModel Jogo)
        {
            try
            {
                await _jogoService.Atualizar(Id, Jogo);

                return Ok();
            }
            catch (JogoNaoCadastradoException ex)
            {
                return NotFound(ex.Message);
            }
            catch (JogoJaCadastradoException ex)
            {
                return UnprocessableEntity(ex.Message);
            }
        }

        /// <summary>
        /// Atualizar o preço de um jogo.
        /// </summary>
        /// <param name="Id">Id do jogo a ser atualizado.</param>
        /// <param name="Preco">Novo preço do jogo. Mínimo 1</param>
        /// <response code="200">Caso o preço seja atualizado com sucesso.</response>
        /// <response code="404">Caso não exista um jogo com este Id.</response>
        [HttpPatch("{Id:guid}/preco/{Preco:double}")]
        public async Task<ActionResult> AtualizarJogo([FromRoute] Guid Id, [FromRoute, Range(1, double.MaxValue)] double Preco)
        {
            try
            {
                await _jogoService.Atualizar(Id, Preco);

                return Ok();
            }
            catch (JogoNaoCadastradoException ex)
            {
                return NotFound(ex.Message);
            }
        }

        /// <summary>
        /// Atualizar o 'Nome' de um jogo ou 'Produtora'.
        /// </summary>
        /// <param name="Id">Id do jogo a ser atualizado.</param>
        /// <param name="Nome">Novo 'Nome' do jogo ou 'Produtora'.</param>
        /// <response code="200">Caso o 'Nome' seja atualizado com sucesso.</response>
        /// <response code="404">Caso não exista um jogo com este Id.</response>
        /// <response code="422">Caso já exista um jogo com mesmo nome para a mesma produtora.</response>
        [HttpPatch("{Id:guid}/nome/{Nome}")]
        [HttpPatch("{Id:guid}/produtora/{Nome}")]
        public async Task<ActionResult> AtualizarJogo([FromRoute] Guid Id, [FromRoute, StringLength(100, MinimumLength = 3, ErrorMessage = "O nome do jogo deve conter entre 3 e 100 caracteres.")] string Nome)
        {
            try
            {
                await _jogoService.Atualizar(Id, Nome, Request.Path.Value.Contains("produtora"));

                return Ok();
            }
            catch (JogoNaoCadastradoException ex)
            {
                return NotFound(ex.Message);
            }
            catch (JogoJaCadastradoException ex)
            {
                return UnprocessableEntity(ex.Message);
            }
        }

        /// <summary>
        /// Excluir um jogo.
        /// </summary>
        /// <param name="Id">Id do jogo a ser excluído.</param>
        /// <response code="200">Caso o jogo seja excluído com sucesso.</response>
        /// <response code="404">Caso não exista um jogo com este Id.</response>
        [HttpDelete("{Id:guid}")]
        public async Task<ActionResult> ApagarJogo([FromRoute] Guid Id)
        {
            try
            {
                await _jogoService.Remover(Id);

                return Ok();
            }
            catch (JogoNaoCadastradoException ex)
            {

                return NotFound(ex.Message);
            }
        }
    }
}
