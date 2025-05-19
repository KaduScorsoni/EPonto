using Application.DTOs;
using Application.Interfaces;
using Data.Connections;
using Data.Interfaces;
using Data.Repositories;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class CargoService : ICargoService
    {
        private readonly ICargoRepository _cargoRepository;
        private readonly DbSession _dbSession;

        public CargoService(ICargoRepository cargoRepository, DbSession dbSession)
        {
            _cargoRepository = cargoRepository;
            _dbSession = dbSession;
        }

        public async Task<CargoDTO> CriarCargoAsync(CargoModel cargo)
        {
            try
            {
                _dbSession.BeginTransaction();

                var cargoExistente = await _cargoRepository.ValidarCargoExistente(cargo.NomeCargo);
                if (cargoExistente != null)
                {
                    return new CargoDTO
                    {
                        Sucesso = false,
                        Mensagem = "Já existe um cargo cadastrado com este nome."
                    };
                }
                var cargoCriado = await _cargoRepository.InserirAsync(cargo);

                _dbSession.Commit();
                return new CargoDTO
                {
                    Sucesso = true,
                    Mensagem = "Cargo cadastrado com sucesso."
                };
            }
            catch (Exception ex)
            {
                _dbSession.Rollback();
                return new CargoDTO
                {
                    Sucesso = false,
                    Mensagem = $"Erro ao criar cargo: {ex.Message}"
                };
            }
        }

        public async Task<CargoDTO> ObterCargoPorIdAsync(int id)
        {
            try
            {
                var cargo = await _cargoRepository.ObterPorIdAsync(id);
                if (cargo == null)
                {
                    return new CargoDTO
                    {
                        Sucesso = false,
                        Mensagem = "Cargo não encontrado."
                    };
                }

                return new CargoDTO
                {
                    Sucesso = true,
                    Cargo = cargo
                };
            }
            catch (Exception ex)
            {
                return new CargoDTO
                {
                    Sucesso = false,
                    Mensagem = $"Erro ao buscar cargo: {ex.Message}"
                };
            }
        }

        public async Task<CargoDTO> ListarTodosCargosAsync()
        {
            try
            {
                var cargos = await _cargoRepository.ListarTodosAsync();
                return new CargoDTO
                {
                    Sucesso = true,
                    Cargos = cargos
                };
            }
            catch (Exception ex)
            {
                return new CargoDTO
                {
                    Sucesso = false,
                    Mensagem = $"Erro ao listar cargos: {ex.Message}"
                };
            }
        }
        public async Task<CargoDTO> AtualizarCargoAsync(CargoModel cargo)
        {
            try
            {
                _dbSession.BeginTransaction();

                var sucesso = await _cargoRepository.AtualizarAsync(cargo);
                _dbSession.Commit();

                return new CargoDTO
                {
                    Sucesso = sucesso,
                    Mensagem = sucesso ? "Cargo atualizado com sucesso." : "Falha ao atualizar cargo."
                };
            }
            catch (Exception ex)
            {
                _dbSession.Rollback();
                return new CargoDTO
                {
                    Sucesso = false,
                    Mensagem = $"Erro ao atualizar cargo: {ex.Message}"
                };
            }
        }
        public async Task<CargoDTO> ExcluirCargoAsync(int id)
        {
            try
            {
                _dbSession.BeginTransaction();

                var sucesso = await _cargoRepository.ExcluirAsync(id);
                _dbSession.Commit();

                return new CargoDTO
                {
                    Sucesso = sucesso,
                    Mensagem = sucesso ? "Cargo excluído com sucesso." : "Falha ao excluir cargo."
                };
            }
            catch (Exception ex)
            {
                _dbSession.Rollback();
                return new CargoDTO
                {
                    Sucesso = false,
                    Mensagem = $"Erro ao excluir cargo: {ex.Message}"
                };
            }
        }
    }
}
