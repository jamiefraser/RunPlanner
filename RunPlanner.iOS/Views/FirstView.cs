using System;
using MvvmCross.iOS.Views;
using MvvmCross.Binding.iOS;
using MvvmCross.Binding.iOS.Views;
using MvvmCross.Binding;
using RunPlanner.Core.ViewModels;
using UIKit;
using MvvmCross.Binding.BindingContext;
using Cirrious.FluentLayouts.Touch;
namespace RunPlanner.iOS
{
    public class FirstView : MvxViewController
    {
        private UILabel lblHello;
        private FirstViewModel vm;

        public FirstView ()
        {
            lblHello = new UILabel ();
            lblHello.TextAlignment = UITextAlignment.Center;
        }
        public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();
            this.View.BackgroundColor = UIColor.White;
            vm = this.ViewModel as FirstViewModel;
            View.Add (lblHello);
            var set = this.CreateBindingSet<FirstView, FirstViewModel> ();
            set.Bind (lblHello).To (viewmodel => viewmodel.Hello);
            set.Apply ();
            SetConstraints ();
        }
        private void SetConstraints ()
        {
            this.View.SubviewsDoNotTranslateAutoresizingMaskIntoConstraints ();
            View.AddConstraints (lblHello.WithSameCenterX (this.View)
                                , lblHello.WithSameCenterY (this.View)
                                , lblHello.WithSameWidth (this.View)
                                );
        }
    }
}
