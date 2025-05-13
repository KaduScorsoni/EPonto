using Dapper;
using Data.Connections;
using Data.Interfaces;
using Domain.Entities;
using Domain.Entities.Ponto;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class RegistroPontoRepository : IRegistroPontoRepository
    {
        #region conexão
        private readonly DbSession _dbSession;

        public RegistroPontoRepository(DbSession dbSession)
        {
            _dbSession = dbSession;
        }
        #endregion

        #region metodos
        public async Task<int> InserirAsync(RegistroPontoModel ponto)
        {
            string sql = @"INSERT INTO REGISTRO_PONTO 
               (ID_USUARIO, HORA_REGISTRO, DATA_REGISTRO,ID_TIPO_REGISTRO_PONTO) 
               VALUES (@IdUsuario, @HoraRegistro, @DataRegistro, @IdTipoRegistroPonto);";
            return await _dbSession.Connection.ExecuteAsync(sql, ponto, _dbSession.Transaction);
        }

        public async Task<RegistroPontoModel> ObterPorIdAsync(int id)
        {
            string sql = @"SELECT ID_REGISTRO, ID_USUARIO, HORA_REGISTRO, DATA_REGISTRO, ID_TIPO_REGISTRO_PONTO
                   FROM REGISTRO_PONTO
                   WHERE ID_REGISTRO = @IdRegistro;";
            return await _dbSession.Connection.QueryFirstOrDefaultAsync<RegistroPontoModel>(sql, new { IdRegistro = id });
        }


        public async Task<IEnumerable<RegistroPontoModel>> ListarTodosAsync()
        {
            string sql = @"SELECT ID_REGISTRO, ID_USUARIO, HORA_REGISTRO, DATA_REGISTRO, ID_TIPO_REGISTRO_PONTO
                   FROM REGISTRO_PONTO";
            return await _dbSession.Connection.QueryAsync<RegistroPontoModel>(sql);
        }
        public async Task<bool> ExcluirAsync(int id)
        {
            string sql = @"DELETE FROM REGISTRO_PONTO WHERE ID_REGISTRO = @IdRegistro;";
            int linhasAfetadas = await _dbSession.Connection.ExecuteAsync(sql, new { IdRegistro = id }, _dbSession.Transaction);
            return linhasAfetadas > 0;
        }

        public async Task<IEnumerable<RegistroPontoModel>> ObterRegistrosUsuarioAsync(int idUsuario)
        {
            string sql = @"SELECT ID_REGISTRO, ID_USUARIO, HORA_REGISTRO, DATA_REGISTRO, ID_TIPO_REGISTRO_PONTO
                   FROM REGISTRO_PONTO WHERE ID_USUARIO =  @IdUsuario;";
            return await _dbSession.Connection.QueryAsync<RegistroPontoModel>(sql, new { IdUsuario = idUsuario });
        }

        public async Task<bool> VerificarValidacaoMesAsync(int idUsuario, int anoReferencia, int mesReferencia)
        {
            string sql = @"SELECT COUNT(1) FROM VALIDACOES_MENSAIS
                   WHERE ID_USUARIO = @IdUsuario
                     AND ANO_REFERENCIA = @AnoReferencia
                     AND MES_REFERENCIA = @MesReferencia;";

            int validado = await _dbSession.Connection.ExecuteScalarAsync<int>(sql, new { IdUsuario = idUsuario, AnoReferencia = anoReferencia, MesReferencia = mesReferencia });

            return validado > 0;
        }

        public async Task<int> ValidarMesAsync(int idUsuario, int anoReferencia, int mesReferencia,int statusValidacao)
        {
            string sql = @"INSERT INTO VALIDACOES_MENSAIS (ID_USUARIO, MES_REFERENCIA, ANO_REFERENCIA, STATUS_VALIDACAO)
                   VALUES (@IdUsuario, @AnoReferencia, @MesReferencia, @StatusValidacao);";

            return await _dbSession.Connection.ExecuteAsync(sql, new
            {
                IdUsuario = idUsuario,
                AnoReferencia = anoReferencia,
                MesReferencia = mesReferencia,
                StatusValidacao = statusValidacao
            }, _dbSession.Transaction);
        }
        public async Task<int> CriarSolicitacaoAsync(SolicitacaoAjustePontoModel solicitacao)
        {
            var sqlSolicitacao = @"
                INSERT INTO SOLICITACAO_AJUSTE_PONTO
                    (JUSTIFICATIVA, STATUS_SOLICITACAO, DATA_SOLICITACAO, ID_GESTOR_RESPONSAVEL)
                VALUES
                    (@Justificativa, @StatusSolicitacao, NOW(), @IdGestorResponsavel);
                SELECT LAST_INSERT_ID();";

            var idSolicitacao = await _dbSession.Connection.ExecuteScalarAsync<int>(
                sqlSolicitacao, solicitacao, _dbSession.Transaction
            );

            foreach (var item in solicitacao.Itens)
            {
                var sqlItem = $@"
                INSERT INTO ITEM_AJUSTE_PONTO
                    (ID_SOLICITACAO, DATA_REGISTRO, HORA_REGISTRO, ID_TIPO_REGISTRO_PONTO,ID_REGISTRO)
                VALUES
                    ({idSolicitacao}, @DataRegistro, @HoraRegistro, @IdTipoRegistroPonto,@IdRegistro);";

                await _dbSession.Connection.ExecuteAsync(
                    sqlItem, item, _dbSession.Transaction
                );
            }
            return idSolicitacao;
        }
        public async Task<IEnumerable<SolicitacaoAjustePontoModel>> ListarSolicitacoesAsync()
        {
            string sql = @"
            SELECT 
                s.ID_SOLICITACAO,
                s.JUSTIFICATIVA,
                s.STATUS_SOLICITACAO,
                s.DATA_SOLICITACAO,
                s.ID_GESTOR_RESPONSAVEL,
    
                i.ID_ITEM,
                i.DATA_REGISTRO,
                i.HORA_REGISTRO,
                i.ID_TIPO_REGISTRO_PONTO,
                i.ID_REGISTRO,
                i.ID_SOLICITACAO AS ITEM_ID_SOLICITACAO
            FROM 
                SOLICITACAO_AJUSTE_PONTO s
            LEFT JOIN 
            ITEM_AJUSTE_PONTO i 
            ON s.ID_SOLICITACAO = i.ID_SOLICITACAO;";

            var solicitacoes = new Dictionary<int, SolicitacaoAjustePontoModel>();

            await _dbSession.Connection.QueryAsync<SolicitacaoAjustePontoModel, ItemAjustePontoModel, SolicitacaoAjustePontoModel>(
            sql,
            (sol, item) =>
            {
                if (!solicitacoes.TryGetValue(sol.IdSolicitacao, out var solicitacao))
                {
                    solicitacao = sol;
                    solicitacao.Itens = new List<ItemAjustePontoModel>();
                    solicitacoes.Add(solicitacao.IdSolicitacao, solicitacao);
                }

                if (item != null)
                    solicitacao.Itens.Add(item);

                return solicitacao;
            },
            //Diz ao Dapper onde começa o segundo objeto
            splitOn: "ID_ITEM"
            );

            return solicitacoes.Values;
        }

        public async Task<SolicitacaoAjustePontoModel> ObterSolicitacaoAltercaoPorIdAsync(int idSolicitacao)
        {
            string sql = @"
                        SELECT 
                            s.ID_SOLICITACAO,
                            s.JUSTIFICATIVA,
                            s.STATUS_SOLICITACAO,
                            s.DATA_SOLICITACAO,
                            s.ID_GESTOR_RESPONSAVEL,
                            i.ID_ITEM,
                            i.DATA_REGISTRO,
                            i.HORA_REGISTRO,
                            i.ID_TIPO_REGISTRO_PONTO,
                            i.ID_REGISTRO,
                            i.ID_SOLICITACAO AS ITEM_ID_SOLICITACAO
                        FROM 
                            SOLICITACAO_AJUSTE_PONTO s
                        LEFT JOIN 
                            ITEM_AJUSTE_PONTO i 
                        ON s.ID_SOLICITACAO = i.ID_SOLICITACAO
                        WHERE s.ID_SOLICITACAO = @IdSolicitacao;";

            var lookup = new Dictionary<int, SolicitacaoAjustePontoModel>();

            await _dbSession.Connection.QueryAsync<SolicitacaoAjustePontoModel, ItemAjustePontoModel, SolicitacaoAjustePontoModel>(
                sql,
                (sol, item) =>
                {
                    if (!lookup.TryGetValue(sol.IdSolicitacao, out var solicitacao))
                    {
                        solicitacao = sol;
                        solicitacao.Itens = new List<ItemAjustePontoModel>();
                        lookup.Add(solicitacao.IdSolicitacao, solicitacao);
                    }

                    if (item != null)
                        solicitacao.Itens.Add(item);

                    return solicitacao;
                },
                new { IdSolicitacao = idSolicitacao },
                transaction: _dbSession.Transaction,
                splitOn: "ID_ITEM"
            );

            return lookup.Values.FirstOrDefault();
        }

        public async Task<bool> AtualizarRegistroAsync(int idSolicitacao, bool aprovar, List<ItemAjustePontoModel> itensAlterados, IDbTransaction transaction)
        {
            try
            {
                // Atualiza o status da solicitação de ajuste
                string sqlSolicitacao = @"
            UPDATE SOLICITACAO_AJUSTE_PONTO
            SET STATUS_SOLICITACAO = @StatusSolicitacao
            WHERE ID_SOLICITACAO = @IdSolicitacao;";

                var statusSolicitacao = aprovar ? 1 : 2;
                await _dbSession.Connection.ExecuteAsync(sqlSolicitacao, new { StatusSolicitacao = statusSolicitacao, IdSolicitacao = idSolicitacao }, transaction);

                if (aprovar)
                {
                    foreach (var item in itensAlterados)
                    {
                        string sqlUpdateItem = @"
                    UPDATE REGISTRO_PONTO
                    SET HORA_REGISTRO = @HoraRegistro, DATA_REGISTRO = @DataRegistro, ID_TIPO_REGISTRO_PONTO = @IdTipoRegistroPonto
                    WHERE ID_REGISTRO = @IdRegistro;";

                        await _dbSession.Connection.ExecuteAsync(sqlUpdateItem, new
                        {
                            HoraRegistro = item.HoraRegistro,
                            DataRegistro = item.DataRegistro,
                            IdTipoRegistroPonto = item.IdTipoRegistroPonto,
                            IdRegistro = item.IdRegistro
                        }, transaction);
                    }
                }
                else
                {
                    string sqlDeleteItems = @"
                DELETE FROM ITEM_AJUSTE_PONTO
                WHERE ID_SOLICITACAO = @IdSolicitacao;";

                    await _dbSession.Connection.ExecuteAsync(sqlDeleteItems, new { IdSolicitacao = idSolicitacao }, transaction);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion

    }
}

