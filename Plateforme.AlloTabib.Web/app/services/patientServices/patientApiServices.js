'use strict';
//Add factory 
app.factory('patientApiServices', function ($http, appSettings) {

    var patientServiceBase = appSettings.patientUri;
    //declare factory object for this service
    var patientApiServiceDataFactory = {};
    /****************************************** Private Methods. ******************************************
   /*****************************************************************************************************/
    // Add new patient.
    var _addPatient = function (patient) {

        var patientServiceAddUrl = patientServiceBase + 'new';
        return $http({
            method: 'POST',
            url: patientServiceAddUrl,
            data: patient
        }).then(function (results) {
            return results;
        });
    };

    var _updatePatient = function (patient) {
        console.log(patient);
        var patientServiceAddUrl = patientServiceBase + 'update';
        return $http({
            method: 'PATCH',
            url: patientServiceAddUrl,
            data: patient
        }).then(function (results) {
            return results;
        });
    };

    var patientGetEmailService = appSettings.getPatientByEmail;
    var _getPatientByEmail = function (email) {

        return $http({
            method: 'GET',
            url: patientGetEmailService + "?email=" + email
        }).then(function (results) {
            return results;
        });
    };


    /****************************************** Public Methods. ******************************************
   /*****************************************************************************************************/

    patientApiServiceDataFactory.updatePatient = _updatePatient;
    patientApiServiceDataFactory.addPatient = _addPatient;
    patientApiServiceDataFactory.getPatientByEmail = _getPatientByEmail;
    // Return.
    return patientApiServiceDataFactory;
});