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

        public async Task<IEnumerable<SolicitacaoFeedbackModel>> ObterSolicitacoesPorUsuarioAsync(int idUsuario,IDbTransaction? transaction = null)
        {
            string sql = @"
                            SELECT ID_SOLICITACAO_FEEDBACK, 
                                   ID_USUARIO_SOLICITACAO, 
                                   ID_RESPONSAVEL_FEEDBACK, 
                                   STATUS, 
                                   DATA_SOLICITACAO, 
                                   MENSAGEM_SOLICITACAO
                            FROM SOLICITACAO_FEEDBACK
                            WHERE ID_USUARIO_SOLICITACAO = @IdUsuarioSolicitacao;";

            return await _dbSession.Connection.QueryAsync<SolicitacaoFeedbackModel>(
                sql,
                new { IdUsuarioSolicitacao = idUsuario },
                transaction
            );
        }
        public async Task<IEnumerable<SolicitacaoFeedbackModel>> ObterSolicitacoesResponsavelAsync(int idResponsavel,IDbTransaction? transaction = null)
        {
            string sql = @"
                            SELECT ID_SOLICITACAO_FEEDBACK, 
                                   ID_USUARIO_SOLICITACAO, 
                                   ID_RESPONSAVEL_FEEDBACK, 
                                   STATUS, 
                                   DATA_SOLICITACAO, 
                                   MENSAGEM_SOLICITACAO
                            FROM SOLICITACAO_FEEDBACK
                            WHERE ID_RESPONSAVEL_FEEDBACK = @IdResponsavel
                            ORDER BY DATA_SOLICITACAO DESC;";

            return await _dbSession.Connection.QueryAsync<SolicitacaoFeedbackModel>(
                sql,
                new { IdResponsavel = idResponsavel },
                transaction
            );
        }

        public async Task<bool> AtualizarSolicitacaoAsync(SolicitacaoFeedbackModel solicitacao, IDbTransaction? transaction = null)
        {
            string sql = @"UPDATE SOLICITACAO_FEEDBACK 
                   SET ID_RESPONSAVEL_FEEDBACK = @IdResponsavelFeedback,
                       MENSAGEM_SOLICITACAO = @MensagemSolicitacao
                   WHERE ID_SOLICITACAO_FEEDBACK = @IdSolicitacaoFeedback;";
            int linhasAfetadas = await _dbSession.Connection.ExecuteAsync(sql, solicitacao, _dbSession.Transaction);
            return linhasAfetadas > 0;
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
            string sqlFeedbackComSolicitacao = @"
                                                INSERT INTO FEEDBACK 
                                                (ID_SOLICITACAO_FEEDBACK, ID_USUARIO_FEEDBACK,ID_AUTOR_FEEDBACK, MENSAGEM_FEEDBACK, AVALIACAO) 
                                                VALUES (@IdSolicitacaoFeedback, @IdUsuarioFeedback,@IdAutorFeedback, @MensagemFeedback, @Avaliacao);";

            string sqlFeedbackSemSolicitacao = @"
                                                INSERT INTO FEEDBACK 
                                                (ID_USUARIO_FEEDBACK,ID_AUTOR_FEEDBACK, MENSAGEM_FEEDBACK, AVALIACAO) 
                                                VALUES (@IdUsuarioFeedback,@IdAutorFeedback, @MensagemFeedback, @Avaliacao);";

            if (feedback.IdSolicitacaoFeedback > 0)
            {
                // Cria feedback vinculado à solicitação
                await _dbSession.Connection.ExecuteAsync(sqlFeedbackComSolicitacao, feedback, _dbSession.Transaction);

                // Atualiza solicitação para "respondida"
                string sqlUpdateSolicitacao = @"
                                                UPDATE SOLICITACAO_FEEDBACK 
                                                SET STATUS = 1 
                                                WHERE ID_SOLICITACAO_FEEDBACK = @IdSolicitacaoFeedback;";

                return await _dbSession.Connection.ExecuteAsync(sqlUpdateSolicitacao, feedback, _dbSession.Transaction);
            }
            else
            {
                // Cria feedback independente
                return await _dbSession.Connection.ExecuteAsync(sqlFeedbackSemSolicitacao, feedback, _dbSession.Transaction);
            }
        }

        public async Task<IEnumerable<FeedbackModel>> ListarTodosFeedbacksAsync()
        {
            string sql = @"
                            SELECT 
                                f.ID_FEEDBACK,
                                f.ID_SOLICITACAO_FEEDBACK,
                                f.DATA_REALIZACAO,
                                f.MENSAGEM_FEEDBACK,
                                f.AVALIACAO,
                                u1.NOME AS NOME_USUARIO_FEEDBACK,
                                u2.NOME AS NOME_AUTOR_FEEDBACK
                            FROM FEEDBACK f
                            LEFT JOIN USUARIO u1 ON f.ID_USUARIO_FEEDBACK = u1.ID_USUARIO
                            LEFT JOIN USUARIO u2 ON f.ID_AUTOR_FEEDBACK = u2.ID_USUARIO;
                        ";
            return await _dbSession.Connection.QueryAsync<FeedbackModel>(sql);
        }

        public async Task<FeedbackModel> ObterFeedbackPorIdAsync(int id)
        {
            string sql = @"
                            SELECT 
                                f.ID_FEEDBACK,
                                f.ID_SOLICITACAO_FEEDBACK,
                                f.DATA_REALIZACAO,
                                f.MENSAGEM_FEEDBACK,
                                f.AVALIACAO,
                                u1.NOME AS NOME_USUARIO_FEEDBACK,
                                u2.NOME AS NOME_AUTOR_FEEDBACK
                            FROM FEEDBACK f
                            LEFT JOIN USUARIO u1 ON f.ID_USUARIO_FEEDBACK = u1.ID_USUARIO
                            LEFT JOIN USUARIO u2 ON f.ID_AUTOR_FEEDBACK = u2.ID_USUARIO
                            WHERE f.ID_FEEDBACK = @IdFeedback;
                        ";
            return await _dbSession.Connection.QueryFirstOrDefaultAsync<FeedbackModel>(sql, new { IdFeedback = id });
        }
        public async Task<IEnumerable<FeedbackModel>> ObterFeedbacksPorUsuarioAsync(int idUsuario)
        {
            string sql = @"
                            SELECT 
                                f.ID_FEEDBACK,
                                f.ID_SOLICITACAO_FEEDBACK,
                                f.DATA_REALIZACAO,
                                f.MENSAGEM_FEEDBACK,
                                f.AVALIACAO,
                                u1.NOME AS NOME_USUARIO_FEEDBACK,
                                u2.NOME AS NOME_AUTOR_FEEDBACK
                            FROM FEEDBACK f
                            LEFT JOIN USUARIO u1 ON f.ID_USUARIO_FEEDBACK = u1.ID_USUARIO
                            LEFT JOIN USUARIO u2 ON f.ID_AUTOR_FEEDBACK = u2.ID_USUARIO
                            WHERE f.ID_AUTOR_FEEDBACK = @IdUsuario;
                        ";
            return await _dbSession.Connection.QueryAsync<FeedbackModel>(sql, new { IdUsuario = idUsuario });
        }

        #endregion
    }
}
