using Dapper;
using Data.Connections;
using Data.Interfaces;
using Data.Util;
using Domain.Entities;
using Domain.Entities.Feriado_e_Ferias;
using static Domain.Entities.Enum.EnumGerais;

namespace Data.Repositories
{
    public class FeriadoRepository : IFeriadoRepository
    {
        #region conexão
        private readonly DbSession _dbSession;

        public FeriadoRepository(DbSession dbSession)
        {
            _dbSession = dbSession;
        }
        #endregion
        public async Task<int> CadastrarFeriado(FeriadoModel param)
        {
            string sql = @"
                INSERT INTO FERIADO (
                    DSC_FERIADO,
                    DAT_FERIADO,
                    TIPO_FERIADO
                )
                VALUES (
                    @DSC_FERIADO,
                    @DAT_FERIADO,
                    @TIPO_FERIADO
                )";

            object auxParametros = new
            {
                DSC_FERIADO = param.DscFeriado,
                DAT_FERIADO = param.DatFeriado,
                TIPO_FERIADO = param.IndTipoFeriado
            };

            return await _dbSession.Connection.ExecuteAsync(sql, auxParametros, _dbSession.Transaction);
        }

        public async Task<int> DeletarFeriado(int idFeriado)
        {
            string sql = @"
                DELETE FROM FERIADO F
                      WHERE F.ID_FERIADO = @ID_FERIADO";

            object auxParametros = new { ID_FERIADO = idFeriado };

            return await _dbSession.Connection.ExecuteAsync(sql, auxParametros, _dbSession.Transaction);
        }

        public async Task<List<ResultadoFeriadoModel>> ListarFeriados()
        {
            string sql = @"
                    	SELECT F.DSC_FERIADO,
		                       F.ID_FERIADO,
		                       F.DAT_FERIADO,
		                       F.TIPO_FERIADO
	                      FROM FERIADO F
                      ORDER BY F.DAT_FERIADO ASC ";

            List<ResultadoFeriadoModel> lista = new List<ResultadoFeriadoModel>();

            using (var reader = _dbSession.Connection.ExecuteReader(sql))
            {
                while (reader.Read())
                {
                    lista.Add(new ResultadoFeriadoModel
                    {
                        DscFeriado = reader["DSC_FERIADO"].ToString(),
                        IdFeriado = reader["ID_FERIADO"].ToLong(),
                        DatFeriado = reader["DAT_FERIADO"].ToDateTime(),
                        IndTipoFeriado = (SituacaoFeriado)reader["TIPO_FERIADO"].ToInt()
                    });
                }
                return lista;
            }
        }

        public async Task<int>CadastrarFerias(FeriasModel param)
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
