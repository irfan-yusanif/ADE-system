var TicketModel = function () {
    var self = this;
    self.Id = ko.observable();
    self.CurrentColumnName = ko.observable();
    self.NextColumnName = ko.observable();


    self.IsDeleted = ko.observable();
    self.DeleterUserId = ko.observable();
    self.DetetionTime = ko.observable();
    self.LastModificationTime = ko.observable();
    self.LastModifierUserId = ko.observable();
    self.CreationTime = ko.observable();
    self.CreatorUserId = ko.observable();

    //self.CustomerTypeId = ko.observable().extend({ required: { message: 'Attendee Type is required' } });
    //self.Estimated = ko.observable().extend({ required: { message: 'Estimated Attendee is required' } });
}