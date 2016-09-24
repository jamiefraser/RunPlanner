using MvvmCross.Platform.IoC;
using System.Threading;
using System;
namespace RunPlanner.Core
{
    public class App : MvvmCross.Core.ViewModels.MvxApplication
    {
        public override void Initialize()
        {
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            RegisterAppStart<ViewModels.FirstViewModel>();
            Properties.Resources.Culture = System.Globalization.CultureInfo.CurrentUICulture;
        }
    }
}
