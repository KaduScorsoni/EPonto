using Dapper;
using Data.Connections;
using Data.Interfaces;
using Domain.Entities.Perfil;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class PerfilRepository : IPerfilRepository
    {
        private readonly DbSession _dbSession;

        public PerfilRepository(DbSession dbSession)
        {
            _dbSession = dbSession;
        }

        public async Task<int> CadastrarPerfil(PerfilModel param)
        {
            string sql = @"
                INSERT INTO PERFIL (
                    DSC_PERFIL,
                    IND_ACESSO_ADMIN,
                    IND_PERMITE_CADASTRAR,
                    IND_PERMITE_DELETAR,
                    IND_PERMITE_EDITAR,
                    IND_PERMITE_REGULAR_SOLICITACOES
                )
                VALUES (
                    @DSC_PERFIL,
                    @IND_ACESSO_ADMIN,
                    @IND_PERMITE_CADASTRAR,
                    @IND_PERMITE_DELETAR,
                    @IND_PERMITE_EDITAR,
                    @IND_PERMITE_REGULAR_SOLICITACOES
                )";

            var parametros = new
            {
                DSC_PERFIL = param.DscPerfil,
                IND_ACESSO_ADMIN = param.IndAcessoAdmin,
                IND_PERMITE_CADASTRAR = param.IndPermiteCadastrar,
                IND_PERMITE_DELETAR = param.IndPermiteDeletar,
                IND_PERMITE_EDITAR = param.IndPermiteEditar,
                IND_PERMITE_REGULAR_SOLICITACOES = param.IndPermiteRegularSolicitacoes
            };

            return await _dbSession.Connection.ExecuteAsync(sql, parametros, _dbSession.Transaction);
        }

        public async Task<List<PerfilModel>> ListarPerfis()
        {
            string sql = @"SELECT P.* 
                             FROM PERFIL P";
            var perfis = await _dbSession.Connection.QueryAsync<PerfilModel>(sql);
            return perfis.AsList();
        }
        public async Task<PerfilModel> ListarPerfil(int idPerfil)
        {
            string sql = @"SELECT P.* 
                             FROM PERFIL P
                            WHERE P.ID_PERFIL = @ID_PERFIL";

            var parametros = new
            {
                ID_PERFIL = idPerfil
            };

            return await _dbSession.Connection.QueryFirstOrDefaultAsync<PerfilModel>(sql, parametros);
        }

        public async Task<int> EditarPerfil(PerfilModel param)
        {
            string sql = @"
                UPDATE PERFIL
                   SET DSC_PERFIL = @DSC_PERFIL,
                       IND_ACESSO_ADMIN = @IND_ACESSO_ADMIN,
                       IND_PERMITE_CADASTRAR = @IND_PERMITE_CADASTRAR,
                       IND_PERMITE_DELETAR = @IND_PERMITE_DELETAR,
                       IND_PERMITE_EDITAR = @IND_PERMITE_EDITAR,
                       IND_PERMITE_REGULAR_SOLICITACOES = @IND_PERMITE_REGULAR_SOLICITACOES
                 WHERE ID_PERFIL = @ID_PERFIL";

            var parametros = new
            {
                DSC_PERFIL = param.DscPerfil,
                IND_ACESSO_ADMIN = param.IndAcessoAdmin,
                IND_PERMITE_CADASTRAR = param.IndPermiteCadastrar,
                IND_PERMITE_DELETAR = param.IndPermiteDeletar,
                IND_PERMITE_EDITAR = param.IndPermiteEditar,
                IND_PERMITE_REGULAR_SOLICITACOES = param.IndPermiteRegularSolicitacoes,
                ID_PERFIL = param.IdPerfil
            };

            return await _dbSession.Connection.ExecuteAsync(sql, parametros, _dbSession.Transaction);
        }

        public async Task<int> RemoverPerfil(int idPerfil)
        {
            string sql = @"DELETE FROM PERFIL WHERE ID_PERFIL = @ID_PERFIL";
            var parametros = new { ID_PERFIL = idPerfil };
            return await _dbSession.Connection.ExecuteAsync(sql, parametros, _dbSession.Transaction);
        }
        public async Task<bool> VerificaPerfilEmUso(int idPerfil)
        {
            string sql = @"SELECT COUNT(1) AS QTD
                             FROM PERFIL_USUARIO PU
                            WHERE PU.ID_PERFIL = @ID_PERFIL";

            var parametros = new
            {
                ID_PERFIL = idPerfil
            };

            int count = await _dbSession.Connection.ExecuteScalarAsync<int>(sql, parametros);
            return count > 0;
        }
        public async Task<int> CadastrarVinculoPerfilUsuario(int idUsuario, int idPerfil)
        {
            string sql = @"
                INSERT INTO PERFIL_USUARIO (
                    ID_USUARIO,
                    ID_PERFIL        
                )
                VALUES (
                    @ID_USUARIO,
                    @ID_PERFIL
                )";

            var parametros = new
            {
                ID_USUARIO = idUsuario,
                ID_PERFIL = idPerfil
            };

            return await _dbSession.Connection.ExecuteAsync(sql, parametros, _dbSession.Transaction);
        }
        public async Task<int> RemoveVinculoPerfilUsuario(int idUsuario)
        {
            string sql = @"
                DELETE FROM PERFIL_USUARIO PU
                      WHERE PU.ID_USUARIO = @ID_USUARIO";

            var parametros = new
            {
                ID_USUARIO = idUsuario
            };

            return await _dbSession.Connection.ExecuteAsync(sql, parametros, _dbSession.Transaction);
        }
    }
}
