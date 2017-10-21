using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;

namespace RequestPageStorageManagement
{
    /// <summary>
    /// Show.xaml 的交互逻辑
    /// </summary>
    public partial class Show : UserControl
    {
        ObservableCollection<Instruction>instructions;          //显示列表
        Queue<memery_node> myQueue;                             //FIFO内存块队列
        List<int> last_recent_pages;                            //RLU内存块访问队列
        int[] isPageInMemery;                                   //页内存标记
        ints_addr[] ins;                                        //指令地址
        int[] memerys;                                          //RLU内存块
        int lack_pages;                                         //缺页数
        int memery_number;                                      //内存块序号
        int total_insturction;                                  //总指令数
        int total_memery;                                       //内存块数
        int total_page;                                         //总页数
        int Step;                                               //步长
        public Show()
        {
            InitializeComponent();
        }
        //初始化设置
        public void Init()
        {
            total_insturction = Option.getOption().Instruction_num;     //获取配置
            total_memery = Option.getOption().Instruction_memery;       //获取配置
            total_page = Option.getOption().Instruction_page;           //获取配置
            memerys = new int[total_memery];                            //内存块号初始化
            myQueue = new Queue<memery_node>();                         //初始化内存块队列
            last_recent_pages = new List<int>();                        //最近最远内存块访问队列
            instructions = new ObservableCollection<Instruction>();     //初始化指令数组
            memery_number = 0;                                          //首内存块初始化
            ins = new ints_addr[total_insturction];                     //初始化指令数组
            isPageInMemery = new int[total_insturction / total_page];   //初始化页数组
            Init_instruction();                                         //指令顺序
            lack_pages = 0;                                             //缺页数初始化
            Step = 0;                                                   //步数初始化
            dataGrid.ItemsSource = instructions;                        //数据绑定
        }
        //选择指令执行顺序
        public void Init_instruction()
        {
            switch (Option.getOption().Instruction_order)
            {
                case 1:
                    Init_page_half_random();
                    break;
                case 2:
                    Init_page_all_random();
                    break;
                case 3:
                    Init_page_order();
                    break;
            }
        }
        //顺序生成：指令访问次序
        public void Init_page_order()
        {
            int index = 0;                                             //指令索引 
            while (index < total_insturction)
            {
                ints_addr ia = new ints_addr();                        //指令——地址
                ia.Addr = index;                                       //先选一个指令
                ia.Page = index / total_page;                          //计算页
                ins[index++] = ia;                                     //指令
               
            }
        }
        //根据设置选择置换方式
        public void ChangeWay(int size, int step)
        {
            if(Option.getOption().Change_way == 1)
            {
                FIFO(size, step);
            }
            else
            {
                LRU(size, step);
            }
        }
        //完全随机：指令访问次序
        public void Init_page_all_random()
        {
            int start;
            int index = 0; //指令索引 
            while (index < total_insturction)
            {
                start = getRandom(0, total_insturction);
                ints_addr ia = new ints_addr();  //指令——地址
                ia.Addr = start;     //先选一个指令
                ia.Page = start / total_page;  //页
                ins[index++] = ia;     //指令

            }
        }
        //半随机生成：指令访问次序 默认
        public void Init_page_half_random()
        {
            int start = 0;
            int index = 0; //指令索引
            int type = 1;  //类型
            while (index < total_insturction)
            {
                //初始选择
                if(type == 1)
                {
                    ints_addr ia = new ints_addr();                 //指令——地址
                    start = getRandom(0, total_insturction - 1);    //随机数
                    ia.Addr = start;                                //先选一个指令
                    ia.Page = start / total_page;                   //页
                    ins[index++] = ia;                              //指令
                    ints_addr ia_add = new ints_addr();             //新加的指令
                    ia_add.Addr = start + 1;                        //地址加1
                    ia_add.Page = ia_add.Addr / total_page;         //计算页地址
                    ins[index++] = ia_add;                          //指令
                    type = 2;                                       //类型二
                    continue;
                }
                //前
                if(type == 2)
                {
                    start = getRandom(0, start);                    //在0 到 start-1直接选一条指令
                    ints_addr ia = new ints_addr();
                    ia.Addr = start;
                    ia.Page = start / total_page;
                    ins[index++] = ia;
                    ints_addr ia_add = new ints_addr();
                    ia_add.Addr = start + 1;
                    ia_add.Page = ia_add.Addr / total_page;
                    ins[index++] = ia_add;
                    type = 3;                                       //跳转到类型三
                    continue;
                }
                //后
                if (type == 3)
                {
                    start = getRandom(start + 2, total_insturction - 1);  //选择start+2到最大中选择
                    ints_addr ia = new ints_addr();
                    ia.Addr = start;
                    ia.Page = start / total_page;
                    ins[index++] = ia;
                    ints_addr ia_add = new ints_addr();
                    ia_add.Addr = start + 1;
                    ia_add.Page = ia_add.Addr / total_page;
                    ins[index++] = ia_add;
                    type = 2;
                    continue;
                }
            }
        }
        //开始
        private void Start_click(object sender, RoutedEventArgs e)
        {
            //单步执行
            if (Option.getOption().excute_way == 2)
            {
                if (Step == 0)
                {
                    Init();
                }
                if(Step == total_insturction-1)
                {
                    Step = 0;  //复位
                    MainSnackbar.MessageQueue.Enqueue("                 缺页率   " + lack_pages *100 / (total_insturction - 1) + " %");
                }
                ChangeWay(total_memery, Step++);
                scrollView.ScrollToBottom();   //滚到底部
            }
            //自动执行
            else
            {
                Init();  //初始化
                for (int index = 0; index < total_insturction; index++)
                {
                    ChangeWay(total_memery, index);   //先进先出
                }   
                MainSnackbar.MessageQueue.Enqueue("                缺页率    " + lack_pages * 100 / (total_insturction - 1) + " %");
            }
        }
        //获取随机数
        public int getRandom(int left, int right)
        {
            if(left > right)       //边界检查
            {
                return right;
            }
            var seed = Guid.NewGuid().GetHashCode();   //种子
            Random random = new Random(seed);
            return random.Next(left, right);
        }
        //FIFO 
        void FIFO(int size, int step)
        {
            //页面在内存中  
            if (isPageInMemery[ins[step].Page] == 1)
            {
                int current_memery = 0;   //已存在内存块编号
                for(int i = 0; i < myQueue.Count; i++)   //取出已存在页的块内存
                {
                    if(myQueue.ElementAt(i).page == ins[step].Page)
                    {
                        current_memery = myQueue.ElementAt(i).memery;
                    }
                }
                Instruction instruction = new Instruction() { Number = step, Instruction_id = ins[step].Addr, Page = ins[step].Page, LackPage = "否", Memery = current_memery, OutPage = "---", InPage = "---" };
                if(instructions != null)
                {
                    instructions.Add(instruction);
                }
                
            }
            else
            {
                lack_pages++;  //缺页数增加
                //直接调入
                if (myQueue.Count < size)
                {
                    memery_node mn = new memery_node();
                    mn.memery = memery_number % total_memery + 1; //内存块
                    mn.page = ins[step].Page; //调入页
                    isPageInMemery[ins[step].Page] = 1;  //该页已调入
                    myQueue.Enqueue(mn);  //入队
                    Instruction instruction = new Instruction() { Number = step, Instruction_id = ins[step].Addr, Page = ins[step].Page, LackPage = "是", Memery = mn.memery, OutPage = "---", InPage = ins[step].Page+"" };
                    if (instructions != null)
                    {
                        instructions.Add(instruction);
                    }
                    memery_number++;   //内存块
                }
                //满页
                else if (myQueue.Count == size)
                {
                    memery_node mn = myQueue.Dequeue();
                    isPageInMemery[mn.page] = 0;  //移除
                    int remove = mn.page;    //移除页
                    mn.page = ins[step].Page;  //换入页
                    isPageInMemery[mn.page] = 1;  //移除
                    myQueue.Enqueue(mn);  //入队
                    Instruction instruction = new Instruction() { Number = step, Instruction_id = ins[step].Addr, Page = ins[step].Page, LackPage = "是", Memery = mn.memery, OutPage = remove+"", InPage = ins[step].Page+"" };
                    if (instructions != null)
                    {
                        instructions.Add(instruction);
                    }
                }
            }
        }
        //LRU
        void LRU(int size, int step)
        {
            if (isPageInMemery[ins[step].Page] == 1)
            {
                int i = 0;
                for (i = 0 ; i < total_memery; i++)
                {
                    if (memerys[i] == ins[step].Page)
                    {
                        break;
                    }
                }
                Instruction instruction = new Instruction() { Number = step, Instruction_id = ins[step].Addr, Page = ins[step].Page, LackPage = "否", Memery = i+1, OutPage = "---", InPage = "---" };
                if (instructions != null)
                {
                    instructions.Add(instruction);
                }
                last_recent_pages.Remove(ins[step].Page);
                last_recent_pages.Add(ins[step].Page);       //最后
            }
            else
            {
                lack_pages++;  //缺页数增加
                //直接调入
                if (last_recent_pages.Count < size)
                {
                    last_recent_pages.Add(ins[step].Page);   //最后
                    isPageInMemery[ins[step].Page] = 1;
                    int page = memery_number % total_memery;
                    memerys[page] = ins[step].Page;
                    Instruction instruction = new Instruction() { Number = step, Instruction_id = ins[step].Addr, Page = ins[step].Page, LackPage = "是", Memery = page + 1, OutPage = "---", InPage = ins[step].Page+"" };
                    if (instructions != null)
                    {
                        instructions.Add(instruction);
                    }
                    memery_number++;   //内存块
                }
                //满页
                else if (last_recent_pages.Count == size)
                {

                    int remove = last_recent_pages[0];
                    isPageInMemery[remove] = 0;  //移除

                    int i = 0;
                    for (i = 0; i < total_memery; i++)
                    {
                        if (memerys[i] == remove)
                        {
                            break;
                        }
                    }
                    memerys[i] = ins[step].Page;
                    int page = ins[step].Page;  //换入页
                    isPageInMemery[page] = 1;  //添加
                    last_recent_pages.Remove(remove);
                    last_recent_pages.Add(page);   //最后

                    Instruction instruction = new Instruction() { Number = step, Instruction_id = ins[step].Addr, Page = ins[step].Page, LackPage = "是", Memery = i+1, OutPage = remove + "", InPage = page+"" };
                    if (instructions != null)
                    {
                        instructions.Add(instruction);
                    }
                }
            }
           
        }
        //复位
        private void Restart_click(object sender, RoutedEventArgs e)
        {
            Init();  //初始化
        }
    }
    //地址对应页
    class ints_addr
    {
        int addr;
        int page;
        public int Page
        {
            get
            {
                return page;
            }

            set
            {
                page = value;
            }
        }
        public int Addr
        {
            get
            {
                return addr;
            }

            set
            {
                addr = value;
            }
        }
        private void Sample2_DialogHost_OnDialogClosing(object sender, DialogClosingEventArgs eventArgs)
        {
            Console.WriteLine("SAMPLE 2: Closing dialog with parameter: " + (eventArgs.Parameter ?? ""));
        }
    }
    //页对应内存块
    class memery_node
    {
        public int page;   //页
        public int memery;  //内存快编号
    }
    
}
