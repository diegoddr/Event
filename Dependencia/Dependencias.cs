using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Dominio.IRepositorios;
using Dominio.Servicos;
using Persistencia.Repositorios;

namespace Dependencia
{
    public static class Dependencias
    {
        private static WindsorContainer _container;
        private static bool _configurado = false;

        public static void Configurar()
        {
            if (_configurado)
            {
                return;
            }
            _container = new WindsorContainer();

            _container.Register(Component.For<UsuarioServicos>().ImplementedBy<UsuarioServicos>());
            _container.Register(Component.For<EventoServicos>().ImplementedBy<EventoServicos>());
            _container.Register(Component.For<ModuloServicos>().ImplementedBy<ModuloServicos>());

            _container.Register(Component.For<IUsuarioRepositorio>().ImplementedBy<UsuarioRepositorio>());
            _container.Register(Component.For<IEventoRepositorio>().ImplementedBy<EventoRepositorio>());
            _container.Register(Component.For<IModuloRepositorio>().ImplementedBy<ModuloRepositorio>());

        }

        public static T Resolver<T>()
        {
            return _container.Resolve<T>();
        }
    }
}
