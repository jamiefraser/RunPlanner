using MvvmCross.Core.ViewModels;
using MvvmCross.Platform.IoC;
using RunPlanner.Core.ViewModels;
namespace RunPlanner.iOS
{
    public class App : MvxApplication
    {
        public override void Initialize ()
        {
            CreatableTypes ()
                .EndingWith ("Service")
                .AsInterfaces ()
                .RegisterAsLazySingleton ();

            RegisterAppStart<FirstViewModel> ();
        }
    }
}