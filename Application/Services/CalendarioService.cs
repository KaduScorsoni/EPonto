using Application.DTOs;
using Application.Interfaces;
using Data.Connections;
using Data.Interfaces;
using Domain.Entities;
using Domain.Entities.Calendario;
using Domain.Entities.Feriado_e_Ferias;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class CalendarioService : ICalendarioService
    {
        private readonly IFeriadoRepository _feriadoRepository;
        public CalendarioService(IFeriadoRepository feriadoRepository)
        {
            _feriadoRepository = feriadoRepository;
        }
        public async Task<CalendarioDTO> MontaCalendario(int ano, int? idUsuario)
        {
            try
            {
                List<CalendarioModel> calendario = new List<CalendarioModel>();

                List<ResultadoFeriasModel> ListaFerias = await _feriadoRepository.ListarFerias(idUsuario);
                List<ResultadoFeriadoModel> ListaFeriados = await _feriadoRepository.ListarFeriados();
                
                //Inserindo os feriados
                foreach (ResultadoFeriadoModel item in ListaFeriados)
                {
                    calendario.Add(new CalendarioModel
                    {
                        DscEvento = item.DscFeriado,
                        DatEvento = item.DatFeriado,
                        TipoEvento = 1 //feriado
                        
                    });
                }

                //Inserindo as ferias
                foreach (ResultadoFeriasModel item in ListaFerias)
                {
                    for (var data = item.DatIncioFerias; data <= item.DatFimFerias; data = data.AddDays(1))
                    {
                        calendario.Add(new CalendarioModel
                        {
                            DscEvento = item.DscFerias,
                            DatEvento = data,
                            TipoEvento = 2 //ferias

                        });
                    }
                }

                //Falta ainda buscar o saldo de horas e usar a propriedade do ano para
                //montar o calendario apenas do ano informado.

                calendario = calendario.OrderBy(c => c.DatEvento).ToList();

                if (calendario.Count > 0)
                    return new CalendarioDTO { Dias = calendario, Sucesso = true };
                else
                    return new CalendarioDTO { Sucesso = true, Mensagem = "Ainda não existem registros no Calendario!" };
                
            }
            catch (Exception ex)
            {
                return new CalendarioDTO {  Sucesso = false, Mensagem = $"Erro ao montar o calendario: {ex.Message}" };
            }
        }
    }
}
