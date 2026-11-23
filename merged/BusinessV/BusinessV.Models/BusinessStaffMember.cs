namespace BusinessV.Models
{
	public sealed class BusinessStaffMember
	{
		public string Id { get; set; } = string.Empty;


		public BusinessStaffRole Role { get; set; }

		public int DailySalary { get; set; }
	}
}
