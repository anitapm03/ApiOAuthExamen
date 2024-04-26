using ApiOAuthExamen.Data;
using ApiOAuthExamen.Models;
using ApiOAuthExamen.Services;
using Microsoft.EntityFrameworkCore;

namespace ApiOAuthExamen.Repositories
{
    public class RepositoryCubos
    {
        private CubosContext context;
        private ServiceStorageBlobs serviceBlobs;

        public RepositoryCubos(CubosContext context,
            ServiceStorageBlobs serviceBlobs)
        {
            this.context = context;
            this.serviceBlobs = serviceBlobs;
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

        //lista cubos
        public async Task<List<Cubo>> GetCubosBlobAsync()
        {
            List<Cubo> cubos = await this.context.Cubos.ToListAsync();
            //List<Cubo> cubosBlob = new List<Cubo>();
            foreach (Cubo c in cubos)
            {
                string urlBlob = this.serviceBlobs.GetContainerUrl("examenana");
                c.Imagen = urlBlob + "/" + c.Imagen;
                
            }

            return cubos;
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

        //find usuario con blob
        public async Task<UsuarioCubo> FindUsuarioBlobAsync(int id)
        {
            UsuarioCubo user = await
                this.context.Usuarios
                .FirstOrDefaultAsync(x => x.IdUsuario == id);
            string urlBlob = this.serviceBlobs.GetContainerUrl("examenana");

            UsuarioCubo userblob = new UsuarioCubo();
            userblob.IdUsuario = user.IdUsuario;
            userblob.Nombre = user.Nombre;
            userblob.Email = user.Email;
            userblob.Pass = user.Pass;
            userblob.Imagen = urlBlob + "/" + user.Imagen;

            return userblob;
            /*return await
                this.context.Usuarios
                .FirstOrDefaultAsync(x => x.IdUsuario == id);*/
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
