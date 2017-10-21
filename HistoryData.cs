using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RequestPageStorageManagement
{
    class HistoryData
    {
        private DateTime dataTime;
        private int way;
        private int pages;
        private int lackPages;
        private double rate;

        public HistoryData(DateTime dataTime,int way, int pages, int lackPages, double rate )
        {
            this.dataTime = dataTime;
            this.way = way;
            this.pages = pages;
            this.lackPages = lackPages;
            this.rate = rate;
        }

        public DateTime DataTime
        {
            get
            {
                return dataTime;
            }

            set
            {
                dataTime = value;
            }
        }

        public int Way
        {
            get
            {
                return way;
            }

            set
            {
                way = value;
            }
        }

        public int Pages
        {
            get
            {
                return pages;
            }

            set
            {
                pages = value;
            }
        }

        public int LackPages
        {
            get
            {
                return lackPages;
            }

            set
            {
                lackPages = value;
            }
        }

        public double Rate
        {
            get
            {
                return rate;
            }

            set
            {
                rate = value;
            }
        }
    }
}
