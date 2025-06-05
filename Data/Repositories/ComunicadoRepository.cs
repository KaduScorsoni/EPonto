using Dapper;
using Data.Connections;
using Data.Interfaces;
using Data.Util;
using Domain.Entities;
using Domain.Entities.Comunicado;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class ComunicadoRepository : IComunicadoRepository
    {
        #region conexão
        private readonly DbSession _dbSession;

        public ComunicadoRepository(DbSession dbSession)
        {
            _dbSession = dbSession;
        }
        #endregion
        public async Task<int> CadastrarComunicado(ComunicadoModel param)
        {
            string sql = @"
                INSERT INTO COMUNICADO (
                    TITULO_COMUNICADO,
                    DSC_COMUNICADO,
                    DAT_INICIO_EXIBICAO,
                    DAT_FIM_EXIBICAO
                )
                VALUES (
                    @TITULO_COMUNICADO,
                    @DSC_COMUNICADO,
                    @DAT_INICIO_EXIBICAO,
                    @DAT_FIM_EXIBICAO
                )";

            object auxParametros = new
            {
                TITULO_COMUNICADO = param.TituloComunicado,
                DSC_COMUNICADO = param.DscComunicado,
                DAT_INICIO_EXIBICAO = param.DatInicioExibicao,
                DAT_FIM_EXIBICAO = param.DatFimExibicao
            };
            return await _dbSession.Connection.ExecuteAsync(sql, auxParametros, _dbSession.Transaction);
        }

        public async Task<int> DeletarComunicado(int idComunicado)
        {
            string sql = @"
                DELETE FROM COMUNICADO C
                      WHERE F.ID_COMUNICADO = @ID_COMUNICADO";

            object auxParametros = new { ID_COMUNICADO = idComunicado };

            return await _dbSession.Connection.ExecuteAsync(sql, auxParametros, _dbSession.Transaction);
        }

        public async Task<List<ComunicadoModel>> ListarComunicado()
        {
            string sql = @"
                    	SELECT C.ID_COMUNICADO,
                               C.TITULO_COMUNICADO,
		                       C.DSC_COMUNICADO,
		                       C.DAT_INICIO_EXIBICAO,
		                       C.DAT_FIM_EXIBICAO
	                      FROM COMUNICADO C
                      ORDER BY F.DAT_INICIO_EXIBICAO ASC ";

            List<ComunicadoModel> lista = new List<ComunicadoModel>();

            using (var reader = _dbSession.Connection.ExecuteReader(sql))
            {
                while (reader.Read())
                {
                    lista.Add(new ComunicadoModel
                    {
                        IdComunicado = reader["ID_COMUNICADO"].ToInt(),
                        TituloComunicado = reader["TITULO_COMUNICADO"].ToString(),
                        DscComunicado = reader["DSC_COMUNICADO"].ToString(),
                        DatInicioExibicao = reader["DAT_INICIO_EXIBICAO"].ToDateTime(),
                        DatFimExibicao = reader["DAT_FIM_EXIBICAO"].ToDateTime()
                    });
                }
                return lista;
            }
        }
    }
}
