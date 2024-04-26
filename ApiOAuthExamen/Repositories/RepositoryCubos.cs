using ApiOAuthExamen.Data;
using ApiOAuthExamen.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiOAuthExamen.Repositories
{
    public class RepositoryCubos
    {
        private CubosContext context;

        public RepositoryCubos(CubosContext context)
        {
            this.context = context;
        }

        public async Task<UsuarioCubo>
            LoginUsuarioAsync(string email, string pass)
        {
            return await this.context.Usuarios
                .Where(x => x.Email == email
                && x.Pass == pass)
                .FirstOrDefaultAsync();
        }

        //metodos publicos

        //lista cubos
        public async Task<List<Cubo>> GetCubosAsync()
        {
            return await this.context.Cubos.ToListAsync();
        }

        //filtrar por marca
        public async Task<List<Cubo>> GetCubosMarcaAsync
            (string marca)
        {
            var consulta = from datos in this.context.Cubos
                           where datos.Marca == marca
                           select datos;
            return await consulta.ToListAsync(); ;
        }

        //crear usuario
        public async Task InsertUsuario
            (string nombre, string email, string passw, string imagen)
        {
            var maxIdUsuario = (from datos in this.context.Usuarios
                               select datos.IdUsuario).Max();

            int idusuario = maxIdUsuario + 1;

            UsuarioCubo usuario = new UsuarioCubo();
            usuario.IdUsuario = idusuario;
            usuario.Nombre = nombre;
            usuario.Email = email;
            usuario.Pass = passw;
            usuario.Imagen = imagen;

            this.context.Usuarios.Add(usuario);
            await this.context.SaveChangesAsync();
        }

        //metodos privados

        
        //find usuario
        public async Task<UsuarioCubo> FindUsuarioAsync(int id)
        {
            return await
                this.context.Usuarios
                .FirstOrDefaultAsync(x => x.IdUsuario == id);
        }

        //pedidos user
        public async Task<List<CompraCubo>> GetPedidosUsuarioAsync(int id)
        {
            var consulta = from datos in this.context.Pedidos
                           where datos.IdUsuario == id
                           select datos;

            return await consulta.ToListAsync();
        }

        //insert pedido
        public async Task InsertarPedido(int idcubo, int idusuario)
        {
            var maxIdPedido = (from datos in this.context.Pedidos
                               select datos.IdPedido).Max();

            int idpedido = maxIdPedido + 1;
            DateTime fecha = DateTime.UtcNow;

            CompraCubo pedido = new CompraCubo();
            pedido.IdPedido = idpedido;
            pedido.IdCubo = idcubo;
            pedido.IdUsuario = idusuario;
            pedido.Fecha = fecha;

            this.context.Pedidos.Add(pedido);
            await this.context.SaveChangesAsync();
        }



        ////extras
        //public async Task<List<CompraCubo>> GetPedidosAsync()
        //{
        //    return await this.context.Pedidos.ToListAsync();
        //}

        //public async Task<List<UsuarioCubo>> GetUsuariosAsync()
        //{
        //    return await this.context.Usuarios.ToListAsync();
        //}
    }
}
