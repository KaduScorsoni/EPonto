using Application.DTOs;
using Application.Interfaces;
using Data.Connections;
using Data.Interfaces;
using Domain.Entities.Perfil;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Services
{
    public class PerfilService : IPerfilService
    {
        private readonly IPerfilRepository _perfilRepository;
        private readonly DbSession _dbSession;

        public PerfilService(IPerfilRepository perfilRepository, DbSession dbSession)
        {
            _perfilRepository = perfilRepository;
            _dbSession = dbSession;
        }

        public async Task<ResultadoDTO> CadastrarPerfil(PerfilModel param)
        {
            if (string.IsNullOrEmpty(param.DscPerfil))
                return new ResultadoDTO { Sucesso = false, Mensagem = "É obrigatório informar um nome de Perfil." };

            try
            {
                _dbSession.BeginTransaction();
                await _perfilRepository.CadastrarPerfil(param);
                _dbSession.Commit();
                return new ResultadoDTO { Sucesso = true, Mensagem = "Perfil cadastrado com sucesso." };
            }
            catch (Exception ex)
            {
                _dbSession.Rollback();
                return new ResultadoDTO { Sucesso = false, Mensagem = $"Erro ao cadastrar perfil: {ex.Message}" };
            }
        }

        public async Task<PerfilDTO> ListarPerfis()
        {
            try
            {
                var lista = await _perfilRepository.ListarPerfis();
                return new PerfilDTO
                {
                    Sucesso = true,
                    Mensagem = lista.Count > 0 ? "Perfis listados com sucesso." : "Não há perfis cadastrados.",
                    ListaPerfis = lista
                };
            }
            catch (Exception ex)
            {
                return new PerfilDTO { Sucesso = false, Mensagem = $"Erro ao listar perfis: {ex.Message}" };
            }
        }
        public async Task<PerfilDTO> ListarPerfil(int idPerfil)
        {
            if (idPerfil <= 0)
                return new PerfilDTO { Sucesso = false, Mensagem = "ID de perfil inválido." };
           
            try
            {
                PerfilModel perfil = await _perfilRepository.ListarPerfil(idPerfil);
                return new PerfilDTO
                {
                    Sucesso = !string.IsNullOrEmpty(perfil.DscPerfil),
                    Mensagem = !string.IsNullOrEmpty(perfil.DscPerfil) ? "Perfil listado com sucesso." : "Perfil não localizado",
                    ListaPerfis = [perfil]
                };
            }
            catch (Exception ex)
            {
                return new PerfilDTO { Sucesso = false, Mensagem = $"Erro ao listar perfis: {ex.Message}" };
            }
        }
        public async Task<ResultadoDTO> EditarPerfil(PerfilModel param)
        {
            if (param.IdPerfil <= 0)
                return new ResultadoDTO { Sucesso = false, Mensagem = "ID de perfil inválido." };

            try
            {
                _dbSession.BeginTransaction();
                await _perfilRepository.EditarPerfil(param);
                _dbSession.Commit();
                return new ResultadoDTO { Sucesso = true, Mensagem = "Perfil editado com sucesso." };
            }
            catch (Exception ex)
            {
                _dbSession.Rollback();
                return new ResultadoDTO { Sucesso = false, Mensagem = $"Erro ao editar perfil: {ex.Message}" };
            }
        }

        public async Task<ResultadoDTO> RemoverPerfil(int idPerfil)
        {
            if (idPerfil <= 0)
                return new ResultadoDTO { Sucesso = false, Mensagem = "ID de perfil inválido." };

            try
            {
                _dbSession.BeginTransaction();

                //Deve ser feita uma validação para verificar se o perfil está vinculado a algum usuário antes de remover
                bool auxPerfilEmUso = await _perfilRepository.VerificaPerfilEmUso(idPerfil);
                if (auxPerfilEmUso)
                    return new ResultadoDTO { Sucesso = false, Mensagem = "Não é possível remover o perfil, pois ele está vinculado a um ou mais usuários." };
                
                await _perfilRepository.RemoverPerfil(idPerfil);
                _dbSession.Commit();
                return new ResultadoDTO { Sucesso = true, Mensagem = "Perfil removido com sucesso." };
            }
            catch (Exception ex)
            {
                _dbSession.Rollback();
                return new ResultadoDTO { Sucesso = false, Mensagem = $"Erro ao remover perfil: {ex.Message}" };
            }
        }
        public async Task<ResultadoDTO> CadastrarVinculoPerfilUsuario(VinculoPerfilUsuario param)
        {
            if (param.IdUsuario <= 0)
                return new ResultadoDTO { Sucesso = false, Mensagem = "ID de usuário inválido." };

            try
            {
                _dbSession.BeginTransaction();

                await _perfilRepository.RemoveVinculoPerfilUsuario(param.IdUsuario);

                foreach (var idPerfil in param.ListaIdPerfis)
                    await _perfilRepository.CadastrarVinculoPerfilUsuario(param.IdUsuario, idPerfil);

                _dbSession.Commit();
                return new ResultadoDTO { Sucesso = true, Mensagem = "Vinculo cadastrado com sucesso." };
            }
            catch (Exception ex)
            {
                _dbSession.Rollback();
                return new ResultadoDTO { Sucesso = false, Mensagem = $"Erro ao cadastrar o vinculo: {ex.Message}" };
            }
        }
    }
}
