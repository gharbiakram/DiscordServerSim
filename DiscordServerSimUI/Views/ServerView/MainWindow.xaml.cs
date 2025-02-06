using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using ConnectionCore;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace DiscordServerSimUI.Views.ServerView
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        ClientCore client ;

        User user ;

        string username;
        //notifies the UI when changes occur , so it will be updatable
        ObservableCollection<Message> Messages { get; set; } = new();
        public MainWindow()
        {
            username  = Microsoft.VisualBasic.Interaction.InputBox("Enter your username:", "Username", username);
            user = new(username);

            client = new ClientCore(username);



            InitializeComponent();
            //link the Message List component with the message collection
            MessageList.ItemsSource = Messages;

            RecieveMessageAsync();

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

        private async void SendMessage()
        {

            string sentMessage = MessageBox.Text;

            if (!string.IsNullOrWhiteSpace(sentMessage))
            {

                //client sends message here

                //Make a way so that the userID will also be included (for DB) purpose

                await client.SendMessageAsync(sentMessage).ContinueWith(task =>
                {
                    if (task.IsCompletedSuccessfully)
                    {
                        // Ensure UI updates are on the main thread (if applicable)
                        App.Current.Dispatcher.Invoke(() =>
                        {
                            Messages.Add(new Message { Sender = user, messageText = sentMessage });
                        });
                    }
                });




                // Messages.Add(new Message { messageText = MessageBox.Text, Sender = new User("Akram") });















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


        private async void RecieveMessageAsync() {

            while (true)
            {

                var recievedMessage = await client.RecieveResponseAsync();

                var splitMessage = recievedMessage.Split(": ", 2);

                string senderName = splitMessage.Length > 1 ? splitMessage[0] : "Unknown";
                string messageText = splitMessage.Length > 1 ? splitMessage[1] : recievedMessage;


                Messages.Add(new Message { Sender = new User(senderName) , messageText = messageText });
  
            
            
            
            }
        
                
        
        
        
        
        
        }

        private void UpdateUI()
        {


            // upon recieving the message , change the UI (add it to the message list)



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