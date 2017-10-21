using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RequestPageStorageManagement
{     
    //设置
    class Option
    {
        public static Option option = null;
        
        public static Option getOption()
        {
            if(option == null)
            {
                option = new Option();
            }
            return option;
        }
        public Option()
        {
            instruction_order = 1;
            change_way = 1;
            excute_way = 1;
            instruction_num = 320;
            instruction_page = 10;
            instruction_memery = 4;
        }
        private int instruction_order;
        private int change_way;
        public int excute_way;
        private int instruction_num;
        private int instruction_page;
        private int instruction_memery;

        public int Instruction_order
        {
            get
            {
                return instruction_order;
            }

            set
            {
                instruction_order = value;
            }
        }

        public int Change_way
        {
            get
            {
                return change_way;
            }

            set
            {
                change_way = value;
            }
        }

        public int Instruction_num
        {
            get
            {
                return instruction_num;
            }

            set
            {
                instruction_num = value;
            }
        }

        public int Instruction_page
        {
            get
            {
                return instruction_page;
            }

            set
            {
                instruction_page = value;
            }
        }

        public int Instruction_memery
        {
            get
            {
                return instruction_memery;
            }

            set
            {
                instruction_memery = value;
            }
        }
    }
}
