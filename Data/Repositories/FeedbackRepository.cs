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
    public class FeedbackRepository : IFeedbackRepository
    {
        #region conexão
        private readonly DbSession _dbSession;

        public FeedbackRepository(DbSession dbSession)
        {
            _dbSession = dbSession;
        }
        #endregion

        #region metodos solicitacão Feedback
        public async Task<int> InserirSolicitacaoAsync(SolicitacaoFeedbackModel solicitacao)
        {
            string sql = @"INSERT INTO SOLICITACAO_FEEDBACK 
               (ID_USUARIO_SOLICITACAO, ID_RESPONSAVEL_FEEDBACK, STATUS, MENSAGEM_SOLICITACAO) 
               VALUES (@IdUsuarioSolicitacao, @IdResponsavelFeedback, 0, @MensagemSolicitacao);";
            return await _dbSession.Connection.ExecuteAsync(sql, solicitacao, _dbSession.Transaction);
        }

        public async Task<IEnumerable<SolicitacaoFeedbackModel>> ListarTodasSolicitacoesAsync()
        {
            string sql = @"
                        SELECT 
                            SF.ID_SOLICITACAO_FEEDBACK AS IdSolicitacaoFeedback,
                            SF.ID_USUARIO_SOLICITACAO AS IdUsuarioSolicitacao,
                            U1.NOME AS NomeUsuarioSolicitacao,
                            SF.ID_RESPONSAVEL_FEEDBACK AS IdResponsavelFeedback,
                            U2.NOME AS NomeResponsavelFeedback,
                            SF.STATUS AS Status,
                            SF.DATA_SOLICITACAO AS DataSolicitacao,
                            SF.MENSAGEM_SOLICITACAO AS MensagemSolicitacao
                        FROM SOLICITACAO_FEEDBACK SF
                        INNER JOIN USUARIO U1 ON U1.ID_USUARIO = SF.ID_USUARIO_SOLICITACAO
                        INNER JOIN USUARIO U2 ON U2.ID_USUARIO = SF.ID_RESPONSAVEL_FEEDBACK;
                    ";
            return await _dbSession.Connection.QueryAsync<SolicitacaoFeedbackModel>(sql);
        }

        public async Task<SolicitacaoFeedbackModel> ObterSolicitacaoPorIdAsync(int id, IDbTransaction? transaction = null)
        {
            string sql = @"
                        SELECT ID_SOLICITACAO_FEEDBACK, ID_USUARIO_SOLICITACAO, ID_RESPONSAVEL_FEEDBACK, STATUS, DATA_SOLICITACAO, MENSAGEM_SOLICITACAO
                        FROM SOLICITACAO_FEEDBACK
                        WHERE ID_SOLICITACAO_FEEDBACK = @IdSolicitacaoFeedback;";
            return await _dbSession.Connection.QueryFirstOrDefaultAsync<SolicitacaoFeedbackModel>(
                sql,
                new { IdSolicitacaoFeedback = id },
                transaction
            );
        }

        public async Task<bool> ExcluirSolicitacaoAsync(int id, IDbTransaction? transaction = null)
        {
            string sql = @"DELETE FROM SOLICITACAO_FEEDBACK WHERE ID_SOLICITACAO_FEEDBACK = @IdSolicitacaoFeedback;";
            int linhasAfetadas = await _dbSession.Connection.ExecuteAsync(sql, new { IdSolicitacaoFeedback = id }, transaction);
            return linhasAfetadas > 0;
        }

        #endregion

        #region metodos Feedback
        public async Task<int> InserirFeedbackAsync(FeedbackModel feedback)
        {
            string sqlFeedback = @"INSERT INTO FEEDBACK 
            (ID_SOLICITACAO_FEEDBACK, MENSAGEM_FEEDBACK, AVALIACAO) 
            VALUES (@IdSolicitacaoFeedback, @MensagemFeedback, @Avaliacao);";

            string sqlUpdateSolicitacao = @"UPDATE SOLICITACAO_FEEDBACK 
            SET STATUS = 1 
            WHERE ID_SOLICITACAO_FEEDBACK = @IdSolicitacaoFeedback;";
            await _dbSession.Connection.ExecuteAsync(sqlFeedback, feedback, _dbSession.Transaction);
            return await _dbSession.Connection.ExecuteAsync(sqlUpdateSolicitacao, feedback, _dbSession.Transaction);
        }

        public async Task<IEnumerable<FeedbackModel>> ListarTodosFeedbacksAsync()
        {
            string sql = @"SELECT ID_FEEDBACK,ID_SOLICITACAO_FEEDBACK,DATA_REALIZACAO,MENSAGEM_FEEDBACK,AVALIACAO FROM FEEDBACK;";
            return await _dbSession.Connection.QueryAsync<FeedbackModel>(sql);
        }
        public async Task<FeedbackModel> ObterFeedbackPorIdAsync(int id)
        {
            string sql = @"SELECT ID_FEEDBACK, ID_SOLICITACAO_FEEDBACK, DATA_REALIZACAO, MENSAGEM_FEEDBACK, AVALIACAO
                   FROM FEEDBACK
                   WHERE ID_FEEDBACK = @IdFeedback;";
            return await _dbSession.Connection.QueryFirstOrDefaultAsync<FeedbackModel>(sql, new { IdFeedback = id });
        }
        #endregion
    }
}
