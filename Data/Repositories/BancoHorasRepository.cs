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
        public async Task<List<DateTime>> ObterDiasInconsistentesAsync(int idUsuario)
        {
            string sql = @"SELECT DATA_REFERENCIA FROM SALDO_DIARIO_BANCO_HORAS
                   WHERE ID_USUARIO = @IdUsuario AND APONTAMENTO_INCONSISTENTE = 1";

            var dias = await _dbSession.Connection.QueryAsync<DateTime>(
                sql,
                new { IdUsuario = idUsuario },
                transaction: _dbSession.Transaction
            );

            return dias.ToList();
        }

        public async Task AtualizarSaldoDiarioInconsistenteAsync(int idUsuario, DateTime data, TimeSpan saldo, bool inconsistente)
        {
            string sql = @"
                UPDATE SALDO_DIARIO_BANCO_HORAS
                SET 
                    SALDO_DIARIO = @Saldo,
                    APONTAMENTO_INCONSISTENTE = @Inconsistente
                WHERE 
                    ID_USUARIO = @IdUsuario AND DATE(DATA_REFERENCIA) = @Data";

            await _dbSession.Connection.ExecuteAsync(sql, new
            {
                IdUsuario = idUsuario,
                Data = data.Date,
                Saldo = saldo,
                Inconsistente = inconsistente
            }, transaction: _dbSession.Transaction);
        }

        public async Task<(TimeSpan? saldo, bool apontamentoInconsistente)> CalcularSaldoDiarioAsync(int idUsuario, DateTime dataReferencia)
        {
            string sql = @"SELECT HORA_REGISTRO 
                   FROM REGISTRO_PONTO 
                   WHERE ID_USUARIO = @IdUsuario 
                   AND DATE(HORA_REGISTRO) = @DataReferencia 
                   ORDER BY HORA_REGISTRO";

            var registros = (await _dbSession.Connection.QueryAsync<DateTime>(
                sql,
                new { IdUsuario = idUsuario, DataReferencia = dataReferencia.Date },
                transaction: _dbSession.Transaction
            )).ToList();

            if (registros.Count % 2 != 0)
            {
                return (TimeSpan.Zero, true);
            }

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
                Saldo = saldo,
                DataReferencia = data,
                ApontamentoInconsistente = apontamentoInconsistente
            }, transaction: _dbSession.Transaction);
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
            // Pega a jornada esperada diária do usuário
            TimeSpan jornadaEsperada = await _jornadaTrabalhoRepository.ObterJornadaDiariaUsuario(idUsuario);

            // Consulta todos os saldos diários que não estão inconsistentes
            string sql = @"
                SELECT SALDO_DIARIO
                FROM SALDO_DIARIO_BANCO_HORAS
                WHERE ID_USUARIO = @IdUsuario
                    AND APONTAMENTO_INCONSISTENTE = FALSE";

            var saldos = await _dbSession.Connection.QueryAsync<TimeSpan?>(
                sql,
                new { IdUsuario = idUsuario },
                transaction: _dbSession.Transaction
            );

            TimeSpan horasTrabalhadas = TimeSpan.Zero;
            TimeSpan saldoTotal = TimeSpan.Zero;

            foreach (var saldo in saldos)
            {
                if (!saldo.HasValue) continue;

                saldoTotal += saldo.Value;

                // Horas trabalhadas naquele dia = jornada esperada + saldo daquele dia
                var horasNoDia = jornadaEsperada + saldo.Value;

                // Somar todas as horas trabalhadas
                horasTrabalhadas += horasNoDia;
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
            }, transaction: _dbSession.Transaction);
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

        public async Task<IEnumerable<BancoHorasModel>> ObterSaldosDiariosDoMesAsync(int idUsuario)
        {
            string sql = @"
                            SELECT 
                                ID_USUARIO AS IdUsuario,
                                SALDO_DIARIO AS Saldo
                            FROM 
                                SALDO_DIARIO_BANCO_HORAS
                            WHERE 
                                ID_USUARIO = @IdUsuario
                                AND APONTAMENTO_INCONSISTENTE = 0
                                AND MONTH(DATA_REFERENCIA) = MONTH(NOW())
                                AND YEAR(DATA_REFERENCIA) = YEAR(NOW())";

            var resultado = await _dbSession.Connection.QueryAsync<BancoHorasModel>(sql, new
            {
                IdUsuario = idUsuario
            });

            return resultado;
        }

        public async Task<IEnumerable<BancoHorasModel>> ObterHorasExtrasPorMesAsync(int idUsuario)
        {
            string sql = @"
                            SELECT 
                            NULL AS IdBancoHoras,
                            ID_USUARIO AS IdUsuario,
                            NULL AS HorasTrabalhadas,
                            SEC_TO_TIME(SUM(TIME_TO_SEC(SALDO_DIARIO))) AS Saldo
                        FROM 
                            SALDO_DIARIO_BANCO_HORAS
                        WHERE 
                            ID_USUARIO = @IdUsuario
                            AND APONTAMENTO_INCONSISTENTE = 0
                            AND TIME_TO_SEC(SALDO_DIARIO) > 0
                            AND MONTH(DATA_REFERENCIA) = MONTH(NOW())
                            AND YEAR(DATA_REFERENCIA) = YEAR(NOW())
                        GROUP BY 
                            ID_USUARIO;";

            var resultado = await _dbSession.Connection.QueryAsync<BancoHorasModel>(sql, new
            {
                IdUsuario = idUsuario
            });

            return resultado;
        }

        #endregion
    }
}
