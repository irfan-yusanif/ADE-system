var TicketsViewModel = function () {
    var self = this;
    self.Tickets = ko.observableArray([new TicketModel()]);
    self.ResponseStatus = ko.observable();
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
                        var tags = $.map(response.Entity, function (item) {

                            // return new Tag1(item);
                        });
                        self.showTags(tags);
                    }

                    $.each((self.showBidings()), function (i, item) {
                        if (item.postedById == $loginUserId) {
                            isalreadybid = true;
                        }
                    });

                    self.showBidings.push(new Biding(data));

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
               
                $.unblockUI();
            },
            error: function () {
                toastr.error("Some Error Occurred");
            }
        });
    }

}