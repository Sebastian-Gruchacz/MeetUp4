namespace MeetUp.Model
{
    using System.Diagnostics.CodeAnalysis;

    [SuppressMessage("ReSharper", "VirtualMemberCallInConstructor")] // Creating default, empty collections for new entities
    public partial class Department
    {
        public string InternalEmail { get; set; }
    }
}