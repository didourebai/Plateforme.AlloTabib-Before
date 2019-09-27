'use strict';
//Add factory 
app.factory('contactApiServices', function ($http, appSettings) {

    var contactServiceBase = appSettings.contactUri;
    //declare factory object for this service
    var contactApiServiceDataFactory = {};
    /****************************************** Private Methods. ******************************************
   /*****************************************************************************************************/
    // Add new contact.
    var _contacts = function (from, sujet, body) {

        var contactServiceAddUrl = contactServiceBase + 'sendmail?' + 'from=' + from + '&to=contact@allotabib.net'+'&sujet='+ sujet + '&body='+body;
        return $http({
            method: 'GET',
            url: contactServiceAddUrl,
        }).then(function (results) {
            return results;
        });
    };

    var _addMail = function(contact) {
        var contactServiceAddUrl = contactServiceBase + 'new';
        return $http({
            method: 'POST',
            url: contactServiceAddUrl,
            data: contact
        }).then(function (results) {
            return results;
        });
    };
    var _sendEmail = function (contact) {
        var contactServiceAddUrl = contactServiceBase + "sendpassword";
        return $http({
            method: 'POST',
            url: contactServiceAddUrl,
            data: contact
        }).then(function (results) {
            return results;
        });
    };
    /****************************************** Public Methods. ******************************************
   /*****************************************************************************************************/
    contactApiServiceDataFactory.sendEmail = _sendEmail;
    contactApiServiceDataFactory.contacts = _contacts;
    contactApiServiceDataFactory.addMail = _addMail;
    // Return.
    return contactApiServiceDataFactory;
});