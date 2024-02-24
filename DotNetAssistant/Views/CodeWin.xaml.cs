using HandyControl.Interactivity;
using ICSharpCode.AvalonEdit;
using System;
using System.Windows;
using System.Windows.Controls;

namespace DotNetAssistant.Views
{
    /// <summary>
    /// Interaction logic for CodeWin
    /// </summary>
    public partial class CodeWin : UserControl
    {
        public CodeWin()
        {
            InitializeComponent();
        }

        private void textEditor_Loaded(object sender, RoutedEventArgs e)
        {
            textEditor.Text = " ";
        }
    }

    public class AvalonEditBehaviour : Behavior<TextEditor>
    {
        public static readonly DependencyProperty GiveMeTheTextProperty =
            DependencyProperty.Register("GiveMeTheText", typeof(string), typeof(AvalonEditBehaviour),
            new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, PropertyChangedCallback));

        public string GiveMeTheText
        {
            get { return (string)GetValue(GiveMeTheTextProperty); }
            set { SetValue(GiveMeTheTextProperty, value); }
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            if (AssociatedObject != null)
                AssociatedObject.TextChanged += AssociatedObjectOnTextChanged;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            if (AssociatedObject != null)
                AssociatedObject.TextChanged -= AssociatedObjectOnTextChanged;
        }

        private void AssociatedObjectOnTextChanged(object sender, EventArgs eventArgs)
        {
            var textEditor = sender as TextEditor;
            if (textEditor != null)
            {
                if (textEditor.Document != null)
                    GiveMeTheText = textEditor.Document.Text;
            }
        }

        private static void PropertyChangedCallback(
            DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var behavior = dependencyObject as AvalonEditBehaviour;
            if (behavior.AssociatedObject != null)
            {
                var editor = behavior.AssociatedObject as TextEditor;
                if (editor.Document != null)
                {
                    try
                    {
                        //这里保存鼠标位置
                        var caretOffset = editor.CaretOffset;
                        editor.Document.Text = dependencyPropertyChangedEventArgs.NewValue.ToString();
                        //还原鼠标位置
                        editor.CaretOffset = caretOffset;
                    }
                    catch { editor.Document.Text = dependencyPropertyChangedEventArgs.NewValue.ToString(); }
                }
            }
        }
    }
}