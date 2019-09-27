'use strict';
//Add factory 
app.factory('authService', function ($http, appSettings) {

    var serviceBase = appSettings.userIsAuth;
    //declare factory object for this service
    var authServiceFactory = {};

    // Add new praticien.
    var _isUserAuthenticated = function (loginData) {

        return $http({
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            url: serviceBase,
            data: loginData
        }).then(function (results) {
            return results;
        });
    };


    // Add new contact.
    var _sendEmailMotDePasseO = function (username) {
        var accountService = appSettings.userAccount;
        var contactServiceAddUrl = accountService + 'motdepasseoublie?' + 'username=' + username;
        return $http({
            method: 'GET',
            url: contactServiceAddUrl,
        }).then(function (results) {
            return results;
        });
    };
    /****************************************** Public Methods. ******************************************
  /*****************************************************************************************************/


    authServiceFactory.isUserAuthenticated = _isUserAuthenticated;
    authServiceFactory.sendEmailMotDePasseO = _sendEmailMotDePasseO;

    // Return.
    return authServiceFactory;
});