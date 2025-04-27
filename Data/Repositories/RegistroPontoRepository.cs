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

        public async Task<bool> AtualizarAsync(RegistroPontoModel ponto)
        {
            string sql = @"UPDATE REGISTRO_PONTO 
                   SET HORA_REGISTRO = @HoraRegistro,
                       DATA_REGISTRO = @DataRegistro,
                       ID_TIPO_REGISTRO_PONTO = @IdTipoRegistroPonto
                   WHERE ID_REGISTRO = @IdRegistro;";
            int linhasAfetadas = await _dbSession.Connection.ExecuteAsync(sql, ponto, _dbSession.Transaction);
            return linhasAfetadas > 0;
        }

        public async Task<bool> ExcluirAsync(int id)
        {
            string sql = @"DELETE FROM REGISTRO_PONTO WHERE ID_REGISTRO = @IdRegistro;";
            int linhasAfetadas = await _dbSession.Connection.ExecuteAsync(sql, new { IdRegistro = id }, _dbSession.Transaction);
            return linhasAfetadas > 0;
        }
        #endregion

    }
}
