using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Data.Connections
{
    // Classe responsável por gerenciar a sessão de conexão com o banco de dados
    public class DbSession : IDisposable
    {
        // Conexão com o banco de dados
        private readonly IDbConnection _connection;

        // Transação atual (se houver)
        private IDbTransaction _transaction;

        // Construtor que recebe a configuração para obter a string de conexão do banco
        public DbSession(IConfiguration configuration)
        {
            // Inicializa a conexão com o banco de dados utilizando a string de conexão do appsettings.json
            _connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));

            // Abre a conexão imediatamente
            _connection.Open();
        }

        // Propriedade que expõe a conexão com o banco
        public IDbConnection Connection => _connection;

        // Propriedade que expõe a transação ativa
        public IDbTransaction Transaction => _transaction;

        // Inicia uma transação no banco de dados
        public void BeginTransaction()
        {
            _transaction = _connection.BeginTransaction();
        }

        // Confirma a transação, persistindo as mudanças no banco de dados
        public void Commit()
        {
            _transaction?.Commit();
            _transaction = null; // Limpa a referência após o commit
        }

        // Reverte a transação, desfazendo as mudanças realizadas
        public void Rollback()
        {
            _transaction?.Rollback();
            _transaction = null;
        }

        // Libera os recursos da conexão e da transação ao descartar a instância da classe
        public void Dispose()
        {
            _transaction?.Dispose(); // Libera a transação se ainda existir
            _connection?.Dispose();
        }
    }
}
