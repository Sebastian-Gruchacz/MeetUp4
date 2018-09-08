namespace MeetUp.Model
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Diagnostics.CodeAnalysis;

    using Common;

    /// <summary>
    /// Department is part of customer with separate address and order flow, yet shared payments flow (Not implemented in example)
    /// </summary>
    [SuppressMessage("ReSharper", "VirtualMemberCallInConstructor")] // Creating default, empty collections for new entities
    public partial class Department : ITrackeable
    {
        [Key]
        public int Id { get; set; }

        [StringLength(150)]
        public string Name { get; set; }

        /// <summary>
        /// This is internal email that is used to extract direct emails from catch-all in-box.
        /// </summary>
        [StringLength(250)]
        public string InternalEmail { get; set; }

        /// <inheritdoc />
        public EntityTracker Tracking { get; set; }
    }
}