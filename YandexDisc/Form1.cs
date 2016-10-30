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

namespace YandexDisc
{
    public partial class Form1 : Form
    {
        Disc disc;

        //String DirectoryCurrent;
        //string DirectoryPrev;
        List<String> Directory;

        public Form1()
        {
            InitializeComponent();
#error      disc = new Disc("");


            Directory = new List<String>();
            Directory.Add("/");
            UpdateShow();
            disc.UploadFileChangedEvent += UploadProgressChanged;
            disc.UploadFileCompleteEvent += UploadCompleted;
            disc.DownloadFileChangedEvent += DownloadProgressChanged;
            disc.DownloadFileCompleteEvent += DownloadCompleted;

        }

        void UploadProgressChanged(object sender, UploadProgressChangedEventArgs e)
        {
            //Console.WriteLine("Download {0}% complete. ", e.ProgressPercentage);
            tssl_up.Text = string.Format("Загрузка {0}% ", e.ProgressPercentage);

        }

        void UploadCompleted(object sender, UploadFileCompletedEventArgs e)
        {
            //Console.WriteLine("Download {0}% complete. ", e.ProgressPercentage);
            tssl_up.Text = "";
            MessageBox.Show("Загрузка файла завершена!");

        }

        void DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            //Console.WriteLine("Download {0}% complete. ", e.ProgressPercentage);
            tssl_down.Text = string.Format("Сохранение {0}% ", e.ProgressPercentage);

        }

        void DownloadCompleted(object sender, AsyncCompletedEventArgs e)
        {
            //Console.WriteLine("Download {0}% complete. ", e.ProgressPercentage);
            tssl_down.Text = "";
            MessageBox.Show("Сохранение файла завершено!");
        }

        private void обновитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UpdateShow();
        }
        private void UpdateShow()
        {
            TypeDisc infoDisc = disc.GetDiscInfo();
            int size_disc = (int)(infoDisc.total_space / 1000000);
            int size_used = (int)(infoDisc.used_space / 1000000);

            StatusLabel.Text = "Диск: " + size_used.ToString() + "/" + size_disc.ToString() + "МБ";

            listView1.Items.Clear();

            ListViewItem temp = new ListViewItem("..");
            temp.SubItems.Add("dir");
            listView1.Items.Add(temp);

            TypeResource resource = disc.GetResource(GetDirectoryCurrent());
            foreach (TypeResource item in resource._embedded.items)
            {
                ListViewItem items = new ListViewItem(item.name);
                items.SubItems.Add(item.type);
                int size = (int)(item.size * 0.001);
                items.SubItems.Add(size.ToString());
                items.SubItems.Add(item.created);
                listView1.Items.Add(items);

            }
        }

        String GetDirectoryCurrent()
        {
            String DirectoryCurrent = "";

            //if (Directory.Count > 1)
            //{
                foreach (String item in Directory)
                {
                    DirectoryCurrent += item;
                }
            //}
            return DirectoryCurrent;
        }
        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {

        }


        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            
            foreach (ListViewItem item in listView1.Items)
                if (item.Selected)
                {
                    if(item.Text == "..")
                    {
                        if (GetDirectoryCurrent() == "/")
                            return;

                        Directory.Remove(Directory.Last());
                        UpdateShow();
                        return;
                    }
                    if(item.SubItems[1].Text == "dir")
                    {
                        Directory.Add(item.SubItems[0].Text + "/");
                        UpdateShow();
                    }
                }
        }

        private void создатьПапкуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddDirectory form = new AddDirectory();
            form.ShowDialog();
            
            if(form.DialogResult == DialogResult.OK)
            {
                String directory = form.NameDirectory;
                if(directory.Length > 0)
                {
                    disc.CreateDirectory(GetDirectoryCurrent() + directory);
                }
                UpdateShow();
            }
            //disc.CreateDirectory("test");
        }

        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView1.Items)
                if (item.Selected)
                {
                    if (item.Text == "..")
                        return;

                    DialogResult dialogResult = MessageBox.Show(String.Format("Удалить {0} ?", item.SubItems[0].Text), "Удалить?", MessageBoxButtons.YesNo);

                    if(dialogResult == DialogResult.Yes)
                        disc.DeleteDirectory(GetDirectoryCurrent() + item.SubItems[0].Text);

                    UpdateShow();
                }
        }

        private void загрузитьФайлToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.InitialDirectory = "d:\\";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string path = openFileDialog1.FileName;
                string name = openFileDialog1.SafeFileName;
                disc.UploadFile(GetDirectoryCurrent() + name, path);
            }

            UpdateShow();
        }

        private void сохранитьФайлToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView1.Items)
                if (item.Selected)
                {
                    if (item.SubItems[1].Text == "file")
                    {
                        FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
                        folderBrowserDialog.Description = "Выбирете каталог:";
                        if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                        {
                            string InPath  = folderBrowserDialog.SelectedPath;
                            disc.DownloadFile(InPath + "\\" + item.SubItems[0].Text, GetDirectoryCurrent() + item.SubItems[0].Text);
                        }
                        UpdateShow();
                    }
                }
        }
    }
}
