using Dapper;
using Data.Connections;
using Data.Interfaces;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class BancoHorasRepository : IBancoHorasRepository
    {
        #region conexão
        private readonly DbSession _dbSession;
        private readonly IJornadaTrabalhoRepository _jornadaTrabalhoRepository;

        public BancoHorasRepository(DbSession dbSession, IJornadaTrabalhoRepository jornadaTrabalhoRepository)
        {
            _dbSession = dbSession;
            _jornadaTrabalhoRepository = jornadaTrabalhoRepository;
        }
        #endregion

        #region metodos
        public async Task<(TimeSpan? saldo, bool apontamentoInconsistente)> CalcularSaldoDiarioAsync(int idUsuario, DateTime dataReferencia)
        {
            string sql = @"SELECT HORA_REGISTRO 
                   FROM REGISTRO_PONTO 
                   WHERE ID_USUARIO = @IdUsuario 
                   AND DATE(HORA_REGISTRO) = @DataReferencia 
                   ORDER BY HORA_REGISTRO";

            var registros = (await _dbSession.Connection.QueryAsync<DateTime>(
                sql, new { IdUsuario = idUsuario, DataReferencia = dataReferencia.Date })
            ).ToList();

            // Se número ímpar de registros, marca como inconsistente
            if (registros.Count % 2 != 0)
            {
                return (null, true);
            }
            // registros de ponto em pares (entrada e saida)
            TimeSpan totalTrabalhado = TimeSpan.Zero;
            for (int i = 0; i < registros.Count - 1; i += 2)
            {
                totalTrabalhado += (registros[i + 1] - registros[i]);
            }

            TimeSpan jornadaEsperada = await _jornadaTrabalhoRepository.ObterJornadaDiariaUsuario(idUsuario);
            TimeSpan saldo = totalTrabalhado - jornadaEsperada;

            return (saldo, false);
        }

        public async Task InserirSaldoDiarioAsync(int idUsuario, DateTime data, TimeSpan? saldo, bool apontamentoInconsistente)
        {
            string sql = @"INSERT INTO SALDO_DIARIO_BANCO_HORAS (ID_USUARIO, SALDO_DIARIO, DATA_REFERENCIA, APONTAMENTO_INCONSISTENTE)
                   VALUES (@IdUsuario, @Saldo, @DataReferencia, @ApontamentoInconsistente)";

            await _dbSession.Connection.ExecuteAsync(sql, new
            {
                IdUsuario = idUsuario,
                Saldo = saldo.HasValue ? (int)saldo.Value.TotalMinutes : (int?)null,
                DataReferencia = data,
                ApontamentoInconsistente = apontamentoInconsistente
            });
        }
        public async Task<IEnumerable<SaldoDiarioBancoHorasModel>> ObterSaldosUsuarioAsync(int idUsuario)
        {
            string sql = @"
                            SELECT DATA_REFERENCIA, SALDO_DIARIO, APONTAMENTO_INCONSISTENTE
                            FROM SALDO_DIARIO_BANCO_HORAS
                            WHERE ID_USUARIO = @IdUsuario";

            var resultado = await _dbSession.Connection.QueryAsync<SaldoDiarioBancoHorasModel>(sql, new
            {
                IdUsuario = idUsuario
            });

            return resultado;
        }

        public async Task<(TimeSpan horasTrabalhadas, TimeSpan saldoTotal)> CalcularBancoHorasAsync(int idUsuario)
        {
            string sql = @"
                            SELECT SALDO_DIARIO
                            FROM SALDO_DIARIO_BANCO_HORAS
                            WHERE ID_USUARIO = @IdUsuario
                              AND APONTAMENTO_INCONSISTENTE = FALSE";

            var saldos = await _dbSession.Connection.QueryAsync<int>(sql, new { IdUsuario = idUsuario });

            TimeSpan horasTrabalhadas = TimeSpan.Zero;
            TimeSpan saldoTotal = TimeSpan.Zero;

            foreach (var saldoMinutos in saldos)
            {
                TimeSpan saldo = TimeSpan.FromMinutes(saldoMinutos);
                saldoTotal += saldo;

                // Se trabalhou mais que 0 (mesmo que o saldo seja negativo, ainda houve horas)
                if (saldo >= TimeSpan.Zero)
                    horasTrabalhadas += saldo + await _jornadaTrabalhoRepository.ObterJornadaDiariaUsuario(idUsuario);
                else
                    horasTrabalhadas += await _jornadaTrabalhoRepository.ObterJornadaDiariaUsuario(idUsuario);
            }

            return (horasTrabalhadas, saldoTotal);
        }

        public async Task InserirBancoHorasAsync(int idUsuario, TimeSpan horasTrabalhadas, TimeSpan saldo)
        {
            string sql = @"
                            INSERT INTO BANCO_HORAS (ID_USUARIO, HORAS_TRABALHADAS, SALDO)
                            VALUES (@IdUsuario, @HorasTrabalhadas, @Saldo)
                            ON DUPLICATE KEY UPDATE
                                HORAS_TRABALHADAS = @HorasTrabalhadas,
                                SALDO = @Saldo";

            await _dbSession.Connection.ExecuteAsync(sql, new
            {
                IdUsuario = idUsuario,
                HorasTrabalhadas = horasTrabalhadas,
                Saldo = saldo
            });
        }

        public async Task<(TimeSpan horasTrabalhadas, TimeSpan saldo)> ObterBancoHorasAtualAsync(int idUsuario)
        {
            string sql = @"
                            SELECT HORAS_TRABALHADAS, SALDO
                            FROM BANCO_HORAS
                            WHERE ID_USUARIO = @IdUsuario";

            var resultado = await _dbSession.Connection.QueryFirstOrDefaultAsync<(TimeSpan, TimeSpan)>(sql, new
            {
                IdUsuario = idUsuario
            });

            return resultado;
        }

        #endregion
    }
}
