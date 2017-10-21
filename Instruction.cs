using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RequestPageStorageManagement
{
    //指令执行详细信息
    class Instruction : INotifyPropertyChanged
    {
        private int number;             //指令序号
        private int instruction_id;     //执行地址
        private int page;               //指令页
        private string lackPage;        //是否缺页
        private int memery;             //所在内存块
        private string outPage;            //换出页
        private string inPage;             //换入页

        public int Number
        {
            get
            {
                return number;
            }

            set
            {
                number = value;
                NotiFy("Number");
            }
        }

        public int Instruction_id
        {
            get
            {
                return instruction_id;
            }

            set
            {
                instruction_id = value;
                NotiFy("Instruction_id");
            }
        }

        public int Page
        {
            get
            {
                return page;
            }

            set
            {
                page = value;
                NotiFy("Page");
            }
        }

        public String LackPage
        {
            get
            {
                return lackPage;
            }

            set
            {
                lackPage = value;
                NotiFy("LackPage");
            }
        }

        public int Memery
        {
            get
            {
                return memery;
            }

            set
            {
                memery = value;
            }
        }

        public string OutPage
        {
            get
            {
                return outPage;
            }

            set
            {
                outPage = value;
                NotiFy("OutPage");
            }
        }

        public string InPage
        {
            get
            {
                return inPage;

            }

            set
            {
                inPage = value;
                NotiFy("InPage");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotiFy(string property)
        {
            PropertyChangedEventHandler PropertyChanged = this.PropertyChanged;
            if (PropertyChanged != null)
            {
                this.PropertyChanged.Invoke(this, new PropertyChangedEventArgs(property));
            }
        }
    }
}
