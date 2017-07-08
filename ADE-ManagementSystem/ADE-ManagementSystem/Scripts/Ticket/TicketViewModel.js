var ticketVM;

var TicketsViewModel = function () {
    var self = this;
    self.Tickets = ko.observableArray();
    self.ToDoTickets = ko.observableArray();
    self.InProgressTickets = ko.observableArray();
    self.ResponseStatus = ko.observable("abc");
    self.ErrorMessage = ko.observable();

    self.Initialize = function () {
        $.blockUI();
        $.ajax({
            url: "/Ticket/GetBaseData",
            method: "GET",
            //data: { venueId: window.VenueId, eventId: window.EventId },
            contentType: "application/json",
            success: function (response) {
                if (response.Success) {
                    
                    if (response.Entity != null) {

                        ko.mapping.fromJS(response.Entity, TicketModel, self.Tickets);
                       // ko.mapping.fromJS(response.Entity, {}, self.Tickets); also works

                        //var ticketObjectsList = $.map(response.Entity, function (item) {
                        //    ko.mapping.fromJS(response.Entity, {
                        //        key: function (data) {
                        //            return ko.utils.unwrapObservable(data.id);
                        //        }
                        //    },TicketModel);
                        //    //return new TicketModel();
                        //});
                        //self.Tickets(ticketObjectsList);
                    }

                    //$.each((self.showBidings()), function (i, item) {
                    //    if (item.postedById == $loginUserId) {
                    //        isalreadybid = true;
                    //    }
                    //});

                    //self.showBidings.push(new Biding(data));

                } else {
                    toastr.error("Error while loading tickets!");
                    console.log(response.Message);
                }
                //ko.mapping.fromJS(result.Titles, {
                //    key: function (data) {
                //        return ko.utils.unwrapObservable(data.id);
                //    },
                //    create: function (options) {
                //        return new TitleModel(options.data.Id, options.data.Name);
                //    }
                //}, clientInfoVm.Titles);
                self.CategorizeTickets();

                $.unblockUI();
            },
            error: function (response) {
                $.unblockUI();
                toastr.error("Some Error Occurred");
                console.log(response.responseText);
            }
        });
    }

    self.CategorizeTickets = function () {
        $.each((self.Tickets()), function (i, ticket) {
            if (ticket.CurrentColumnName() == "ToDo") {
                self.ToDoTickets.push(ticket);
            } else if (ticket.CurrentColumnName() == "InProgress") {
                self.InProgressTickets.push(ticket);
            }
        });
    }

    self.Initialize();

}