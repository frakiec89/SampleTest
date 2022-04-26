using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfApp2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public List<AgentViewModel> agentViews;
        private int page=0;
        private int pageSize = 0;

        public MainWindow()
        {
            InitializeComponent();
           this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
           List<string> listSorting = new List<string>() { "Сортировка" , "Без сортировки"  , "по продажам(up)" , "по продажам (down)" ,
            "по рейтингу (up)" , "по рейтингу (down)" , "по типу агентов  (up)" , "по типу агентов(down)" , "по скидке (up)" , "по скидке(down)"

           };

           var listSirt = new List<string>() { "Фильтрация", "Без фильтрации", };

           List<string> listFilter = listSirt; // это потом поменяем

           cbFilter.ItemsSource = listFilter;
           cbSorting.ItemsSource = listSorting;
           cbFilter.SelectedIndex = 0;
           cbSorting.SelectedIndex = 0;
           cbSorting.SelectionChanged += CbSorting_SelectionChanged;
           cbFilter.SelectionChanged += CbFilter_SelectionChanged;
           tbSearch.GotMouseCapture += TbSearch_GotMouseCapture;
            tbSearch.GotFocus += TbSearch_GotFocus;


            tbSearch.TextChanged += TbSearch_TextChanged;
            try
            {
                DB.MyContext myContext = new DB.MyContext();
                agentViews = myContext.AgentViewModels.ToList();

                var agentType = agentViews.Select(x => x.TypeName).Distinct().ToList();
                listFilter.AddRange(agentType);
                cbFilter.ItemsSource = listFilter;
                lbContent.ItemsSource = agentViews; 
                Loadnavigation(agentViews.Count , 15);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            LabelEnd_Click(new Button { Content = "1" }, e);


        }

        private void TbSearch_GotFocus(object sender, RoutedEventArgs e)
        {
            tbSearch.Clear();
        }

        private void TbSearch_GotMouseCapture(object sender, System.Windows.Input.MouseEventArgs e)
        {
            tbSearch.Clear();
        }

        private void TbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
          if(tbSearch.Text ==String.Empty)
          {
                lbContent.ItemsSource = agentViews;
                Loadnavigation(agentViews.ToList().Count, 15); return; 
          }
          
          var s = tbSearch.Text.TrimStart().TrimEnd().ToLower();
            var al = agentViews.Where(x =>
           (x.NameCompany.ToLower().StartsWith(s)) ||
           (x.NameCompany.ToLower().EndsWith(s)) ||
           (x.Telefone.ToLower().StartsWith(s)) ||
           ( x.Telefone.ToLower().EndsWith(s)) ||
           (x.TypeName.ToLower().StartsWith(s)) ||
           (x.TypeName.ToLower().EndsWith(s))
           );
            lbContent.ItemsSource = al.ToList();

            Loadnavigation(al.ToList().Count, 15);
        }

        private void CbFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbFilter.SelectedIndex <= 1)
            {
                lbContent.ItemsSource = agentViews;
                Loadnavigation(agentViews.Count, 15); return;
            }

            var al = agentViews.Where(x => x.TypeName==cbFilter.SelectedValue.ToString()).ToList();
            lbContent.ItemsSource = al;
            Loadnavigation(1, 15); return;


        }

        private void CbSorting_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbSorting.SelectedIndex <= 1)
            {
                lbContent.ItemsSource = agentViews;
                Loadnavigation(agentViews.ToList().Count, 15); return;
            }

            if (cbSorting.SelectedIndex == 2)
            {
                var al = agentViews.OrderBy(x => x.Sale).ToList();
                lbContent.ItemsSource = al;
                Loadnavigation(al.ToList().Count, 15); return;
            }

            if (cbSorting.SelectedIndex == 3)
            {
                var al = agentViews.OrderByDescending(x => x.Sale).ToList();
                lbContent.ItemsSource = al;
                Loadnavigation(al.ToList().Count, 15); return;
            }

            if (cbSorting.SelectedIndex == 4)
            {
                var al = agentViews.OrderBy(x => x.Priority).ToList();
                lbContent.ItemsSource = al;
                Loadnavigation(al.ToList().Count, 15); return;
            }

            if (cbSorting.SelectedIndex == 5)
            {
                var al = agentViews.OrderByDescending(x => x.Priority).ToList();
                lbContent.ItemsSource = al;
                Loadnavigation(al.ToList().Count, 15); return;
            }


            if (cbSorting.SelectedIndex == 6)
            {
                var al = agentViews.OrderBy(x => x.TypeName).ToList();
                lbContent.ItemsSource = al;
                Loadnavigation(al.ToList().Count, 15); return;
            }

            if (cbSorting.SelectedIndex == 7)
            {
                var al = agentViews.OrderByDescending(x => x.TypeName).ToList();
                lbContent.ItemsSource = al;
                Loadnavigation(al.ToList().Count, 15); return;
            }


            if (cbSorting.SelectedIndex == 8)
            {
                var al = agentViews.OrderBy(x => x.Discount).ToList();
                lbContent.ItemsSource = al;
                Loadnavigation(al.ToList().Count, 15); return;
            }

            if (cbSorting.SelectedIndex == 9)
            {
                var al = agentViews.OrderByDescending(x => x.Discount).ToList();
                lbContent.ItemsSource = al;
                Loadnavigation(al.ToList().Count, 15); return;
            }


        }

        private void Loadnavigation(int count, int step)
        {
            staclNavigation.Children.Clear();

            int countLabel = count / step; // сколько  элементов  надо 
            pageSize = countLabel;
            if (countLabel % step > 0)
            {
                countLabel++; // ищим хвост 
            }
            pageSize = countLabel;

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
                if(page>=pageSize)
                {
                    return; 
                }
                 
                   page++;
                   LabelEnd_Click(new Button { Content = page.ToString() }, e);
                   return;
                
            }

            if (b.Content.ToString() == "<--" || b.Content.ToString()=="0")
            {
                if (page > 0)
                {
                    page--;
                    LabelEnd_Click(new Button { Content = page.ToString() }, e);
                    return;
                }
                return;
            }

            foreach (var item in staclNavigation.Children)
            {
                var but = item as Button;
                but.Background = Brushes.Gray;
            }


            int start = Convert.ToInt32(b.Content.ToString());
            
            page = start;


            
                var ag = agentViews.Skip((start - 1) * 15).Take(15).ToList();
                lbContent.ItemsSource = ag;
            

            foreach (var item in staclNavigation.Children)
            {
                b = item as Button;
                if (b.Content.ToString() == page.ToString())
                    b.Background = Brushes.Red ;
            }

        }

        
    }
};
