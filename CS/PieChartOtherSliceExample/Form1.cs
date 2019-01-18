using DevExpress.DashboardCommon;
using DevExpress.DashboardCommon.ViewerData;
using DevExpress.DashboardWin;
using DevExpress.XtraEditors;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PieChartOtherSliceExample
{
    public partial class Form1 : XtraForm
    {
        // Stores the selection state. 
        // The parameter modification resets the selection. This variable is used to restore it.
        List<AxisPoint> selectionState = new List<AxisPoint>();
        public Form1()
        {
            InitializeComponent();
            dashboardViewer1.ConfigureDataConnection += dashboardViewer1_ConfigureDataConnection;

            dashboardViewer1.DashboardItemVisualInteractivity += dashboardViewer1_DashboardItemVisualInteractivity;
            dashboardViewer1.DashboardItemSelectionChanged += dashboardViewer1_DashboardItemSelectionChanged;

            dashboardViewer1.LoadDashboard("Dashboard.xml");
        }

        private void dashboardViewer1_DashboardItemVisualInteractivity(object sender, DashboardItemVisualInteractivityEventArgs e) {
            if(e.DashboardItemName == "gridDashboardItem1") {
                e.SelectionMode = DashboardSelectionMode.Multiple;
                e.SetDefaultSelection(selectionState);
            }
        }  
        private void dashboardViewer1_DashboardItemSelectionChanged(object sender, DashboardItemSelectionChangedEventArgs e) {
            DashboardViewer viewer = (DashboardViewer)sender;
            if(e.DashboardItemName == "gridDashboardItem1") {
                selectionState = e.CurrentSelection.Select(tuple => tuple.GetAxisPoint(DashboardDataAxisNames.DefaultAxis)).ToList();
                IEnumerable<string> stringSelection = selectionState.Select(p => p.Value).Cast<string>();
                viewer.Parameters["ParamSalesPerson"].SelectedValues = stringSelection;
            }
        }
        private void dashboardViewer1_ConfigureDataConnection(object sender, DashboardConfigureDataConnectionEventArgs e)
        {
            ExtractDataSourceConnectionParameters parameters = e.ConnectionParameters as ExtractDataSourceConnectionParameters;
            if (parameters != null)
                parameters.FileName = Path.GetFileName(parameters.FileName);
        }
    }
}
