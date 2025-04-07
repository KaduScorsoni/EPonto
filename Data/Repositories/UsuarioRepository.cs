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
            string sql = @"INSERT INTO USUARIO 
               (NOME, DATA_NASCIMENTO, SENHA, BATIDA_ATUAL, EMAIL, ID_CARGO, ID_JORNADA, TELEFONE) 
               VALUES (@Nome, @DataNascimento, @Senha, @BatidaAtual, @Email, @IdCargo, @IdJornada, @Telefone);";
            return await _dbSession.Connection.ExecuteAsync(sql, usuario, _dbSession.Transaction);
        }

        public async Task<UsuarioModel> ObterPorIdAsync(int id)
        {
            string sql = @"SELECT ID_USUARIO, NOME, DATA_NASCIMENTO, SENHA, BATIDA_ATUAL, EMAIL, ID_CARGO, ID_JORNADA, TELEFONE
                   FROM USUARIO
                   WHERE ID_USUARIO = @IdUsuario;";
            return await _dbSession.Connection.QueryFirstOrDefaultAsync<UsuarioModel>(sql, new { IdUsuario = id });
        }

        public async Task<IEnumerable<UsuarioModel>> ListarTodosAsync()
        {
            string sql = @"SELECT ID_USUARIO, NOME, DATA_NASCIMENTO, SENHA, BATIDA_ATUAL, EMAIL, ID_CARGO, ID_JORNADA, TELEFONE
                   FROM USUARIO;";
            return await _dbSession.Connection.QueryAsync<UsuarioModel>(sql);
        }

        public async Task<bool> AtualizarAsync(UsuarioModel usuario)
        {
            string sql = @"UPDATE USUARIO 
                   SET NOME = @Nome,
                       DATA_NASCIMENTO = @DataNascimento,
                       SENHA = @Senha,
                       BATIDA_ATUAL = @BatidaAtual,
                       EMAIL = @Email,
                       ID_CARGO = @IdCargo,
                       ID_JORNADA = @IdJornada,
                       TELEFONE = @Telefone
                   WHERE ID_USUARIO = @IdUsuario;";
            int linhasAfetadas = await _dbSession.Connection.ExecuteAsync(sql, usuario, _dbSession.Transaction);
            return linhasAfetadas > 0;
        }
        //TO DO ESQUECI DE N VAMOS EXLUIR SO INATIVAR O INDICE
        public async Task<bool> ExcluirAsync(int id)
        {
            string sql = @"DELETE FROM USUARIO WHERE ID_USUARIO = @IdUsuario;";
            int linhasAfetadas = await _dbSession.Connection.ExecuteAsync(sql, new { IdUsuario = id }, _dbSession.Transaction);
            return linhasAfetadas > 0;
        }

        #endregion
    }
}
