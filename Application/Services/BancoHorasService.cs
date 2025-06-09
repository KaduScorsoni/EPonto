using Application.DTOs;
using Application.Interfaces;
using Data.Connections;
using Data.Interfaces;
using Domain.Entities;

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

            // 1. Processar o saldo do dia atual
            var (saldo, inconsistente) = await _bancoHorasRepository.CalcularSaldoDiarioAsync(idUsuario, data);
            await _bancoHorasRepository.InserirSaldoDiarioAsync(idUsuario, data, saldo, inconsistente);

            // 2. Verificar se há dias inconsistentes que podem ter sido corrigidos
            var diasInconsistentes = await _bancoHorasRepository.ObterDiasInconsistentesAsync(idUsuario);

            foreach (var dia in diasInconsistentes)
            {
                var (saldoDia, aindaInconsistente) = await _bancoHorasRepository.CalcularSaldoDiarioAsync(idUsuario, dia);

                if (!aindaInconsistente)
                {
                    // Atualiza o dia que foi corrigido
                    await _bancoHorasRepository.AtualizarSaldoDiarioInconsistenteAsync(idUsuario, dia, saldoDia.Value, false);
                }
            }

            // 3. Recalcular banco de horas total
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
    public async Task<BancoHorasDTO> ObterBancoHorasAtualAsync(int idUsuario)
    {
        try
        {
            var (horasTrabalhadas, saldo) = await _bancoHorasRepository.ObterBancoHorasAtualAsync(idUsuario);

            return new BancoHorasDTO
            {
                Sucesso = true,
                BancoHoras = new List<BancoHorasModel>
            {
                new BancoHorasModel
                {
                    IdUsuario = idUsuario,
                    HorasTrabalhadas = horasTrabalhadas,
                    Saldo = saldo
                }
            }
            };
        }
        catch (Exception ex)
        {
            return new BancoHorasDTO
            {
                Sucesso = false,
                Mensagem = $"Erro ao obter banco de horas atual: {ex.Message}"
            };
        }
    }

}
