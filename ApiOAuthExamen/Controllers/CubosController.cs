using ApiOAuthExamen.Helpers;
using ApiOAuthExamen.Models;
using ApiOAuthExamen.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiOAuthExamen.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CubosController : ControllerBase
    {
        private RepositoryCubos repo;

        public CubosController(RepositoryCubos repo)
        {
            this.repo = repo;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<List<Cubo>>> GetCubos()
        {
            return await this.repo.GetCubosAsync();
        }

        [HttpGet]
        [Route("[action]/{marca}")]
        public async Task<ActionResult<List<Cubo>>> GetCubosMarca(string marca)
        {
            return await this.repo.GetCubosMarcaAsync(marca);
        }

        [HttpPost]
        [Route("[action]")]
        public async Task InsertarUsuario(UsuarioCubo user)
        {
            await this.repo.InsertUsuario(user.Nombre, user.Email
                , user.Pass, user.Imagen);
        }

        //metodos protegidos

        [HttpGet]
        [Authorize]
        [Route("[action]/{id}")]
        public async Task<ActionResult<UsuarioCubo>> FindUsuario(int id)
        {
            return await this.repo.FindUsuarioAsync(id);
        }

        [HttpGet]
        [Authorize]
        [Route("[action]/{id}")]
        public async Task<ActionResult<List<CompraCubo>>> PedidosUsuario(int id)
        {
            return await this.repo.GetPedidosUsuarioAsync(id);
        }

        [HttpPost]
        [Route("[action]")]
        public async Task InsertarPedido(CompraCubo pedido)
        {
            await this.repo.InsertarPedido(pedido.IdCubo, pedido.IdUsuario);
        }



    }
}
