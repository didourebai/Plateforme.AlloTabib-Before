'use strict';
//Add factory 
app.factory('praticienApiServices', function($http, appSettings) {

    var praticienServiceBase = appSettings.praticienUri;

    //declare factory object for this service
    var praticienApiServiceDataFactory = {};

    //liste des praticiens : ajouter skip et take dans la méthode en controller
    var _getPraticiens = function(skip, take) {

        return $http({
            method: 'GET',
            url: praticienServiceBase + "praticiens" + "?skip=" + skip + "&take=" + take
        }).then(function(results) {
            return results;
        });
    };

    // Add new praticien.
    var _addPraticien = function (praticien) {

        var praticienServiceAddUrl = praticienServiceBase + 'new';
        return $http({
            method: 'POST',
            url: praticienServiceAddUrl,
            data: praticien
        }).then(function (results) {
            return results;
        });
    };
    var praticienService = appSettings.getPraticienByEmail;
    var _getPraticienByEmail = function(email) {
        
        return $http({
            method: 'GET',
            url: praticienService + "?email=" + email
        }).then(function (results) {
            return results;
        });
    };


    //update a praticien
    var _updatePraticien = function (praticien) {

        var praticienServiceAddUrl = praticienServiceBase + 'update';
        return $http({
            method: 'PATCH',
            url: praticienServiceAddUrl,
            data: praticien
        }).then(function (results) {
            return results;
        });
    };

    /****************************************** Public Methods. ******************************************
  /*****************************************************************************************************/

    praticienApiServiceDataFactory.getPraticiens = _getPraticiens;
    praticienApiServiceDataFactory.getPraticienByEmail = _getPraticienByEmail;
    praticienApiServiceDataFactory.addPraticien = _addPraticien;
    praticienApiServiceDataFactory.updatePraticien = _updatePraticien;

    // Return.
    return praticienApiServiceDataFactory;
});