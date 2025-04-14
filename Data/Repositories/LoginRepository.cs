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
                     WHERE U.EMAIL = PEMAIL
                       AND U.IND_ATIVO = 1
                ";

            object auxParametros = new
            {
                PEMAIL = email
            };

            using (var reader = _dbSession.Connection.ExecuteReader(sql, auxParametros))
            {
                if (reader.Read())
                {
                    return new LoginAuxiliarModel 
                    { 
                        Senha = reader["ID_USUARIO"].ToString(),
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
    }
}
