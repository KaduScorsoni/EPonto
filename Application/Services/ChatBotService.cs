using Application.DTOs;
using Application.Interfaces;
using Data.Interfaces;
using Domain.Entities;
using Domain.Entities.WhatsApp;
using System;
using System.Globalization;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ChatBotService : IChatBotService
    {
        private readonly IWhatsAppRepository _whatsAppRepository;
        private readonly IWhatsAppService _whatsAppService;
        private readonly IFeriasService _feriasService;
        private readonly IUsuarioService _usuarioService;

        public ChatBotService(
            IWhatsAppRepository whatsAppRepository,
            IWhatsAppService whatsAppService,
            IFeriasService feriasService,
            IUsuarioService usuarioService)
        {
            _whatsAppRepository = whatsAppRepository;
            _whatsAppService = whatsAppService;
            _feriasService = feriasService;
            _usuarioService = usuarioService;
        }

        public async Task<ResultadoDTO> ProcessMessage(string phoneNumber, string message)
        {
            try
            {
                var session = await GetOrCreateSession(phoneNumber);
                var response = "";

                message = message.ToLower().Trim();

                // Comandos principais
                if (message.Contains("menu") || message.Contains("ajuda") || message.Contains("help"))
                {
                    response = await GetHelpMessage();
                    session.CurrentCommand = null;
                }
                else if (message.Contains("ferias") || message.Contains("vacation"))
                {
                    session.CurrentCommand = "FERIAS_MENU";
                    response = "🏖️ *Menu de Férias*\n\n" +
                              "1️⃣ Solicitar férias\n" +
                              "2️⃣ Consultar saldo de férias\n" +
                              "3️⃣ Listar minhas férias\n" +
                              "4️⃣ Cancelar solicitação\n\n" +
                              "Digite o número da opção desejada:";
                }
                else if (session.CurrentCommand == "FERIAS_MENU")
                {
                    response = await ProcessVacationMenuOption(session, message);
                }
                else if (session.CurrentCommand?.StartsWith("SOLICITAR_FERIAS") == true)
                {
                    var result = await ProcessVacationCommand(session, message);
                    response = result.Mensagem;
                }
                else
                {
                    response = "Desculpe, não entendi. Digite *menu* para ver as opções disponíveis.";
                }

                // Atualizar sessão
                await _whatsAppRepository.UpdateSession(session);

                // Enviar resposta
                await _whatsAppService.SendMessage(phoneNumber, response);

                return new ResultadoDTO { Sucesso = true, Mensagem = "Mensagem processada com sucesso." };
            }
            catch (Exception ex)
            {
                await _whatsAppService.SendMessage(phoneNumber, "Ocorreu um erro interno. Tente novamente mais tarde.");
                return new ResultadoDTO { Sucesso = false, Mensagem = $"Erro: {ex.Message}" };
            }
        }

        private async Task<string> ProcessVacationMenuOption(ChatSessionModel session, string message)
        {
            switch (message)
            {
                case "1":
                    session.CurrentCommand = "SOLICITAR_FERIAS_INICIO";
                    return "📅 Para solicitar férias, informe a data de início no formato DD/MM/AAAA:";

                case "2":
                    return await ConsultarSaldoFerias(session);

                case "3":
                    return await ListarFerias(session);

                case "4":
                    session.CurrentCommand = "CANCELAR_SOLICITACAO";
                    return "❌ Para cancelar uma solicitação, primeiro vou listar suas solicitações pendentes...";

                default:
                    return "Opção inválida. Digite um número de 1 a 4.";
            }
        }

        public async Task<ResultadoDTO> ProcessVacationCommand(ChatSessionModel session, string message)
        {
            try
            {
                var sessionData = string.IsNullOrEmpty(session.SessionData) 
                    ? new { } 
                    : JsonSerializer.Deserialize<dynamic>(session.SessionData);

                switch (session.CurrentCommand)
                {
                    case "SOLICITAR_FERIAS_INICIO":
                        if (DateTime.TryParseExact(message, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dataInicio))
                        {
                            session.SessionData = JsonSerializer.Serialize(new { DataInicio = dataInicio });
                            session.CurrentCommand = "SOLICITAR_FERIAS_FIM";
                            return new ResultadoDTO { Sucesso = true, Mensagem = "📅 Agora informe a data de término no formato DD/MM/AAAA:" };
                        }
                        return new ResultadoDTO { Sucesso = false, Mensagem = "Data inválida. Use o formato DD/MM/AAAA (ex: 15/12/2024):" };

                    case "SOLICITAR_FERIAS_FIM":
                        if (DateTime.TryParseExact(message, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dataFim))
                        {
                            var data = JsonSerializer.Deserialize<VacationSessionData>(session.SessionData);
                            
                            if (dataFim <= data.DataInicio)
                            {
                                return new ResultadoDTO { Sucesso = false, Mensagem = "A data de término deve ser posterior à data de início. Informe novamente:" };
                            }

                            session.SessionData = JsonSerializer.Serialize(new { DataInicio = data.DataInicio, DataFim = dataFim });
                            session.CurrentCommand = "SOLICITAR_FERIAS_DESCRICAO";
                            return new ResultadoDTO { Sucesso = true, Mensagem = "📝 Por favor, informe uma descrição para suas férias:" };
                        }
                        return new ResultadoDTO { Sucesso = false, Mensagem = "Data inválida. Use o formato DD/MM/AAAA:" };

                    case "SOLICITAR_FERIAS_DESCRICAO":
                        var finalData = JsonSerializer.Deserialize<VacationSessionData>(session.SessionData);
                        
                        var feriasModel = new FeriasModel
                        {
                            DscFerias = message,
                            DatIncioFerias = finalData.DataInicio,
                            DatFimFerias = finalData.DataFim,
                            IdUsuario = session.IdUsuario
                        };

                        var resultado = await _feriasService.CadastrarFerias(feriasModel);
                        
                        session.CurrentCommand = null;
                        session.SessionData = null;
                        
                        if (resultado.Sucesso)
                        {
                            return new ResultadoDTO 
                            { 
                                Sucesso = true, 
                                Mensagem = $"✅ Férias solicitadas com sucesso!\n\n" +
                                          $"📅 Período: {finalData.DataInicio:dd/MM/yyyy} a {finalData.DataFim:dd/MM/yyyy}\n" +
                                          $"📝 Descrição: {message}\n\n" +
                                          $"Sua solicitação está aguardando aprovação."
                            };
                        }
                        return resultado;

                    default:
                        return new ResultadoDTO { Sucesso = false, Mensagem = "Comando não reconhecido." };
                }
            }
            catch (Exception ex)
            {
                return new ResultadoDTO { Sucesso = false, Mensagem = $"Erro ao processar comando: {ex.Message}" };
            }
        }

        private async Task<string> ConsultarSaldoFerias(ChatSessionModel session)
        {
            try
            {
                var saldo = await _feriasService.RetornaSaldoFerias(session.IdUsuario);
                
                if (saldo.Sucesso && saldo.ListaSaldoFerias?.Any() == true)
                {
                    var response = "💰 *Seu Saldo de Férias:*\n\n";
                    foreach (var item in saldo.ListaSaldoFerias)
                    {
                        response += $"📅 Período: {item.AnoReferencia}\n";
                        response += $"⏰ Dias disponíveis: {item.DiasDisponiveis}\n";
                        response += $"✅ Dias utilizados: {item.DiasUtilizados}\n\n";
                    }
                    return response;
                }
                return "Você não possui saldo de férias disponível no momento.";
            }
            catch
            {
                return "Erro ao consultar saldo de férias. Tente novamente mais tarde.";
            }
        }

        private async Task<string> ListarFerias(ChatSessionModel session)
        {
            try
            {
                var ferias = await _feriasService.ListarFerias(session.IdUsuario);
                
                if (ferias.Sucesso && ferias.ListaFerias?.Any() == true)
                {
                    var response = "🏖️ *Suas Férias:*\n\n";
                    foreach (var item in ferias.ListaFerias)
                    {
                        response += $"📅 {item.DatIncioFerias:dd/MM/yyyy} a {item.DatFimFerias:dd/MM/yyyy}\n";
                        response += $"📝 {item.DscFerias}\n";
                        response += $"📊 Status: {item.StatusFerias}\n\n";
                    }
                    return response;
                }
                return "Você não possui férias cadastradas.";
            }
            catch
            {
                return "Erro ao listar férias. Tente novamente mais tarde.";
            }
        }

        public async Task<ChatSessionModel> GetOrCreateSession(string phoneNumber)
        {
            var session = await _whatsAppRepository.GetActiveSession(phoneNumber);
            
            if (session == null)
            {
                session = new ChatSessionModel
                {
                    PhoneNumber = phoneNumber,
                    DataInicio = DateTime.Now,
                    DataUltimaInteracao = DateTime.Now,
                    IsActive = true
                };
                
                // Tentar identificar usuário pelo telefone
                // Aqui você pode implementar a lógica para associar o telefone ao usuário
                
                await _whatsAppRepository.CreateSession(session);
            }
            else
            {
                session.DataUltimaInteracao = DateTime.Now;
            }
            
            return session;
        }

        public async Task<string> GetHelpMessage()
        {
            return "🤖 *EPonto Bot - Menu Principal*\n\n" +
                   "Bem-vindo ao sistema de ponto digital!\n\n" +
                   "📋 *Comandos disponíveis:*\n\n" +
                   "🏖️ *ferias* - Gerenciar suas férias\n" +
                   "❓ *menu* - Exibir este menu\n\n" +
                   "💡 *Dica:* Digite a palavra-chave do que deseja fazer!";
        }

        private class VacationSessionData
        {
            public DateTime DataInicio { get; set; }
            public DateTime DataFim { get; set; }
        }
    }
}
