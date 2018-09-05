namespace MeetUp.Common
{
    using System;

    // Restore GuidGen in VS 2017: https://social.technet.microsoft.com/wiki/contents/articles/33047.create-guid-tool-in-visual-studio.aspx

    /// <summary>
    /// This class defines system users identifiers
    /// </summary>
    public static class SystemUsers
    {
        public static Guid Migrator => new Guid(@"{C8CC2EFF-F387-45ED-9FDB-2E0199CBE12E}");

        public static Guid OrderService => new Guid(@"{5AED9D6E-6425-4CAE-AFCF-B3C81674F6EA}");

        public static Guid OrderFormService => new Guid(@"{F6E4F96C-1855-4276-98BB-1FBDCDEF911A}");

        public static Guid EmailingService => new Guid(@"{1617B50F-71A6-4ACD-8365-434C5A8AE84F}");
    }
}
