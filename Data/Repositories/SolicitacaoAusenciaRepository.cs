using Dapper;
using Data.Connections;
using Data.Interfaces;
using Domain.Entities;
using Domain.Entities.Feedback;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class SolicitacaoAusenciaRepository : ISolicitacaoAusenciaRepository
    {
        #region conexão
        private readonly DbSession _dbSession;

        public SolicitacaoAusenciaRepository(DbSession dbSession)
        {
            _dbSession = dbSession;
        }
        #endregion

        #region metodos
        public async Task<IEnumerable<SolicitacaoAusenciaModel>> ListarTodosAsync()
        {
            string sql = @"SELECT ID_SOLICITACAO_AUSENCIA, ID_USUARIO, LINK_ARQUIVO, MENSAGEM_SOLICITACAO,DATA_INICIO_AUSENCIA,DATA_FIM_AUSENCIA,DATA_SOLICITACAO,STATUS_SOLICITACAO
                   FROM SOLICITACAO_AUSENCIA;";
            return await _dbSession.Connection.QueryAsync<SolicitacaoAusenciaModel>(sql);
        }

        public async Task<SolicitacaoAusenciaModel> ObterPorIdAsync(int id, IDbTransaction? transaction = null)
        {
            string sql = @"SELECT ID_SOLICITACAO_AUSENCIA, ID_USUARIO, LINK_ARQUIVO, 
                          MENSAGEM_SOLICITACAO, DATA_INICIO_AUSENCIA, DATA_FIM_AUSENCIA, 
                          DATA_SOLICITACAO, STATUS_SOLICITACAO
                   FROM SOLICITACAO_AUSENCIA
                   WHERE ID_SOLICITACAO_AUSENCIA = @IdSolicitacaoAusencia;";

            return await _dbSession.Connection.QueryFirstOrDefaultAsync<SolicitacaoAusenciaModel>(
                sql,
                new { IdSolicitacaoAusencia = id },
                transaction: transaction 
            );
        }


        public async Task<IEnumerable<SolicitacaoAusenciaModel>> ObterSolicitacoesPorUsuarioAsync(int idUsuario, IDbTransaction? transaction = null)
        {
            string sql = @"
                             SELECT ID_SOLICITACAO_AUSENCIA, 
                                ID_USUARIO, 
                                LINK_ARQUIVO, 
                                MENSAGEM_SOLICITACAO, 
                                DATA_INICIO_AUSENCIA, 
                                DATA_FIM_AUSENCIA,
		                        DATA_SOLICITACAO,
		                        STATUS_SOLICITACAO
                             FROM SOLICITACAO_AUSENCIA
                             WHERE ID_USUARIO = @IdUsuario;";

            return await _dbSession.Connection.QueryAsync<SolicitacaoAusenciaModel>(
                sql,
                new { IdUsuario = idUsuario },
                transaction
            );
        }

        public async Task<int> InserirAsync(SolicitacaoAusenciaModel solicitacao)
        {
            string sql = @"
                        INSERT INTO SOLICITACAO_AUSENCIA
                        (ID_USUARIO,
                         LINK_ARQUIVO,
                         MENSAGEM_SOLICITACAO,
                         DATA_INICIO_AUSENCIA,
                         DATA_FIM_AUSENCIA,
                         STATUS_SOLICITACAO)
                        VALUES
                        (@IdUsuario,
                         @LinkArquivo,
                         @MensagemSolicitacao,
                         @DataInicioAusencia,
                         @DataFimAusencia,
                         0);";
            return await _dbSession.Connection.ExecuteAsync(sql, solicitacao, _dbSession.Transaction);
        }

        public async Task<bool> AtualizarSolicitacaoAsync(SolicitacaoAusenciaModel solicitacao, IDbTransaction? transaction = null)
        {
            string sql = @"UPDATE SOLICITACAO_AUSENCIA 
                   SET LINK_ARQUIVO = @LinkArquivo,
                       MENSAGEM_SOLICITACAO = @MensagemSolicitacao,
                       DATA_INICIO_AUSENCIA = @DataInicioAusencia,
                       DATA_FIM_AUSENCIA = @DataFimAusencia,
                       DATA_SOLICITACAO = @DataSolicitacao
                   WHERE ID_SOLICITACAO_AUSENCIA = @IdSolicitacaoAusencia;";

            int linhasAfetadas = await _dbSession.Connection.ExecuteAsync(sql, solicitacao, transaction);
            return linhasAfetadas > 0;
        }

        public async Task<bool> ExcluirSolicitacaoAsync(int id, IDbTransaction? transaction = null)
        {
            string sql = @"DELETE FROM SOLICITACAO_AUSENCIA WHERE ID_SOLICITACAO_AUSENCIA = @IdSolicitacaoAusencia;";
            int linhasAfetadas = await _dbSession.Connection.ExecuteAsync(sql, new { IdSolicitacaoAusencia = id }, transaction);
            return linhasAfetadas > 0;
        }
        public async Task<bool> AtualizarStatusAsync(int idSolicitacao, int status, IDbTransaction? transaction = null)
        {
            string sql = @"UPDATE SOLICITACAO_AUSENCIA 
                   SET STATUS_SOLICITACAO = @Status
                   WHERE ID_SOLICITACAO_AUSENCIA = @IdSolicitacao";

            int linhasAfetadas = await _dbSession.Connection.ExecuteAsync(sql, new
            {
                IdSolicitacao = idSolicitacao,
                Status = status
            }, transaction);

            return linhasAfetadas > 0;
        }

        #endregion
    }
}
