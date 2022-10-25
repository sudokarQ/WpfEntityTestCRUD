using System;
using System.Collections.Generic;
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

namespace WpfEntityTestCRUD
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {


            using (ApplicationContext db = new ApplicationContext())
            {
                // создаем два объекта User
                User tom = new User { Name = "Tom", Age = 33 };
                User alice = new User { Name = "Alice", Age = 26 };

                // добавляем их в бд
                db.Users.Add(tom);
                db.Users.Add(alice);
                db.SaveChanges();
                //Console.WriteLine("Объекты успешно сохранены");

                // получаем объекты из бд и выводим на консоль
                var users = db.Users.ToList();
                //Console.WriteLine("Список объектов:");
                //foreach (User u in users)
                //{
                //    Console.WriteLine($"{u.Id}.{u.Name} - {u.Age}");
                //}
            }
        }

        private void Canvas_Loaded(object sender, RoutedEventArgs e)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                
                var users = db.Users.ToList();
                dg.ItemsSource = users;
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                try
                {
                    db.Users.Add(new User { Age = Convert.ToInt32(TxtAge.Text), Name = TxtName.Text });
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Введите корренктные данные");
                }

                
                this.Canvas_Loaded(this, e); // !!!
                //dg.Items.Refresh();
            }
        }

        private void BtnRemove_Click(object sender, RoutedEventArgs e)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                
                if (dg.SelectedItem != null && dg.SelectedItems.Count == 1)
                {
                    //int index = db.Users.ToList().FindIndex(x => x == dg.SelectedItem as User);
                    int index = dg.SelectedIndex;
                    db.Users.Remove(dg.SelectedItem as User);
                    db.SaveChanges();

                    this.Canvas_Loaded(this, e); // !!!

                   
                        if (index != -1 && index < db.Users.Count())
                        {
                            dg.Focus();
                            dg.SelectedItem = dg.Items[index];
                        }
                        else dg.SelectedItem = null;
                   

                }
                else if (dg.SelectedItems.Count >= 2)
                {
                    db.Users.RemoveRange(dg.SelectedItems.Cast<User>());
                    db.SaveChanges();
                    this.Canvas_Loaded(this, e); // !!!

                }

            }
        }

        private void BtnChange_Click(object sender, RoutedEventArgs e)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                var record = dg.SelectedItem as User;
                record.Name = TxtName.Text;
                record.Age = Convert.ToInt32(TxtAge.Text);
                
                db.Users.Update(dg.SelectedItem as User);
                db.SaveChanges();
                this.Canvas_Loaded(this, e); // !!!

            }
        }
    }
}
