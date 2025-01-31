using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using ConnectionCore;

namespace DiscordServerSimUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        


        //notifies the UI when changes occur , so it will be updatable
        ObservableCollection<Message> Messages { get; set; } = new();

        public MainWindow()
        {
            InitializeComponent();

            //link the Message List component with the message collection
            MessageList.ItemsSource = Messages;
        }

        private void MessageBox_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {

                SendMessage();
                e.Handled = true;

            }
        }

        private void SendingButton_Click(object sender, RoutedEventArgs e)
        {
            SendMessage();
        }

        private void SendMessage()
        {

            if (!string.IsNullOrWhiteSpace(MessageBox.Text))
            {

             Messages.Add(new Message { messageText = MessageBox.Text, Sender = new User("Akram") });

            

             MessageBox.Clear();



             ScrollToBottom();


            }


        }

        private void ScrollToBottom()
        {
            if(Messages.Count > 0)
            {

                MessageList.UpdateLayout();

                MessageList.ScrollIntoView(Messages.Last());


            }





        }




    }

    class User
    {
        public string Username { get; set; } // get and set so it will be bindable

        public User(string username)
        {


            this.Username = username;



        }


    }
    class Message
    {
        public User Sender { get; set; } // get and set so it will be bindable
        public string messageText { get; set; }// get and set so it will be bindable

    }
}