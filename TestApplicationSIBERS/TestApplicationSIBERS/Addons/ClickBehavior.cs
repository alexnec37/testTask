using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;

namespace TestApplicationSIBERS.Addons
{
    public class ClickBehavior
    {
        public static DependencyProperty DoubleClickProperty = DependencyProperty.RegisterAttached("DoubleClick",
                   typeof(ICommand),
                   typeof(ClickBehavior),
                   new FrameworkPropertyMetadata(null, new PropertyChangedCallback(ClickBehavior.DoubleClickChanged)));
        
        public static void SetDoubleClick(DependencyObject target, ICommand value)
        {
            target.SetValue(DoubleClickProperty, value);
        }

        public static ICommand GetDoubleClick(DependencyObject target)
        {
            return (ICommand)target.GetValue(DoubleClickProperty);
        }

        public static void ElementMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var element = (UIElement)sender;
            var command = (ICommand)element.GetValue(DoubleClickProperty);
            command.Execute(null);
        }

        private static void DoubleClickChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
        {
            var element = target as ListViewItem;
            if (element != null)
            {
                if ((e.NewValue != null) && (e.OldValue == null))
                {
                    element.MouseDoubleClick += ElementMouseDoubleClick;
                }
                else if ((e.NewValue == null) && (e.OldValue != null))
                {
                    element.MouseDoubleClick -= ElementMouseDoubleClick;
                }
            }
        }
    }
}

