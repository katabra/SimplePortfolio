using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PFWSWPFClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// 
    /// 간단히 리스트박스에 바인딩하여 웹소켓 쳇 처리
    /// 
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public ObservableCollection<string> chatList { get; set; } = new ObservableCollection<string>();

        public ObservableCollection<LogEntry> logList { get; set; } = new ObservableCollection<LogEntry>();

        public static MainWindow main;
        WSClient wsClient = new WSClient();
        ApiCall apiCall = new ApiCall();

        public MainWindow()
        {
            InitializeComponent();
            main = this;
            DataContext = this;
        }

        private void ConnectionButton_Click(object sender, RoutedEventArgs e)
        {
            _ = wsClient.Connect(ServerAddrTextBox.Text.Trim());
            AddNewLog($"Connected To {ServerAddrTextBox.Text.Trim()}");
        }

        private async void SendButton_Click(object sender, RoutedEventArgs e)
        {
            await wsClient.SendAsync(MsgTextBox.Text);
            AddNewLog($"Send '{MsgTextBox.Text}'");
            MsgTextBox.Text = string.Empty;
        }

        public void AddNewChatList(string msg)
        {
            Dispatcher.Invoke(() => {
                chatList.Add(msg);
                NotifyPropertyChanged("chatList");
                ChatListBox.ScrollIntoView(ChatListBox.Items[ChatListBox.Items.Count - 1]);
            });
            AddNewLog($"Msg '{msg}'");
        }

        public void ClearChatList()
        {
            Dispatcher.Invoke(() =>
            {
                chatList.Clear();
                NotifyPropertyChanged("chatList");
            });
        }

        public void AddNewLog(string str)
        {
            logList.Add(new LogEntry() { LogTime = DateTime.Now, LogText = str });
            NotifyPropertyChanged("logList");
        }

        private async void ApiGetButton_Click(object sender, RoutedEventArgs e)
        {
            await apiCall.CallGetApi(RestAddrTextBox.Text);
        }

        private async void ApiPostButton_Click(object sender, RoutedEventArgs e)
        {
            await apiCall.CallPostApi(RestAddrTextBox.Text);
        }
    }
}
