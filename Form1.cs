using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using HtmlAgilityPack;


namespace ReceiptLotteryCheck
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        #region Variable
        HttpWebResponse response;
        string html;
        HtmlAgilityPack.HtmlDocument document;
        List<string> name;
        List<string> value;
        List<string> prizeNumber;
        LotteryNumberJudgment lotteryNumber;
        Date date;
        #endregion

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox2.Clear();
            textBox3.Clear();
            List<string> userDataList = new List<string>();
            Dictionary<string, int> result = new Dictionary<string,int> ();
            date = new Date();
            date.Year = textBox4.Text;
            date.Month = textBox5.Text;
            if (IsDateNull(date))
            {
                MessageBox.Show("請輸入日期。", "注意", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!init(date))
            {
                MessageBox.Show("日期輸入錯誤，請確定輸入之日期已經開獎。", "注意", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string userData = textBox1.Text;
            userDataList = processUserData(userData);
            lotteryNumber = new LotteryNumberJudgment(userDataList);
            regularString(ref prizeNumber);
            result = lotteryNumber.decidePrizeType(prizeNumber);
            showPrizeNumber();
            showWonInvoice(result);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            date = new Date();
            date.Year = textBox4.Text;
            date.Month = textBox5.Text;
            if (IsDateNull(date))
            {
                MessageBox.Show("請輸入日期。", "注意", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (!init(date))
            {
                MessageBox.Show("日期輸入錯誤，請確定輸入之日期已經開獎。", "注意", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            showPrizeNumber();
        }


        bool IsDateNull(Date date)
        {
            if (date.Year == "" || date.Month == "")
                return true;
            return false;
        }

        bool init(Date date)
        {
            //https://www.etax.nat.gov.tw/etw-main/web/ETW183W2_10707/
            string urlAddress = "https://www.etax.nat.gov.tw/etw-main/web/ETW183W2_" + date.Year 
                + date.Month + "/";
            response = ReadWeb.Request(urlAddress);
            html = ReadWeb.getHTML(response, Encoding.UTF8);
            if (html != "")
            {
                var tableBody = htmlProcessing(html);
                var trNodeList = tableBody.ChildNodes.ToList();
                name = new List<string>();
                value = new List<string>();
                processTR(trNodeList);
                prizeNumber = new List<string>();
                prizeNumber = processPrizeNumber(value);
                return true;
            }
            return false;
        }

        HtmlNode htmlProcessing(string html)
        {
           
            document = HTMLParsing.LoadData(html);
            var element = HTMLParsing.getElement(document, "tablet01");
            var nodeList  = HTMLParsing.getNodeList(element);
            var attribute = HTMLParsing.getAttributeList(nodeList, 3);
            var ownerNode = HTMLParsing.getOwnerNode(attribute, 0);
            nodeList = ownerNode.ChildNodes.ToList();
            var tableBody = nodeList[3];
            return tableBody;
        }

        void processTR(List<HtmlNode> trNodeLIST)
        {
            foreach (HtmlNode tr in trNodeLIST)
            {
                if (tr.Name == "#text")
                {
                    continue;
                }
                var tableNodeList = tr.ChildNodes.ToList();
                if(tableNodeList.Count == 5)
                    saveValue(tableNodeList);
            }
        }

        void saveValue(List<HtmlNode> tableNodeList)
        {
            foreach (HtmlNode tableNode in tableNodeList)
            {
                if (tableNode.Name == "#text")
                {
                    continue;
                }
                else if (tableNode.Name == "th")
                {
                    name.Add(tableNode.InnerText);
                }
                else if (tableNode.Name == "td")
                {
                    value.Add(tableNode.InnerText);
                }
            }
        }

        List<string> processPrizeNumber(List<string> prizeValue)
        {
            List<string> temp = new List<string>();

            temp.Add(prizeValue[1]);
            temp.Add(prizeValue[2]);
            addToList(ref temp, processNumber(prizeValue[3]));
            addToList(ref temp, processNumber(prizeValue[9]));
            return temp;
        }

        void addToList(ref List<string> valueList, List<string> value)
        {
            foreach (string number in value)
            {
                valueList.Add(number);
            }
        }

        List<string> processNumber(string str)
        {
            List<string> processedNum = str.Split('、').ToList();
            return processedNum;
        }

        List<string> processUserData(string userData)
        {
            List<string> temp = new List<string>();
            temp = userData.Split(new char[] { ' ', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            return temp;
        }

        void regularString(ref string data)
        {
            data = data.Trim();
        }

        void regularString(ref List<string> dataList)
        {
            for (int i = 0; i < dataList.Count; i++)
            {
                dataList[i] = dataList[i].Trim();
            }
        }

        void showPrizeNumber()
        {
            string str = "";
            str = "特別獎: " + prizeNumber[0] + "\r\n";
            str += "特獎: " + prizeNumber[1] + "\r\n";
            str += "頭獎:\r\n";
            for (int i = 2; i < 5; i++)
                str += prizeNumber[i] + "\r\n";
            str += "增開六獎:\r\n";
            for (int i = 5; i < prizeNumber.Count; i++)
                str += prizeNumber[i] + "\r\n";
            textBox3.Text = str;
        }

        void showWonInvoice(Dictionary<string, int> result)
        {
            string str = "";
            foreach (var wonInvoice in result)
            {
                str += wonInvoice.Key + ":" + decidePrizeType(wonInvoice.Value) + "\r\n";
            }
            textBox2.Text = str;
        }

        string decidePrizeType(int code) 
        {
            if (code == PrizeTypeDefinition.specialPrize)
                return PrizeTypeReturnDefinition.specialPrize;
            else if (code == PrizeTypeDefinition.grandPrize)
                return PrizeTypeReturnDefinition.grandPrize;
            else if (code == PrizeTypeDefinition.firstPrize)
                return PrizeTypeReturnDefinition.firstPrize;
            else if (code == PrizeTypeDefinition.twoPrize)
                return PrizeTypeReturnDefinition.twoPrize;
            else if (code == PrizeTypeDefinition.threePrize)
                return PrizeTypeReturnDefinition.threePrize;
            else if (code == PrizeTypeDefinition.fourPrize)
                return PrizeTypeReturnDefinition.fourPrize;
            else if (code == PrizeTypeDefinition.fivePrize)
                return PrizeTypeReturnDefinition.fivePrize;
            else
                return PrizeTypeReturnDefinition.sixPrize;
        }

     
      
     
    }
}
