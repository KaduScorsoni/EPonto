using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces
{
    public interface IBancoHorasRepository
    {
        Task<(TimeSpan? saldo, bool apontamentoInconsistente)> CalcularSaldoDiarioAsync(int idUsuario, DateTime dataReferencia);
        Task InserirSaldoDiarioAsync(int idUsuario, DateTime data, TimeSpan? saldo, bool apontamentoInconsistente);
        Task<(TimeSpan horasTrabalhadas, TimeSpan saldoTotal)> CalcularBancoHorasAsync(int idUsuario);
        Task InserirBancoHorasAsync(int idUsuario, TimeSpan horasTrabalhadas, TimeSpan saldo);
        Task<IEnumerable<SaldoDiarioBancoHorasModel>> ObterSaldosUsuarioAsync(int idUsuario);
        Task<(TimeSpan horasTrabalhadas, TimeSpan saldo)> ObterBancoHorasAtualAsync(int idUsuario);
    }
}
