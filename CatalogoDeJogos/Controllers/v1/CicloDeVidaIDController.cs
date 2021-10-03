using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogoDeJogos.Controllers.v1
{
    [Route("api/[controller]")]
    [ApiController]
    public class CicloDeVidaIDController : ControllerBase
    {
        public readonly IExemploSingleton _ExemploSingleton1;
        public readonly IExemploSingleton _ExemploSingleton2;

        public readonly IExemploScoped _ExemploScoped1;
        public readonly IExemploScoped _ExemploScoped2;

        public readonly IExemploTransient _ExemploTransient1;
        public readonly IExemploTransient _ExemploTransient2;

        public CicloDeVidaIDController(IExemploSingleton ExemploSingleton1,
                                       IExemploSingleton ExemploSingleton2,
                                       IExemploScoped ExemploScoped1,
                                       IExemploScoped ExemploScoped2,
                                       IExemploTransient ExemploTransient1,
                                       IExemploTransient ExemploTransient2)
        {
            _ExemploSingleton1 = ExemploSingleton1;
            _ExemploSingleton2 = ExemploSingleton2;
            _ExemploScoped1 = ExemploScoped1;
            _ExemploScoped2 = ExemploScoped2;
            _ExemploTransient1 = ExemploTransient1;
            _ExemploTransient2 = ExemploTransient2;
        }

        [HttpGet]
        public Task<string> Get()
        {
            StringBuilder StrBuilder = new StringBuilder();

            StrBuilder.AppendLine($"Singleton 1: {_ExemploSingleton1.Id}");
            StrBuilder.AppendLine($"Singleton 2: {_ExemploSingleton2.Id}");
            StrBuilder.AppendLine();
            StrBuilder.AppendLine($"Scoped 1: {_ExemploScoped1.Id}");
            StrBuilder.AppendLine($"Scoped 2: {_ExemploScoped2.Id}");
            StrBuilder.AppendLine();
            StrBuilder.AppendLine($"Transient 1: {_ExemploTransient1.Id}");
            StrBuilder.AppendLine($"Transient 2: {_ExemploTransient2.Id}");

            return Task.FromResult(StrBuilder.ToString());
        }
    }

    public interface IExemploGeral
    {
        public Guid Id { get; }
    }

    public interface IExemploSingleton : IExemploGeral
    { }

    public interface IExemploScoped : IExemploGeral
    { }

    public interface IExemploTransient : IExemploGeral
    { }

    public class ExemploCicloDeVida : IExemploSingleton, IExemploScoped, IExemploTransient
    {
        private readonly Guid _Guid;

        public ExemploCicloDeVida()
        {
            _Guid = Guid.NewGuid();
        }

        public Guid Id => _Guid;
    }
}
