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

namespace Data.Repositories
{
    public class FeriasRepository
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
    }
}
