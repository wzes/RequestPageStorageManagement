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
using static RequestPageStorageManagement.Show;

namespace RequestPageStorageManagement
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {
        bool State = true;   //最大化切换标志
        public MainWindow()
        {
            InitializeComponent();
            Option.getOption();   //设置初始化
        }
        
        //指令执行顺序
        private void orderComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((ComboBoxItem)orderComboBox.SelectedItem).Content.ToString().Equals("默认"))
            {
                Option.getOption().Instruction_order = 1;
            }
            else if(((ComboBoxItem)orderComboBox.SelectedItem).Content.ToString().Equals("随机"))
            {
                Option.getOption().Instruction_order = 2;
            }
            else
            {
                Option.getOption().Instruction_order = 3;
            }
        }
        //指令数
        private void numComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Option.getOption().Instruction_num = Convert.ToInt32(((ComboBoxItem)numComboBox.SelectedItem).Content.ToString());
        }
        //指令每页指令数
        private void instruction_pageComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Option.getOption().Instruction_page = Convert.ToInt32(((ComboBoxItem)instruction_pageComboBox.SelectedItem).Content.ToString());
        }
        //指令内存块数
        private void instruction_memeryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Option.getOption().Instruction_memery = Convert.ToInt32(((ComboBoxItem)instruction_memeryComboBox.SelectedItem).Content.ToString());
        }
        //执行方式
        private void excuteComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((ComboBoxItem)excuteComboBox.SelectedItem).Content.ToString().Equals("单步")){
                Option.getOption().excute_way = 2;
            }
            else
            {
                Option.getOption().excute_way = 1;
            }
          

        }
        //置换方式
        private void changeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((ComboBoxItem)changeComboBox.SelectedItem).Content.ToString().Equals("FIFO"))
            {
                Option.getOption().Change_way = 1;
            }
            else
            {
                Option.getOption().Change_way = 2;
            }


        }
        //最大化切换
        private void Max_Click(object sender, RoutedEventArgs e)
        {
            if (State)
            {
                this.WindowState = WindowState.Maximized;
                State = false;
            }
            else
            {
                this.WindowState = WindowState.Normal;
                State = true;
            }
            
        }
        //最小化切换
        private void Min_Click(object sender, RoutedEventArgs e)
        {

            this.WindowState = WindowState.Minimized;
        }
        //退出应用
        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
