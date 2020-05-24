using TelleR.Logic.Repositories.Impl;
using TelleR.Unity;
using Unity;
using Unity.Lifetime;

namespace TelleR.Logic.Repositories
{
    class UnitySetup : IUnityContainerSetup
    {
        public void SetupContainer(IUnityContainer container)
        {
            container.RegisterType(typeof(IUserRepository), typeof(UserRepositoryImpl), new PerResolveLifetimeManager());
            container.RegisterType(typeof(IBlogRepository), typeof(BlogRepositoryImpl), new PerResolveLifetimeManager());
            container.RegisterType(typeof(IPostRepository), typeof(PostRepositoryImpl), new PerResolveLifetimeManager());
            container.RegisterType(typeof(IAuthorInviteRepository), typeof(AuthorInviteRepositoryImpl), new PerResolveLifetimeManager());
        }
    }
}
