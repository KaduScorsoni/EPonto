using Dapper;
using Data.Connections;
using Data.Interfaces;
using Data.Util;
using Domain.Entities.Feriado_e_Ferias;
using Domain.Entities.Relatorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class RelatorioRepository : IRelatorioRepository
    {
        #region conexão
        private readonly DbSession _dbSession;

        public RelatorioRepository(DbSession dbSession)
        {
            _dbSession = dbSession;
        }
        #endregion

        public async Task<List<RelHorasExtrasModel>> RelatorioHorasExtras(DateTime datInicio, DateTime datFim, int? IdCargo, long? IdUsuario)
        {
            string sql = @"call PROC_RELATORIO_HORAS_EXTRAS(@DAT_INICIO, @DAT_FIM, @ID_CARGO, @ID_USUARIO)";

            object auxParametros = new
            {
                DAT_INICIO = datInicio,
                DAT_FIM = datFim,
                ID_USUARIO = IdUsuario == 0 ? null : IdUsuario,
                ID_CARGO = IdCargo == 0 ? null : IdCargo
            };

            List<RelHorasExtrasModel> lista = new List<RelHorasExtrasModel>();

            using (var reader = _dbSession.Connection.ExecuteReader(sql, auxParametros))
            {
                while (reader.Read())
                {
                    lista.Add(new RelHorasExtrasModel
                    {
                        NomeUsuario = reader["NOME_USUARIO"].ToString(),
                        SaldoHoras = reader["SALDO"].ToString(),
                        Cargo = reader["NOME_CARGO"].ToString(),
                        JornadaTrabalho = reader["NOME_JORNADA"].ToString(),
                        //HorasTrabalhadasTotal = reader[""].ToString(),
                    });
                }
                return lista;
            }
        }
    }
}
