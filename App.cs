using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Events;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using System.Xml.Linq;
using DockablePaneProviderData = Autodesk.Revit.UI.DockablePaneProviderData;
using KeyEventArgs = System.Windows.Forms.KeyEventArgs;

namespace LinkElement
{
    class App : IExternalApplication, IExternalCommandAvailability
    {
       public  static ExternalEvent _event = null;
        static Thread _thread = null;
        static int _timeout_minutes = 90;
       public  static string errorLog = "";
        static int _timeout = 1000 * 60 * _timeout_minutes;
        

        private Autodesk.Revit.DB.Document document;


        #region Create Own Ribbon Tab and Add-in button

        public void AddRibbonPanel(UIControlledApplication application)
        {
            try
            {
                Autodesk.Windows.RibbonControl ribbon = Autodesk.Windows.ComponentManager.Ribbon;
                bool isExistTab = false;
                foreach (Autodesk.Windows.RibbonTab tab in ribbon.Tabs)
                {
                    if (tab.Id == "SketsStudio")
                    {
                        isExistTab = true;
                    }
                }
                String tabName = "SketsStudio";
                if (isExistTab == false)
                {
                    application.CreateRibbonTab(tabName);
                }
                Autodesk.Revit.UI.RibbonPanel ribbonPanel = application.CreateRibbonPanel(tabName, "LockElement");

                string thisAssemblyPath = Assembly.GetExecutingAssembly().Location;

                PushButtonData pushdataButton = new PushButtonData("LockElement", "LockElement", thisAssemblyPath, "LinkElement.Command");

                pushdataButton.LargeImage = GetImage(Resource1.lockelement.GetHbitmap());

                pushdataButton.ToolTip = "LockElement";

                Autodesk.Revit.UI.RibbonItem r = ribbonPanel.AddItem(pushdataButton);                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error No 1 : " + ex.ToString(), "SketsStudio Standard Checker", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        #endregion
        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        #region on revit start event call function of add our own ribbon tab
        public Result OnStartup(UIControlledApplication application)
        {
            AddRibbonPanel(application);
            return Result.Succeeded;
        }
        #endregion

        private void KeyPressed(Keys keyPressed)

        {

            if (keyPressed == Keys.Escape)

            {

                MessageBox.Show("Key was pressed", "Escape");

            }

        }
        public System.Windows.Media.Imaging.BitmapSource GetImage(IntPtr bmg)
        {
            System.Windows.Media.Imaging.BitmapSource bms = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(bmg, IntPtr.Zero, System.Windows.Int32Rect.Empty, System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
            return bms;
        }

        public bool IsCommandAvailable(UIApplication applicationData, CategorySet selectedCategories)
        {
            // Return false to prevent the Escape key from canceling your command
            return false;
        }
    }

    #region Execute method and show User UI Form
    [Autodesk.Revit.Attributes.Transaction(Autodesk.Revit.Attributes.TransactionMode.Manual)]

    public class Command : IExternalCommand
    {
        private bool checkboxState;
        private Document document;

        public bool SelectPinned { get; set; }

        FormCollection forms = Application.OpenForms;
        private object _event;

        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                Form3 frm_history = new Form3(commandData);
                frm_history.Show();

                //Form3 frm_3 = new Form3(commandData);
                //frm_3.ShowDialog();

                //if (Application.OpenForms.Count > 1)
                //{
                //    MessageBox.Show("There are other form(s) open");
                //}
                //else
                //{
                //    Form4 frm_4 = new Form4(commandData);
                //    frm_4.Show();
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error No 2 : " + ex.ToString(), "SketsStudio Standard Checker", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return Result.Succeeded;
        }

        #region On Document open event  record the first element 
        public void application_DocumentOpened(object sender, DocumentOpenedEventArgs args)
        {

            //_event.Raise();
        }
        #endregion
        public void application_DocumentClosing(object sender, DocumentClosingEventArgs args)
        {
            try
            {
                Document doc = args.Document;
                //Form3.asd(doc);

               // _event.Raise();
            }
            catch (Exception ex)
            {
                //errorLog = "Error : " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt") + " - DocumentClosing - " + ex.Message;
                Logger_();
            }
        }

        #region write log into text file
        public static void Logger_()
        {
            try
            {
                #region get local path for log file
                string strWorkPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                strWorkPath = Directory.GetParent(strWorkPath).FullName;
                strWorkPath = Directory.GetParent(strWorkPath).FullName + "\\" + Dns.GetHostName();
                string strFileName = "Log" + DateTime.Now.ToString("ddMMyyy") + ".txt";
                string strLogtxtFilePath = System.IO.Path.Combine(strWorkPath, strFileName);
                if (!Directory.Exists(strWorkPath))
                {
                    try
                    {
                        Directory.CreateDirectory(strWorkPath);
                    }
                    catch (Exception)
                    {

                        return;
                    }

                }
                #endregion

                #region delete old txt  files
                string[] files = Directory.GetFiles(strWorkPath, "*.txt");
                Array.Sort(files, StringComparer.InvariantCulture);

                if (files.Length > 5)
                {
                    try
                    {
                        int end = files.Length - 1;

                        for (int i = end; i >= 0; i--)
                        {
                            FileInfo fi = new FileInfo(files[i]);
                            if (fi.CreationTime < DateTime.Now.AddDays(-5))
                            {
                                fi.Delete();
                            }
                            string[] files2 = Directory.GetFiles(strWorkPath, "*.txt");
                            if (files2.Length <= 5)
                            {
                                break;
                            }
                        }

                    }
                    catch (Exception)
                    {

                    }

                }
                #endregion

                #region write error log into text file
                if (File.Exists(strLogtxtFilePath))
                {
                   // File.AppendAllText(strLogtxtFilePath, errorLog + Environment.NewLine);
                }
                else
                {
                    FileInfo fi = new FileInfo(strLogtxtFilePath);
                    using (StreamWriter sw = fi.CreateText())
                    {
                      //  sw.WriteLine(errorLog);
                    }
                }
                #endregion
            }
            catch (Exception)
            {
            }
        }
        #endregion
        private void EscKeyHandler(object arg1, KeyEventArgs args)
        {
            throw new NotImplementedException();
        }
    }
    #endregion
}
