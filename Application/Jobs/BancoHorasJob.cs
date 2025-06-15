using Application.Interfaces;
using Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Jobs
{
    public class BancoHorasJob
    {
        private readonly IBancoHorasService _bancoHorasService;
        private readonly IUsuarioRepository _usuarioRepository;

        public BancoHorasJob(IBancoHorasService bancoHorasService, IUsuarioRepository usuarioRepository)
        {
            _bancoHorasService = bancoHorasService;
            _usuarioRepository = usuarioRepository;
        }

        public async Task ExecutarProcessamentoDiario()
        {
            var usuarios = await _usuarioRepository.ListarTodosAsync();

            foreach (var usuario in usuarios)
            {
                await _bancoHorasService.ProcessarBancoHorasDiarioAsync(usuario.IdUsuario, DateTime.Today);
            }
        }
    }
}
