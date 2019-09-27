'use strict';
//Add factory 
app.factory('calendrierApiServices', function ($http, appSettings) {

    var calendrierServiceBase = appSettings.calendrierUri;
    
    //declare factory object for this service
    var calendrierApiServiceDataFactory = {};

    /****************************************** Private Methods. ******************************************
    /*****************************************************************************************************/
    var _heures = function (praticiencin, dateCourante) {

        var calendrierServiceAddUrl = calendrierServiceBase + '?praticien=' + praticiencin + '&dateCourante=' + dateCourante;
        return $http({
            method: 'GET',
            url: calendrierServiceAddUrl,
        }).then(function (results) {
            return results;
        });
    };

    /****************************************** Public Methods. ******************************************
    /*****************************************************************************************************/
    var _patients = function (praticiencin, skip, take) {

        var patientServiceUrl = appSettings.rendezvousUri + "?praticien=" + praticiencin + '&take=' + take + "&skip=" + skip;
        return $http({
            method: 'GET',
            url: patientServiceUrl,
        }).then(function (results) {
            return results;
        });
    };

    var _ajouterJourFerie = function (jourferie) {
        var jourFerieUrl = appSettings.jourFerieURi + "/new";
        return $http({
            method: 'POST',
            url: jourFerieUrl,
            data: jourferie
        }).then(function (results) {
            return results;
        });
    };

    var _supprimerJourFerie = function (jourferie,email) {
        var jourFerieUrl = appSettings.jourFerieURi + "/delete" + "?jourName=" + jourferie + "&email=" + email;
        return $http({
            method: 'GET',
            url: jourFerieUrl,
        }).then(function (results) {
            return results;
        });
    };

    var _estFerie = function (jourferie, email) {
        var ferieServiceUrl = appSettings.jourFerieURi + "/estferie" + "?jourferie=" + jourferie + "&email=" + email;
        return $http({
            method: 'GET',
            url: ferieServiceUrl,
        }).then(function (results) {
            return results;
        });
    };

    var _getRendezVous = function (praticien, dateCurrent) {
        var rendezVousServiceUrl = appSettings.rendezvous + "getrendezvous" + "?praticien=" + praticien + "&dateCurrent=" + dateCurrent;
        return $http({
            method: 'GET',
            url: rendezVousServiceUrl,
        }).then(function (results) {
            return results;

        });
    };

    var _ajouterCreneau = function (creneauToAdd) {
        var creneauServiceUrl = appSettings.creneauUri + "/new";
        return $http({
            method: 'POST',
            url: creneauServiceUrl,
            data: creneauToAdd
        }).then(function (results) {
            return results;
        });
    };

    var _creneauAyantRdv = function (praticien, dateCurrent, heureDebut) {//creneauhasrdv
        var rendezVousServiceUrl = appSettings.rendezvous + "creneauhasrdv" + "?praticien=" + praticien + "&dateCurrent=" + dateCurrent + "&heureDebut=" + heureDebut;
        return $http({
            method: 'GET',
            url: rendezVousServiceUrl,
        }).then(function (results) {
            return results;

        });
    };
    // Update RDV.
    var _updateRendezVous = function (rendezvous) {
        return $http({
            method: 'PATCH',
            url: appSettings.rendezvous + "update",
            data: rendezvous
        }).then(function (results) {
            return results;
        });
    };

    //Get list of RDVs not confirmed
    var _getAllRendezVousNonConfirmeOuRejete = function (praticien) {

        return $http({
            method: 'GET',
            url: appSettings.rendezvous + "rendezvousnonconfirme?praticien=" + praticien
        }).then(function (results) {
            return results;

        });
    };


    calendrierApiServiceDataFactory.updateRendezVous = _updateRendezVous;
    calendrierApiServiceDataFactory.creneauAyantRdv = _creneauAyantRdv;
    calendrierApiServiceDataFactory.ajouterCreneau = _ajouterCreneau;
    calendrierApiServiceDataFactory.estFerie = _estFerie;
    calendrierApiServiceDataFactory.getRendezVous = _getRendezVous;
    calendrierApiServiceDataFactory.supprimerJourFerie = _supprimerJourFerie;
    calendrierApiServiceDataFactory.heures = _heures;
    calendrierApiServiceDataFactory.ajouterJourFerie = _ajouterJourFerie;
    calendrierApiServiceDataFactory.patients = _patients;
    calendrierApiServiceDataFactory.getAllRendezVousNonConfirmeOuRejete = _getAllRendezVousNonConfirmeOuRejete;

    // Return.
    return calendrierApiServiceDataFactory;
});
