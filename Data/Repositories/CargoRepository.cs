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
    public class CargoRepository : ICargoRepository
    {
        #region conexão
        private readonly DbSession _dbSession;

        public CargoRepository(DbSession dbSession)
        {
            _dbSession = dbSession;
        }
        #endregion

        #region metodos
        public async Task<int> InserirAsync(CargoModel cargo)
        {
            string sql = @"INSERT INTO CARGO 
               (NOME_CARGO, SALARIO, IND_ATIVO) 
               VALUES (@NomeCargo, @Salario, 1);";
            return await _dbSession.Connection.ExecuteAsync(sql, cargo, _dbSession.Transaction);
        }
        public async Task<CargoModel> ObterPorIdAsync(int id)
        {
            string sql = @"SELECT ID_CARGO, NOME_CARGO, SALARIO, IND_ATIVO
                   FROM CARGO
                   WHERE ID_CARGO = @IdCargo;";
            return await _dbSession.Connection.QueryFirstOrDefaultAsync<CargoModel>(sql, new { IdCargo = id });
        }

        public async Task<IEnumerable<CargoModel>> ListarTodosAsync()
        {
            string sql = @"SELECT ID_CARGO, NOME_CARGO, SALARIO, IND_ATIVO
                   FROM CARGO;";
            return await _dbSession.Connection.QueryAsync<CargoModel>(sql);
        }

        public async Task<bool> AtualizarAsync(CargoModel cargo)
        {
            string sql = @"UPDATE CARGO 
                   SET NOME_CARGO = @NomeCargo,
                       SALARIO = @Salario
                   WHERE ID_CARGO = @IdCargo;";
            int linhasAfetadas = await _dbSession.Connection.ExecuteAsync(sql, cargo, _dbSession.Transaction);
            return linhasAfetadas > 0;
        }

        public async Task<bool> ExcluirAsync(int id)
        {
            string sql = @"UPDATE CARGO SET IND_ATIVO = 0 WHERE ID_CARGO = @IdCargo;";
            int linhasAfetadas = await _dbSession.Connection.ExecuteAsync(sql, new { IdCargo = id }, _dbSession.Transaction);
            return linhasAfetadas > 0;
        }
        public async Task<CargoModel?> ValidarCargoExistente(string nomeCargo)
        {
            string sql = "SELECT * FROM CARGO WHERE NOME_CARGO = @NomeCargo";
            return await _dbSession.Connection.QueryFirstOrDefaultAsync<CargoModel>(
                sql,
                new { NomeCargo = nomeCargo },
                _dbSession.Transaction
            );
        }

        #endregion
    }
}
