using Dapper;
using Data.Connections;
using Data.Interfaces;
using Domain.Entities.WhatsApp;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class WhatsAppRepository : IWhatsAppRepository
    {
        private readonly DbSession _dbSession;

        public WhatsAppRepository(DbSession dbSession)
        {
            _dbSession = dbSession;
        }

        public async Task<WhatsAppConfigModel> GetActiveConfig()
        {
            var sql = @"
                SELECT IdConfig, PhoneNumberId, AccessToken, VerifyToken, 
                       BusinessAccountId, PhoneNumber, IsActive, 
                       DataCriacao, DataAtualizacao
                FROM WhatsAppConfig 
                WHERE IsActive = 1 
                LIMIT 1";

            return await _dbSession.Connection.QueryFirstOrDefaultAsync<WhatsAppConfigModel>(sql, transaction: _dbSession.Transaction);
        }

        public async Task SaveConfig(WhatsAppConfigModel config)
        {
            // Desativar configurações anteriores
            var deactivateSql = "UPDATE WhatsAppConfig SET IsActive = 0";
            await _dbSession.Connection.ExecuteAsync(deactivateSql, transaction: _dbSession.Transaction);

            var sql = @"
                INSERT INTO WhatsAppConfig 
                (PhoneNumberId, AccessToken, VerifyToken, BusinessAccountId, 
                 PhoneNumber, IsActive, DataCriacao)
                VALUES 
                (@PhoneNumberId, @AccessToken, @VerifyToken, @BusinessAccountId, 
                 @PhoneNumber, @IsActive, @DataCriacao)";

            config.IsActive = true;
            config.DataCriacao = System.DateTime.Now;

            await _dbSession.Connection.ExecuteAsync(sql, config, transaction: _dbSession.Transaction);
        }

        public async Task<ChatSessionModel> GetActiveSession(string phoneNumber)
        {
            var sql = @"
                SELECT IdSession, PhoneNumber, UserName, IdUsuario, 
                       CurrentCommand, SessionData, DataInicio, 
                       DataUltimaInteracao, IsActive
                FROM ChatSession 
                WHERE PhoneNumber = @PhoneNumber AND IsActive = 1";

            return await _dbSession.Connection.QueryFirstOrDefaultAsync<ChatSessionModel>(sql, 
                new { PhoneNumber = phoneNumber }, transaction: _dbSession.Transaction);
        }

        public async Task CreateSession(ChatSessionModel session)
        {
            var sql = @"
                INSERT INTO ChatSession 
                (PhoneNumber, UserName, IdUsuario, CurrentCommand, 
                 SessionData, DataInicio, DataUltimaInteracao, IsActive)
                VALUES 
                (@PhoneNumber, @UserName, @IdUsuario, @CurrentCommand, 
                 @SessionData, @DataInicio, @DataUltimaInteracao, @IsActive)";

            await _dbSession.Connection.ExecuteAsync(sql, session, transaction: _dbSession.Transaction);
        }

        public async Task UpdateSession(ChatSessionModel session)
        {
            var sql = @"
                UPDATE ChatSession 
                SET CurrentCommand = @CurrentCommand,
                    SessionData = @SessionData,
                    DataUltimaInteracao = @DataUltimaInteracao
                WHERE IdSession = @IdSession";

            await _dbSession.Connection.ExecuteAsync(sql, session, transaction: _dbSession.Transaction);
        }
    }
}
