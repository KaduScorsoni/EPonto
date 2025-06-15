using Dapper;
using Data.Connections;
using Data.Interfaces;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class JornadaTrabalhoRepository : IJornadaTrabalhoRepository
    {
        #region conexão
        private readonly DbSession _dbSession;

        public JornadaTrabalhoRepository(DbSession dbSession)
        {
            _dbSession = dbSession;
        }
        #endregion

        #region metodos
        public async Task<int> InserirAsync(JornadaTrabalhoModel jornada)
        {
            string sql = @"INSERT INTO JORNADA_TRABALHO 
               (NOME_JORNADA, QTD_HORAS_DIARIAS, IND_ATIVO,TESTE) 
               VALUES (@NomeJornada, @QtdHorasDiarias, 1,@teste);";
            return await _dbSession.Connection.ExecuteAsync(sql, jornada, _dbSession.Transaction);
        }

        public async Task<JornadaTrabalhoModel> ObterPorIdAsync(int id)
        {
            string sql = @"SELECT ID_JORNADA, NOME_JORNADA, QTD_HORAS_DIARIAS, IND_ATIVO
                   FROM JORNADA_TRABALHO
                   WHERE ID_JORNADA = @IdJornada;";
            return await _dbSession.Connection.QueryFirstOrDefaultAsync<JornadaTrabalhoModel>(sql, new { IdJornada = id });
        }
        public async Task<TimeSpan> ObterJornadaDiariaUsuario(int idUsuario)
        {
            string sql = @"
                SELECT j.QTD_HORAS_DIARIAS
                FROM USUARIO u
                INNER JOIN JORNADA_TRABALHO j ON u.ID_JORNADA = j.ID_JORNADA
                WHERE u.ID_USUARIO = @IdUsuario";

            var horasDiarias = await _dbSession.Connection.QueryFirstOrDefaultAsync<decimal?>(
                sql,
                new { IdUsuario = idUsuario },
                transaction: _dbSession.Transaction
            );

            if (horasDiarias == null || horasDiarias == 0)
                throw new Exception("Jornada de trabalho diária não definida para o usuário.");

            return TimeSpan.FromHours((double)horasDiarias);
        }

        public async Task<IEnumerable<JornadaTrabalhoModel>> ListarTodosAsync()
        {
            string sql = @"SELECT ID_JORNADA, NOME_JORNADA, QTD_HORAS_DIARIAS, IND_ATIVO,TESTE
                   FROM JORNADA_TRABALHO;";
            return await _dbSession.Connection.QueryAsync<JornadaTrabalhoModel>(sql);
        }


        public async Task<bool> AtualizarAsync(JornadaTrabalhoModel jornada)
        {
            string sql = @"UPDATE JORNADA_TRABALHO 
                   SET NOME_JORNADA = @NomeJornada,
                       QTD_HORAS_DIARIAS = @QtdHorasDiarias
                   WHERE ID_JORNADA = @IdJornada;";
            int linhasAfetadas = await _dbSession.Connection.ExecuteAsync(sql, jornada, _dbSession.Transaction);
            return linhasAfetadas > 0;
        }

        public async Task<bool> ExcluirAsync(int id)
        {
            string sql = @"UPDATE JORNADA_TRABALHO SET IND_ATIVO = 0 WHERE ID_JORNADA = @IdJornada;";
            int linhasAfetadas = await _dbSession.Connection.ExecuteAsync(sql, new { IdJornada = id }, _dbSession.Transaction);
            return linhasAfetadas > 0;
        }
        public async Task<JornadaTrabalhoModel?> ValidarJornadaExistente(string nomeCargo)
        {
            string sql = "SELECT * FROM JORNADA_TRABALHO WHERE NOME_JORNADA = @NomeJornada";
            return await _dbSession.Connection.QueryFirstOrDefaultAsync<JornadaTrabalhoModel>(
                sql,
                new { NomeJornada = nomeCargo },
                _dbSession.Transaction
            );
        }
        #endregion
    }
}
