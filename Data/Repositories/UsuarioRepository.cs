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
               (NOME, DATA_NASCIMENTO, SENHA, EMAIL, ID_CARGO, ID_JORNADA, TELEFONE,FOTO_PERFIL) 
               VALUES (@Nome, @DataNascimento, @Senha, @Email, @IdCargo, @IdJornada, @Telefone,@FotoPerfil);";
            return await _dbSession.Connection.ExecuteAsync(sql, usuario, _dbSession.Transaction);
        }

        public async Task<UsuarioModel> ObterPorIdAsync(int id)
        {
            string sql = @"SELECT ID_USUARIO, NOME, DATA_NASCIMENTO, SENHA, EMAIL, ID_CARGO, ID_JORNADA, TELEFONE , IND_ATIVO,FOTO_PERFIL
                   FROM USUARIO
                   WHERE ID_USUARIO = @IdUsuario;";
            return await _dbSession.Connection.QueryFirstOrDefaultAsync<UsuarioModel>(sql, new { IdUsuario = id });
        }
        public async Task<ContratoUsuarioModel> ObterContratoUsuarioAsync(int id)
        {
            string sql = @"SELECT 
                        U.ID_USUARIO,U.NOME,U.IND_ATIVO,U.DATA_CADASTRO,U.DATA_NASCIMENTO,U.EMAIL,U.TELEFONE,U.FOTO_PERFIL,C.NOME_CARGO,C.SALARIO,J.QTD_HORAS_DIARIAS
                        FROM USUARIO U
                        INNER JOIN CARGO C 
                        ON U.ID_CARGO = C.ID_CARGO
                        INNER JOIN JORNADA_TRABALHO J 
                        ON U.ID_JORNADA = J.ID_JORNADA
                        WHERE U.ID_USUARIO = @IdUsuario;";
            return await _dbSession.Connection.QueryFirstOrDefaultAsync<ContratoUsuarioModel>(sql, new { IdUsuario = id });
        }

        public async Task<IEnumerable<UsuarioModel>> ListarTodosAsync()
        {
            string sql = @"SELECT ID_USUARIO, NOME, DATA_NASCIMENTO, SENHA, EMAIL, ID_CARGO, ID_JORNADA, TELEFONE, IND_ATIVO,FOTO_PERFIL
                   FROM USUARIO;";
            return await _dbSession.Connection.QueryAsync<UsuarioModel>(sql);
        }

        public async Task<bool> AtualizarAsync(UsuarioModel usuario)
        {
            string sql = @"UPDATE USUARIO 
                   SET NOME = @Nome,
                       DATA_NASCIMENTO = @DataNascimento,
                       EMAIL = @Email,
                       ID_CARGO = @IdCargo,
                       ID_JORNADA = @IdJornada,
                       TELEFONE = @Telefone,
                       FOTO_PERFIL = @FotoPerfil
                   WHERE ID_USUARIO = @IdUsuario;";
            int linhasAfetadas = await _dbSession.Connection.ExecuteAsync(sql, usuario, _dbSession.Transaction);
            return linhasAfetadas > 0;
        }

        public async Task<bool> ExcluirAsync(int id)
        {
            string sql = @"UPDATE USUARIO SET IND_ATIVO = 0 WHERE ID_USUARIO = @IdUsuario;";
            int linhasAfetadas = await _dbSession.Connection.ExecuteAsync(sql, new { IdUsuario = id }, _dbSession.Transaction);
            return linhasAfetadas > 0;
        }

        public async Task<UsuarioModel?> ValidarEmail(string email)
        {
            string sql = "SELECT * FROM USUARIO WHERE EMAIL = @Email";
            return await _dbSession.Connection.QueryFirstOrDefaultAsync<UsuarioModel>(
                sql,
                new { Email = email },
                _dbSession.Transaction 
            );
        }

        #endregion
    }
}
