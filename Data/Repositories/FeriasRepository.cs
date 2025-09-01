using Dapper;
using Data.Connections;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Feriado_e_Ferias;
using Data.Util;
using Data.Interfaces;

namespace Data.Repositories
{
    public class FeriasRepository : IFeriasRepository
    {
        #region conexão
        private readonly DbSession _dbSession;

        public FeriasRepository(DbSession dbSession)
        {
            _dbSession = dbSession;
        }
        #endregion
        public async Task<int> CadastrarFerias(FeriasModel param)
        {
            string sql = @"
                INSERT INTO FERIAS (
                    DSC_FERIAS,
                    DAT_INICIO_FERIAS,
                    DAT_FIM_FERIAS,
                    ID_USUARIO
                )
                VALUES (
                    @DSC_FERIAS,
                    @DAT_INICIO_FERIAS,
                    @DAT_FIM_FERIAS,
                    @ID_USUARIO
                )";

            object auxParametros = new
            {
                DSC_FERIAS = param.DscFerias,
                DAT_INICIO_FERIAS = param.DatIncioFerias,
                DAT_FIM_FERIAS = param.DatFimFerias,
                ID_USUARIO = param.IdUsuario
            };

            return await _dbSession.Connection.ExecuteAsync(sql, auxParametros, _dbSession.Transaction);
        }

        public async Task<int> DeletarFerias(int idFerias)
        {
            string sql = @"
                DELETE FROM FERIAS F
                      WHERE F.ID_FERIAS = @ID_FERIAS";

            object auxParametros = new { ID_FERIAS = idFerias };

            return await _dbSession.Connection.ExecuteAsync(sql, auxParametros, _dbSession.Transaction);
        }

        public async Task<List<ResultadoFeriasModel>> ListarFerias(int? idUsuario)
        {
            string sql = @"
                        SELECT *
                          FROM (
                    	    SELECT F.DSC_FERIAS,
		                           F.ID_FERIAS,
		                           F.DAT_INICIO_FERIAS,
		                           F.DAT_FIM_FERIAS
	                          FROM FERIAS F
                             WHERE F.ID_USUARIO IS NULL

                            UNION

                            SELECT F.DSC_FERIAS,
		                           F.ID_FERIAS,
		                           F.DAT_INICIO_FERIAS,
		                           F.DAT_FIM_FERIAS
	                          FROM FERIAS F
                             WHERE F.ID_USUARIO = @ID_USUARIO
                
                        ) T
                 ORDER BY T.DAT_INICIO_FERIAS ASC";

            object auxParametros = new { ID_USUARIO = idUsuario };

            List<ResultadoFeriasModel> lista = new List<ResultadoFeriasModel>();

            using (var reader = _dbSession.Connection.ExecuteReader(sql, auxParametros))
            {
                while (reader.Read())
                {
                    lista.Add(new ResultadoFeriasModel
                    {
                        DscFerias = reader["DSC_FERIAS"].ToString(),
                        IdFerias = reader["ID_FERIAS"].ToLong(),
                        DatIncioFerias = reader["DAT_INICIO_FERIAS"].ToDateTime(),
                        DatFimFerias = reader["DAT_FIM_FERIAS"].ToDateTime()
                    });
                }
                return lista;
            }
        }
        public async Task<int> CadastrarSolicitacaoFerias(SolicitacaoFeriasModel param)
        {
            string sql = @"
                INSERT INTO SOLICITACAO_FERIAS (
                    ID_USUARIO,
                    DSC_OBSERVACAO,
                    DAT_INICIO_FERIAS,
                    DAT_FIM_FERIAS,
                    DAT_SOLICITACAO
                )
                VALUES (
                    @ID_USUARIO,
                    @DSC_OBSERVACAO,
                    @DAT_INICIO_FERIAS,
                    @DAT_FIM_FERIAS,
                    NOW()
                )";

            object auxParametros = new
            {
                ID_USUARIO = param.IdUsuario,
                DSC_OBSERVACAO = param.DscObservacao,
                DAT_INICIO_FERIAS = param.DatInicioFerias,
                DAT_FIM_FERIAS = param.DatFimFerias
            };

            return await _dbSession.Connection.ExecuteAsync(sql, auxParametros, _dbSession.Transaction);
        }
        public async Task<List<ResultadoSolicitacaoFeriasModel>> ListarSolicitacoesFerias(int? idUsuario)
        {
            string sql = @"
                        SELECT SF.ID_SOLICITACAO_FERIAS,
	                           SF.ID_USUARIO,
                               SF.DSC_OBSERVACAO,
                               SF.IND_SITUACAO,
                               SF.DAT_INICIO_FERIAS,
                               SF.DAT_FIM_FERIAS,
                               SF.DAT_SOLICITACAO,
                               U.NOME AS NOME_USUARIO
                          FROM SOLICITACAO_FERIAS SF
                          JOIN USUARIO U ON U.ID_USUARIO = SF.ID_USUARIO
                         WHERE SF.ID_USUARIO = IFNULL(@ID_USUARIO, SF.ID_USUARIO)";

            object auxParametros = new { ID_USUARIO = idUsuario };

            List<ResultadoSolicitacaoFeriasModel> lista = new List<ResultadoSolicitacaoFeriasModel>();

            using (var reader = _dbSession.Connection.ExecuteReader(sql, auxParametros))
            {
                while (reader.Read())
                {
                    lista.Add(new ResultadoSolicitacaoFeriasModel
                    {
                        IdSolicFerias = reader["ID_SOLICITACAO_FERIAS"].ToInt(),
                        IdUsuario = reader["ID_USUARIO"].ToInt(),
                        DscObservacao = reader["DSC_OBSERVACAO"].ToString(),
                        DatInicioFerias = reader["DAT_INICIO_FERIAS"].ToDateTime(),
                        DatFimFerias = reader["DAT_FIM_FERIAS"].ToDateTime(),
                        DatSolicitacaoFerias = reader["DAT_SOLICITACAO"].ToDateTime(),
                        NomeUsuario = reader["NOME_USUARIO"].ToString(),
                        IndSituacao = reader["IND_SITUACAO"].ToInt()
                    });
                }
                return lista;
            }
        }

