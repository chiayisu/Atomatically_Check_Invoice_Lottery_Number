using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReceiptLotteryCheck
{
    class Date
    {
        #region Variable
        private string _year;
        private string _month;
        #endregion

        #region Constructor
        public Date() { }
        #endregion

        #region Property
        public string Year
        {
            set
            {
                _year = value;
            }
            get
            {
                return _year;
            }
        }

        public string Month
        {
              set
            {
                _month = value;
            }
            get
            {
                if (_month.Count() == 1)
                {
                    _month = _month.Insert(0, "0");
                }
                return _month;
            }
        }
        #endregion
    }
}
