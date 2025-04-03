using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Connections;
using Domain.Entities;

namespace Application.Services
{
    public class UsuarioService //: IUsuarioService
    {
        //private readonly IUsuarioRepository _usuarioRepository;
        //private readonly DbSession _dbSession;

        //public UsuarioService(IUsuarioRepository usuarioRepository, DbSession dbSession)
        //{
        //    _usuarioRepository = usuarioRepository;
        //    _dbSession = dbSession;
        //}

        //public async Task CriarUsuarioAsync(UsuarioModel usuario)
        //{
        //    try
        //    {
        //        _dbSession.BeginTransaction();

        //        var usuarioCriado = await _usuarioRepository.InserirAsync(usuario);
        //        _dbSession.Commit();

        //        return await usuarioCriado;
        //    }
        //    catch
        //    {
        //        _dbSession.Rollback();
        //        throw;
        //    }
        //}
    }
}
