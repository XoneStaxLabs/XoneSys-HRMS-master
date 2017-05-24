
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;



namespace Xone.Models
{
    public class DependencyResolverService : IDependencyResolver
    {

        private IUnityContainer _Unitycontainer;
        public DependencyResolverService(IUnityContainer unityContainer)
        {
            _Unitycontainer = unityContainer;
        }
        public object GetService(Type serviceType)
        {
            try
            {
                return _Unitycontainer.Resolve(serviceType);
            }
            catch
            {
                return null;
            }
        }
        public IEnumerable<object> GetServices(Type serviceType)
        {
            try
            {
                return _Unitycontainer.ResolveAll(serviceType);
            }
            catch
            {
                return new List<object>();
            }
        }



    }
} 