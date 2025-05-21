using Dapper;
using Data.Connections;
using Data.Interfaces;
using Data.Util;
using Domain.Entities;
using Domain.Entities.Login;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<List<FeriadoModel>> ListarFeriados()
        {
            string sql = @"
                    	SELECT F.DSC_FERIADO,
		                       F.ID_FERIADO,
		                       F.DAT_FERIADO,
		                       F.TIPO_FERIADO
	                      FROM FERIADO F
                      ORDER BY F.DAT_FERIADO ASC ";

            List<FeriadoModel> lista = new List<FeriadoModel>();

            using (var reader = _dbSession.Connection.ExecuteReader(sql))
            {
                while (reader.Read())
                {
                    lista.Add(new FeriadoModel
                    {
                        DscFeriado = reader["DSC_FERIADO"].ToString(),
                        IdFeriado = reader["ID_FERIADO"].ToLong(),
                        DatFeriado = reader["DAT_FERIADO"].ToDateTime()
                    });
                }
                return lista;
            }
        }
    }
}
