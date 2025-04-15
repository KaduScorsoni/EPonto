using Dapper;
using Data.Connections;
using Data.Interfaces;
using Data.Util;
using Domain.Entities;
using Domain.Entities.Login;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class LoginRepository : ILoginRepository
    {
        #region conexão
        private readonly DbSession _dbSession;

        public LoginRepository(DbSession dbSession)
        {
            _dbSession = dbSession;
        }
        #endregion
        public async Task<LoginAuxiliarModel> BuscaUsuarioNoSistema(string email)
        {
            string sql = @"
                    SELECT U.ID_USUARIO,
                           U.SENHA
                      FROM USUARIO U
                     WHERE U.EMAIL = @EMAIL
                       AND U.IND_ATIVO = 1
                ";

            object auxParametros = new
            {
                EMAIL = email
            };

            using (var reader = _dbSession.Connection.ExecuteReader(sql, auxParametros))
            {
                if (reader.Read())
                {
                    return new LoginAuxiliarModel 
                    { 
                        Senha = reader["SENHA"].ToString(),
                        IdUsuario = reader["ID_USUARIO"].ToLong() 
                    };
                }
            }

            return new LoginAuxiliarModel
            {
                Senha = "",
                IdUsuario = 0
            };
        }
        public async Task<bool> InsereRegistroLogin(long IdUsuario, string token)
        {
            string sql = @"
                INSERT INTO LOGIN (
                    ID_USUARIO,
                    TOKEN,
                    DAT_LOGIN
                )
                VALUES (
                    @ID_USUARIO,
                    @TOKEN,
                    NOW()
                )
                ";

            object auxParametros = new
            {
                ID_USUARIO = IdUsuario,
                TOKEN = token
            };

            return _dbSession.Connection.ExecuteReader(sql, auxParametros).ToBool();
        }
        public async Task<bool> SalvaCodigoRecuperacao(int codigo, string email)
        {
            string sql = @"
                INSERT INTO LOGIN (
                    ID_USUARIO,
                    COD_RECUPERACAO
                )
                VALUES (
                    (SELECT U.ID_USUARIO FROM USUARIO U WHERE U.EMAIL = @EMAIL AND U.IND_ATIVO = 1),
                    @CODIGO
                )
                ";

            object auxParametros = new
            {
                CODIGO = codigo,
                EMAIL = email
            };

            return _dbSession.Connection.ExecuteReader(sql, auxParametros).ToBool();
        }
    }
}
