


using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using Xone.Models;
using RepositoryImplement;
using RepositoryImplement.Xone.RepositoryDerive;
using RepositoryImplement.Xone.RepositoryImplement;

namespace Xone
{
    public class IOCConfiguration
    {
        public static void UnityConfiguratorContainer()
        {
            IUnityContainer container = new UnityContainer();
            RegisterServices(container);
            DependencyResolver.SetResolver(new DependencyResolverService(container));
        }

        public static void RegisterServices(IUnityContainer container)
        {            
            container.RegisterType<ICitizenMaster, CitizenMasterRepository>();
            container.RegisterType<IRaceMaster, RaceMasterRepository>();
            container.RegisterType<ILanguageMaster, LanguageMasterRepository>();
            container.RegisterType<ISkillsetMaster, SkillsetMasterRepository>();
            container.RegisterType<IDocumentTypeMaster, DocumentTypeMasterRepository>();
            container.RegisterType<IDocSubTypeMaster, DocSubTypeRepository>();
            container.RegisterType<IDepartmentMaster, DepartmentMasterRepository>();
            container.RegisterType<IHolidayMaster, HolidayMasterRepository>();
            container.RegisterType<IGradeMaster, GradeMasterRepository>();
            container.RegisterType<IDeductionTypeMaster, DeductionTypeMasterRepository>();
            container.RegisterType<ICheckListMaster, CheckListMasterRepository>();

            container.RegisterType<IDesigDocTypeMaster, DesigDocTypeMasterRepository>();


        }
    }
} 