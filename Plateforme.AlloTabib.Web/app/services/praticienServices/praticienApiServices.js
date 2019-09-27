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
            url: patientServiceBase + "praticiens" + "?skip=" + skip + "&take=" + take
        }).then(function(results) {
            return results;
        });
    };

    // Add new praticien.
    var _addPraticien = function (praticien) {

        var praticienServiceAddUrl = praticienServiceBase + 'add';
        return $http({
            method: 'POST',
            url: praticienServiceAddUrl,
            data: praticien
        }).then(function (results) {
            return results;
        });
    };

    var _searchPraticien = function (gouvernerat, specialite, nomPraticien, skip, take) {
        var praticienSearchServiceBase = appSettings.praticienSearch;
       return $http({
            method: 'GET',
            url: praticienSearchServiceBase + "?gouvernerat=" + gouvernerat + "&specialite=" + specialite + "&nomPraticien=" + nomPraticien + "&takeForSearch=" + take + "&skipForSearch=" + skip
        }).then(function (results) {
            return results;
        });
    };

    var _getSpecialitiesGouvernerat = function() {
        var praticienSearchServiceBase = appSettings.praticienSearch;
        return $http({
            method: 'GET',
            url: praticienSearchServiceBase + "/searchfilter"
        }).then(function (results) {
            return results;
        });
    };

    var praticienService = appSettings.getPraticienByEmail;
    var _getPraticienByEmail = function (email) {

        return $http({
            method: 'GET',
            url: praticienService + "?email=" + email
        }).then(function (results) {
            return results;
        });
    };

    var praticienService = appSettings.getPraticienByNomPrenom;
    var _getPraticienByNomPrenom = function (email) {

        return $http({
            method: 'GET',
            url: praticienService + "?nomPrenom=" + email
        }).then(function (results) {
            return results;
        });
    };

    /****************************************** Public Methods. ******************************************
  /*****************************************************************************************************/
    praticienApiServiceDataFactory.getSpecialitiesGouvernerat = _getSpecialitiesGouvernerat;
    praticienApiServiceDataFactory.searchPraticien = _searchPraticien;
    praticienApiServiceDataFactory.getPraticiens = _getPraticiens;
    praticienApiServiceDataFactory.addPraticien = _addPraticien;
    praticienApiServiceDataFactory.getPraticienByEmail = _getPraticienByEmail;
    praticienApiServiceDataFactory.getPraticienByNomPrenom = _getPraticienByNomPrenom;

    // Return.
    return praticienApiServiceDataFactory;
});