        public async Task<List<SaldoFeriasModel>> RetornaSaldoFerias(int? idUsuario)
        {
            string sql = @"
                        SELECT U.NOME AS NOME_USUARIO,
	                           SF.QTD_SALDO,
                               SF.ID_USUARIO
                          FROM SALDO_FERIAS SF
                          JOIN USUARIO U ON U.ID_USUARIO = SF.ID_USUARIO
                         WHERE SF.ID_USUARIO = IFNULL(@ID_USUARIO, SF.ID_USUARIO)";

            object auxParametros = new { ID_USUARIO = idUsuario };

            List<SaldoFeriasModel> lista = new List<SaldoFeriasModel>();

            using (var reader = _dbSession.Connection.ExecuteReader(sql, auxParametros))
            {
                while (reader.Read())
                {
                    lista.Add(new SaldoFeriasModel
                    {
                        IdUsuario = reader["ID_USUARIO"].ToInt(),
                        QtdSaldo = reader["QTD_SALDO"].ToInt(),
                        NomeUsuario = reader["NOME_USUARIO"].ToString(),
                    });
                }
                return lista;
            }
        }
        public async Task<int> AtualizaSolicitacaoFerias(int idSolicitacao, int indSituacao)
        {
            string sql = @"
                UPDATE SOLICITACAO_FERIAS SF
                   SET SF.IND_SITUACAO = @IND_SITUACAO
                 WHERE SF.ID_SOLICITACAO_FERIAS = @ID_SOLICITACAO_FERIAS";

            object auxParametros = new
            {
                IND_SITUACAO = indSituacao,
                ID_SOLICITACAO_FERIAS = idSolicitacao
            };

            return await _dbSession.Connection.ExecuteAsync(sql, auxParametros, _dbSession.Transaction);
        }
    }
}
