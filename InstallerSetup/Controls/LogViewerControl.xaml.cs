using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace InstallerSetup.Controls
{
    /// <summary>
    /// Interaction logic for LogViewerControl.xaml
    /// </summary>
    public partial class LogViewerControl : UserControl
    {
        public static readonly DependencyProperty LogLinesProperty =
            DependencyProperty.Register(name: nameof(LogLines),
                                        propertyType: typeof(ObservableCollection<ILogViewerLine>),
                                        ownerType: typeof(LogViewerControl),
                                        typeMetadata: new FrameworkPropertyMetadata(defaultValue: null, propertyChangedCallback: OnLogLinesChanged));

        public static readonly DependencyProperty AutoScrollProperty =
            DependencyProperty.Register(name: nameof(AutoScroll),
                                        propertyType: typeof(bool),
                                        ownerType: typeof(LogViewerControl),
                                        typeMetadata: new FrameworkPropertyMetadata(defaultValue: false));

        public static readonly DependencyProperty MaximumTextWidthProperty =
            DependencyProperty.Register(name: nameof(MaximumTextWidth),
                                        propertyType: typeof(double),
                                        ownerType: typeof(LogViewerControl),
                                        typeMetadata: new FrameworkPropertyMetadata(defaultValue: 400.0));

        public LogViewerControl()
        {
            this.InitializeComponent();

            // Listen to collection changes (items added or removed)
            INotifyCollectionChanged itemsCollection = this.LogViewerDataGrid.Items as INotifyCollectionChanged;
            itemsCollection.CollectionChanged += this.OnCollectionChanged;
        }

        public ObservableCollection<ILogViewerLine> LogLines
        {
            get { return (ObservableCollection<ILogViewerLine>)GetValue(LogLinesProperty); }
            set { SetValue(LogLinesProperty, value); }
        }

        public bool AutoScroll
        {
            get { return (bool)GetValue(AutoScrollProperty); }
            set { SetValue(AutoScrollProperty, value); }
        }

        public double MaximumTextWidth
        {
            get { return (double)GetValue(MaximumTextWidthProperty); }
            set { SetValue(MaximumTextWidthProperty, value); }
        }

        private static void OnLogLinesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is LogViewerControl logViewerControl)
            {
                int oldHash = (e.OldValue as INotifyCollectionChanged)?.GetHashCode() ?? 0;
                int newHash = (e.NewValue as INotifyCollectionChanged)?.GetHashCode() ?? 0;
                int currentHash = (logViewerControl.LogViewerDataGrid.Items as INotifyCollectionChanged)?.GetHashCode() ?? 0;
                Debug.WriteLine("------ OnLogMessagesChanged() ------- ");
                Debug.WriteLine($"oldHash = {oldHash}");
                Debug.WriteLine($"newHash = {newHash}");
                Debug.WriteLine($"currentHash = {currentHash}");
                Debug.WriteLine("");
            }
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // Auto-scroll (if enabled)
            if (this.AutoScroll)
            {
                if (this.LogViewerDataGrid.Items.Count == 0) return;
                this.LogViewerDataGrid.ScrollIntoView(this.LogViewerDataGrid.Items[this.LogViewerDataGrid.Items.Count - 1]);
            }
        }
    }

    /// <summary>
    /// Log lines are expected to implement this interface in order to be displayed on the log viewer
    /// </summary>
    public interface ILogViewerLine : INotifyPropertyChanged
    {
        DateTime Timestamp { get; }

        string Text { get; }

        LogViewerLineColor Color { get; }
    }

    public enum LogViewerLineColor
    {
        Normal,
        Green,
        Red,
    }

    /// <summary>
    /// This is just the most basic implementation of the ILogViewerLine interface
    /// This is used just for demo purpose. Try to implement the ILogViewerLine interface in your own classes for more functionality
    /// </summary>
    internal class BasicLogViewerLine : BindableBase, ILogViewerLine
    {
        private DateTime timestamp;
        private string text;
        private LogViewerLineColor color;

        public BasicLogViewerLine(DateTime timestamp, string text, LogViewerLineColor color)
        {
            this.Timestamp = timestamp;
            this.Text = text;
            this.color = color;
        }

        public BasicLogViewerLine(DateTime timestamp, string text) : this(timestamp, text, LogViewerLineColor.Normal)
        {
        }

        public BasicLogViewerLine(string text) : this(DateTime.Now, text)
        {
        }

        public DateTime Timestamp
        {
            get { return this.timestamp; }
            set { this.SetProperty(ref this.timestamp, value); }
        }

        public string Text
        {
            get { return this.text; }
            set { this.SetProperty(ref this.text, value); }
        }

        public LogViewerLineColor Color
        {
            get { return this.color; }
            set { this.SetProperty(ref this.color, value); }
        }
    }
}
