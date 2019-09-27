'use strict';
//Add factory 
app.factory('rendezvousApiServices', function ($http, appSettings) {

    var rendezvousServiceBase = appSettings.calendrierUri;
    //declare factory object for this service
    var rendezvousApiServiceDataFactory = {};

    ////liste des rendezvouss : ajouter skip et take dans la méthode en controller
    var _calendrierPourPatient = function (praticien, dateCourante) {
       
        return $http({
            method: 'GET',
            url: rendezvousServiceBase + "?praticien=" + praticien + "&dateCourante=" + dateCourante
        }).then(function (results) {
            return results;
        });
    };

    // Add new praticien.
    var _addRendezVous = function (rendezvous) {

        var rendezvousServiceAddUrl = appSettings.rendezVousPatientUri + '/new';
        return $http({
            method: 'POST',
            url: rendezvousServiceAddUrl,
            data: rendezvous
        }).then(function (results) {
            return results;
        });
    };
    
    ////liste des rendezvouss : ajouter skip et take dans la méthode en controller
    var _calendrierPourPatientPrevious = function (praticien, dateCourante) {

        return $http({
            method: 'GET',
            url: appSettings.calendrierSemUri + "?praticien=" + praticien + "&dateCourante=" + dateCourante
        }).then(function (results) {
            return results;
        });
    };


    var _getAllRendezVous = function (email) {
      
        return $http({
            method: 'GET',
            url: appSettings.patientUri + "/getrendezvous?email=" + email
        }).then(function (results) {
            return results;
        });
    };

    var _getFirstDayDisponibity = function (praticien, dateCourante) {
        return $http({
            method: 'GET',
            url: appSettings.calendrierJourDisp + "?praticien=" + praticien + "&dateCourante=" + dateCourante
        }).then(function (results) {
            return results;
        });
    };

    /****************************************** Public Methods. ******************************************
  /*****************************************************************************************************/
    rendezvousApiServiceDataFactory.getFirstDayDisponibity = _getFirstDayDisponibity;
    rendezvousApiServiceDataFactory.getAllRendezVous = _getAllRendezVous;
    rendezvousApiServiceDataFactory.calendrierPourPatient = _calendrierPourPatient;
    rendezvousApiServiceDataFactory.calendrierPourPatientPrevious = _calendrierPourPatientPrevious;
    rendezvousApiServiceDataFactory.addRendezVous = _addRendezVous;
    //rendezvousApiServiceDataFactory.addPraticien = _addPraticien;

    // Return.
    return rendezvousApiServiceDataFactory;
});