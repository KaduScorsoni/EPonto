using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Data.Connections;
using Data.Interfaces;
using Domain.Entities;
namespace Data.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        #region conexão
        private readonly DbSession _dbSession;

        public UsuarioRepository(DbSession dbSession)
        {
            _dbSession = dbSession;
        }
        #endregion

        #region metodos
        public async Task<int> InserirAsync(UsuarioModel usuario)
        {
            string sql = @"INSERT INTO `railway`.`USUARIO`
                        (`NOME`,
                        `DATA_NASCIMENTO`,
                        `SENHA`,
                        `BATIDA_ATUAL`,
                        `EMAIL`,
                        `ID_CARGO`,
                        `ID_JORNADA`,
                        `TELEFONE`);";
            return await _dbSession.Connection.ExecuteAsync(sql, usuario, _dbSession.Transaction);
        }

        public async Task<UsuarioModel> ObterPorIdAsync(int id)
        {
            string sql = "";
            return await _dbSession.Connection.QueryFirstOrDefaultAsync<UsuarioModel>(sql, new { Id = id });
        }

        public async Task<IEnumerable<UsuarioModel>> ListarTodosAsync()
        {
            string sql = "";
            return await _dbSession.Connection.QueryAsync<UsuarioModel>(sql);
        }

        public async Task<bool> AtualizarAsync(UsuarioModel usuario)
        {
            string sql = "";
            int linhasAfetadas = await _dbSession.Connection.ExecuteAsync(sql, usuario, _dbSession.Transaction);
            return linhasAfetadas > 0;
        }

        public async Task<bool> ExcluirAsync(int id)
        {
            string sql = "";
            int linhasAfetadas = await _dbSession.Connection.ExecuteAsync(sql, new { Id = id }, _dbSession.Transaction);
            return linhasAfetadas > 0;
        }
        #endregion
    }
}
