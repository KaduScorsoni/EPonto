using Dapper;
using Data.Connections;
using Data.Interfaces;
using Data.Util;
using Domain.Entities;
using Domain.Entities.Login;
using Domain.Entities.Perfil;
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

            return _dbSession.Connection.ExecuteScalar(sql, auxParametros).ToBool();
        }
        public async Task<bool> SalvaCodigoRecuperacao(int codigo, string email)
        {
            string sql = @"
                INSERT INTO LOGIN (
                    ID_USUARIO,
                    COD_RECUPERACAO,
                    DAT_LOGIN
                )
                VALUES (
                    (SELECT U.ID_USUARIO FROM USUARIO U WHERE U.EMAIL = @EMAIL AND U.IND_ATIVO = 1),
                    @CODIGO,
                    NOW()
                )
                ";

            object auxParametros = new
            {
                CODIGO = codigo,
                EMAIL = email
            };

            return _dbSession.Connection.ExecuteScalar(sql, auxParametros).ToBool();
        }
        public async Task<int> BuscaCodigoEmail(string email)
        {
            string sql = @"
                    SELECT L.COD_RECUPERACAO
                      FROM LOGIN L
                     WHERE L.DAT_LOGIN = (SELECT MAX(LO.DAT_LOGIN)
                                            FROM LOGIN LO
                                           WHERE LO.ID_USUARIO = (SELECT U1.ID_USUARIO 
                                                                    FROM USUARIO U1
                                                                   WHERE U1.EMAIL = @EMAIL 
                                                                     AND U1.IND_ATIVO = 1))
                       AND L.ID_USUARIO = (SELECT U2.ID_USUARIO 
                                             FROM USUARIO U2 
                                            WHERE U2.EMAIL = @EMAIL 
                                              AND U2.IND_ATIVO = 1)
                ";

            object auxParametros = new
            {
                EMAIL = email
            };

            int result = 0;

            using (var reader = _dbSession.Connection.ExecuteReader(sql, auxParametros))
            {
                if (reader.Read())
                    result = reader["COD_RECUPERACAO"].ToInt();
            }

            return result;
        }
        public async Task<bool> SalvaAlteracaoSenha(string senha, string email)
        {
            string sql = @"
                    UPDATE USUARIO U
                       SET U.SENHA = @SENHA
                     WHERE U.EMAIL = @EMAIL
                ";

            object auxParametros = new
            {
                EMAIL = email,
                SENHA = senha
            };

            return _dbSession.Connection.ExecuteScalar(sql, auxParametros).ToBool();
        }
        public async Task<List<IdDescricaoPerfilModel>> RetornaPerfilUsuario(long idUsuario)
        {
            string sql = @"SELECT P.ID_PERFIL,
	                              P.DSC_PERFIL
                             FROM PERFIL_USUARIO PU
                             JOIN PERFIL P ON P.ID_PERFIL = PU.ID_PERFIL
                            WHERE PU.ID_USUARIO = @ID_USUARIO";

            var parametros = new
            {
                ID_USUARIO = idUsuario
            };

            var perfis = await _dbSession.Connection.QueryAsync<IdDescricaoPerfilModel>(sql, parametros);
            return perfis.AsList();
        }
    }
}
