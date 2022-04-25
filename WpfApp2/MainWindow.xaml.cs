using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public List<AgentViewModel> agentViews;
        public MainWindow()
        {
            InitializeComponent();
           this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
           List<string> listSorting = new List<string>() { "Сортировка" , "Без сортировки"  , "по цене (up)" , "по цене (down)" };
           List<string> listFilter = new List<string>() { "Фильтрация" , "Без фильтрации" ,  }; // это потом поменяем
           cbFilter.ItemsSource = listFilter;
           cbSorting.ItemsSource = listSorting;
           cbFilter.SelectedIndex = 0;
           cbSorting.SelectedIndex = 0;

            try
            {
                DB.MyContext myContext = new DB.MyContext();
                agentViews = myContext.AgentViewModels.ToList();
                lbContent.ItemsSource = agentViews; 
                Loadnavigation(agentViews.Count , 15);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
          
        }

        private void Loadnavigation(int count, int step)
        {
            int countLabel = count / step; // сколько  элементов  надо 

            if(countLabel % step > 0)
            {
                countLabel++; // ищим хвост 
            }

            var labelStart = new Button() // первый  элемент
            {
                Content = "<--",
                Margin = new Thickness(5) , Padding = new Thickness(5)
            };

            labelStart.Click += LabelEnd_Click; // подписка на  событие 

            var labelEnd = new Button() // последний  элемент 
            {
                Content = "-->",
                Margin = new Thickness(5) ,
                Padding = new Thickness(5)
            };

            labelEnd.Click += LabelEnd_Click;

            staclNavigation.Children.Add(labelStart );  // добавилт  в стек первый элемент 

            for (int i = 0; i < countLabel; i++)
            {
                var l = new Button() { Content = $"{i + 1}", Margin = new Thickness(5) , Padding = new Thickness(5) };
                l.Click += LabelEnd_Click;

                staclNavigation.Children.Add(l);  // добавиом  в стек 
            }
            staclNavigation.Children.Add(labelEnd);  // добавиом  в стек  послдним
        }

        private void LabelEnd_Click(object sender, RoutedEventArgs e)
        {
            var b = sender as Button; // получили объект 

            if(b.Content.ToString() == "-->")
            {
                MessageBox.Show("вы хотели  перейти  на след страницу, но  пока  что  эта функция не  доступна (");
                return;
            }

            if (b.Content.ToString() == "<--")
            {
                MessageBox.Show("вы хотели  перейти  на предыдущую страницу, но  пока  что  эта функция не  доступна (");
                return;
            }

            MessageBox.Show("вы хотели  перейти на  стр "  +  b.Content.ToString() + " но  пока  что  эта функция не  доступна (");
        }

        
    }
};
