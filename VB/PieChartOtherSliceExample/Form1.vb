Imports DevExpress.DashboardCommon
Imports DevExpress.DashboardCommon.ViewerData
Imports DevExpress.DashboardWin
Imports DevExpress.XtraEditors
Imports System.Collections.Generic
Imports System.IO
Imports System.Linq

Namespace PieChartOtherSliceExample
    Partial Public Class Form1
        Inherits XtraForm

        ' Stores the selection state. 
        ' The parameter modification resets the selection. This variable is used to restore it.
        Private selectionState As New List(Of AxisPoint)()
        Public Sub New()
            InitializeComponent()
            AddHandler dashboardViewer1.ConfigureDataConnection, AddressOf dashboardViewer1_ConfigureDataConnection

            AddHandler dashboardViewer1.DashboardItemVisualInteractivity, AddressOf dashboardViewer1_DashboardItemVisualInteractivity
            AddHandler dashboardViewer1.DashboardItemSelectionChanged, AddressOf dashboardViewer1_DashboardItemSelectionChanged

            dashboardViewer1.LoadDashboard("Dashboard.xml")
        End Sub

        Private Sub dashboardViewer1_DashboardItemVisualInteractivity(ByVal sender As Object, ByVal e As DashboardItemVisualInteractivityEventArgs)
            If e.DashboardItemName = "gridDashboardItem1" Then
                e.SelectionMode = DashboardSelectionMode.Multiple
                e.SetDefaultSelection(selectionState)
            End If
        End Sub
        Private Sub dashboardViewer1_DashboardItemSelectionChanged(ByVal sender As Object, ByVal e As DashboardItemSelectionChangedEventArgs)
            Dim viewer As DashboardViewer = DirectCast(sender, DashboardViewer)
            If e.DashboardItemName = "gridDashboardItem1" Then
                selectionState = e.CurrentSelection.Select(Function(tuple) tuple.GetAxisPoint(DashboardDataAxisNames.DefaultAxis)).ToList()
                Dim stringSelection As IEnumerable(Of String) = selectionState.Select(Function(p) p.Value).Cast(Of String)()
                viewer.Parameters("ParamSalesPerson").SelectedValues = stringSelection
            End If
        End Sub
        Private Sub dashboardViewer1_ConfigureDataConnection(ByVal sender As Object, ByVal e As DashboardConfigureDataConnectionEventArgs)
            Dim parameters As ExtractDataSourceConnectionParameters = TryCast(e.ConnectionParameters, ExtractDataSourceConnectionParameters)
            If parameters IsNot Nothing Then
                parameters.FileName = Path.GetFileName(parameters.FileName)
            End If
        End Sub
    End Class
End Namespace
