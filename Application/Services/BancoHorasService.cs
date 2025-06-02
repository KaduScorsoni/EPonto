using Application.DTOs;
using Application.Interfaces;
using Data.Connections;
using Data.Interfaces;

public class BancoHorasService : IBancoHorasService
{
    private readonly IBancoHorasRepository _bancoHorasRepository;
    private readonly IJornadaTrabalhoRepository _jornadaTrabalhoRepository;
    private readonly DbSession _dbSession;

    public BancoHorasService(IBancoHorasRepository bancoHorasRepository,
                             IJornadaTrabalhoRepository jornadaTrabalhoRepository,
                             DbSession dbSession)
    {
        _bancoHorasRepository = bancoHorasRepository;
        _jornadaTrabalhoRepository = jornadaTrabalhoRepository;
        _dbSession = dbSession;
    }

    public async Task<BancoHorasDTO> ProcessarBancoHorasDiarioAsync(int idUsuario, DateTime data)
    {
        try
        {
            _dbSession.BeginTransaction();

            // 1. Calcular saldo do dia
            var (saldo, inconsistente) = await _bancoHorasRepository.CalcularSaldoDiarioAsync(idUsuario, data);

            // 2. Inserir saldo diário
            await _bancoHorasRepository.InserirSaldoDiarioAsync(idUsuario, data, saldo, inconsistente);

            // 3. Calcular banco de horas total
            var (horasTrabalhadas, saldoTotal) = await _bancoHorasRepository.CalcularBancoHorasAsync(idUsuario);

            // 4. Inserir/atualizar banco de horas
            await _bancoHorasRepository.InserirBancoHorasAsync(idUsuario, horasTrabalhadas, saldoTotal);

            _dbSession.Commit();

            return new BancoHorasDTO
            {
                Sucesso = true,
                Mensagem = "Banco de horas processado com sucesso."
            };
        }
        catch (Exception ex)
        {
            _dbSession.Rollback();
            return new BancoHorasDTO
            {
                Sucesso = false,
                Mensagem = $"Erro ao processar banco de horas: {ex.Message}"
            };
        }
    }

    public async Task<BancoHorasDTO> ObterSaldosUsuarioAsync(int idUsuario)
    {
        try
        {
            var saldos = await _bancoHorasRepository.ObterSaldosUsuarioAsync(idUsuario);

            return new BancoHorasDTO
            {
                Sucesso = true,
                SaldosDiarios = saldos
            };
        }
        catch (Exception ex)
        {
            return new BancoHorasDTO
            {
                Sucesso = false,
                Mensagem = $"Erro ao obter saldo diário do usuário: {ex.Message}"
            };
        }
    }
}
