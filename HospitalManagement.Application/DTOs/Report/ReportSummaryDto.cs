namespace HospitalManagement.Application.DTOs.Report
{
    public class ReportSummaryDto
    {
        public int TotalPatients { get; set; }
        public int TotalDoctors { get; set; }
        public int TotalAppointments { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}