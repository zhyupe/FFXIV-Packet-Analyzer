using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PacketAnalyzer.Network;

namespace PacketAnalyzer.Test
{
    public partial class TestForm : Form
    {
        public TestForm()
        {
            InitializeComponent();
            mainControl.characterButton.Enabled = true;
        }
    }
}
