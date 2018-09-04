
1. Enabling migrations (with context with another assembly):

	Enable-Migrations -ContextTypeName MeetupDbContext -ContextProjectName MeetUp.Model



2. Current Errors in model to resolve:

	MeetUp.Model.CustomerOrderAttachment: : EntityType 'CustomerOrderAttachment' has no key defined. Define the key for this EntityType.
	MeetUp.Model.UserCustomerRole: : EntityType 'UserCustomerRole' has no key defined. Define the key for this EntityType.
	MeetUp.Model.Department: : EntityType 'Department' has no key defined. Define the key for this EntityType.
	MeetUp.Model.MailAttachment: : EntityType 'MailAttachment' has no key defined. Define the key for this EntityType.
	MeetUp.Model.SupplierOrderPolicy: : EntityType 'SupplierOrderPolicy' has no key defined. Define the key for this EntityType.
	CustomerOrderAttachments: EntityType: EntitySet 'CustomerOrderAttachments' is based on type 'CustomerOrderAttachment' that has no keys defined.
	UserCustomerRoles: EntityType: EntitySet 'UserCustomerRoles' is based on type 'UserCustomerRole' that has no keys defined.
	Departments: EntityType: EntitySet 'Departments' is based on type 'Department' that has no keys defined.
	MailMessageAttachmentss: EntityType: EntitySet 'MailMessageAttachmentss' is based on type 'MailAttachment' that has no keys defined.
	SupplierOrderPolicies: EntityType: EntitySet 'SupplierOrderPolicies' is based on type 'SupplierOrderPolicy' that has no keys defined.