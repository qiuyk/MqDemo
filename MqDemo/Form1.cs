using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommonUtils;
namespace MqDemo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnPublish_Click(object sender, EventArgs e)
        {
            MqHelper.Publish("测试消息", "aaa", "bbb");
        }

        private void btnSubscribe_Click(object sender, EventArgs e)
        {
            MqHelper.Subscribe("aaa");
        }

    }
}
